using System.Globalization;
using System.Runtime.Versioning;

using WebKit;

namespace Spice;

public partial class BlazorWebView
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view">The Spice.BlazorWebView</param>
	public static implicit operator WKWebView(BlazorWebView view) => view.NativeView;

	internal const string AppOrigin = "app://" + BlazorWebView.AppHostAddress + "/";
	internal static readonly Uri AppOriginUri = new(AppOrigin);
	const string BlazorInitScript = @"
		window.__receiveMessageCallbacks = [];
		window.__dispatchMessageCallback = function(message) {
			window.__receiveMessageCallbacks.forEach(function(callback) { callback(message); });
		};
		window.external = {
			sendMessage: function(message) {
				window.webkit.messageHandlers.webwindowinterop.postMessage(message);
			},
			receiveMessage: function(callback) {
				window.__receiveMessageCallbacks.push(callback);
			}
		};

		Blazor.start();

		(function () {
			window.onpageshow = function(event) {
				if (event.persisted) {
					window.location.reload();
				}
			};
		})();
	";

	static new WKWebViewConfiguration CreateConfiguration()
	{
		var config = WebView.CreateConfiguration();

		config.UserContentController.AddScriptMessageHandler(new WebViewScriptMessageHandler(), "webwindowinterop");
		config.UserContentController.AddUserScript(new WKUserScript(
			new NSString(BlazorInitScript), WKUserScriptInjectionTime.AtDocumentEnd, true));

		// iOS WKWebView doesn't allow handling 'http'/'https' schemes, so we use the fake 'app' scheme
		config.SetUrlSchemeHandler(new SchemeHandler(), urlScheme: "app");

		return config;
	}

	/// <summary>
	/// A "web view" for rendering HTML content on each platform.
	/// Android -> Android.Webkit.WebView
	/// iOS -> WebKit.WKWebView
	/// </summary>
	public BlazorWebView() : base(_ => Create(CGRect.Empty, CreateConfiguration))
	{
		//TODO: do something better than this
		if (NativeView.Configuration.GetUrlSchemeHandler(urlScheme: "app") is SchemeHandler s)
		{
			s.WebView = this;
		}

		Initialize();
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public BlazorWebView(CGRect frame) : base(_ => Create(frame, CreateConfiguration))
	{
		//TODO: do something better than this
		if (NativeView.Configuration.GetUrlSchemeHandler(urlScheme: "app") is SchemeHandler s)
		{
			s.WebView = this;
		}

		Initialize();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected BlazorWebView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying WebKit.WKWebView
	/// </summary>
	public new WKWebView NativeView => (WKWebView)_nativeView.Value;

	static iOSFileProvider? _fileProvider;
	iOSWebViewManager? _webViewManager;

	internal iOSWebViewManager? Manager => _webViewManager;

	partial void LoadNativeWebView(string contentRootDir, string hostPageRelativePath)
	{
		if (_fileProvider is null)
		{
			_fileProvider = new iOSFileProvider(contentRootDir);
		}
		_webViewManager = new iOSWebViewManager(NativeView, SpiceServiceProvider.Instance, SpiceDispatcher.Instance, _fileProvider, _jSComponents, contentRootDir, hostPageRelativePath);

		foreach (var component in RootComponents)
		{
			// Since the page isn't loaded yet, this will always complete synchronously
			_ = component.AddToWebViewManagerAsync(_webViewManager);
		}

		_webViewManager.Navigate(StartPath);
	}

	private class WebViewScriptMessageHandler : NSObject, IWKScriptMessageHandler
	{
		public BlazorWebView? WebView { get; set; }

		public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
		{
			ArgumentNullException.ThrowIfNull(message);
			WebView?.Manager?.MessageReceivedInternal(AppOriginUri, ((NSString)message.Body).ToString());
		}
	}

	private class SchemeHandler : NSObject, IWKUrlSchemeHandler
	{
		public BlazorWebView? WebView { get; set; }

		[Export("webView:startURLSchemeTask:")]
		[SupportedOSPlatform("ios11.0")]
		public void StartUrlSchemeTask(WKWebView webView, IWKUrlSchemeTask urlSchemeTask)
		{
			var responseBytes = GetResponseBytes(urlSchemeTask.Request.Url?.AbsoluteString ?? "", out var contentType, statusCode: out var statusCode);
			if (statusCode == 200)
			{
				using (var dic = new NSMutableDictionary<NSString, NSString>())
				{
					dic.Add((NSString)"Content-Length", (NSString)(responseBytes.Length.ToString(CultureInfo.InvariantCulture)));
					dic.Add((NSString)"Content-Type", (NSString)contentType);
					// Disable local caching. This will prevent user scripts from executing correctly.
					dic.Add((NSString)"Cache-Control", (NSString)"no-cache, max-age=0, must-revalidate, no-store");
					if (urlSchemeTask.Request.Url != null)
					{
						using var response = new NSHttpUrlResponse(urlSchemeTask.Request.Url, statusCode, "HTTP/1.1", dic);
						urlSchemeTask.DidReceiveResponse(response);
					}

				}
				urlSchemeTask.DidReceiveData(NSData.FromArray(responseBytes));
				urlSchemeTask.DidFinish();
			}
		}

		byte[] GetResponseBytes(string? url, out string contentType, out int statusCode)
		{
			var allowFallbackOnHostPage = AppOriginUri.IsBaseOfPage(url);
			url = UriExtensions.RemovePossibleQueryString(url);

			if (WebView?.Manager is iOSWebViewManager m && m.TryGetResponseContentInternal(url, allowFallbackOnHostPage, out statusCode, out var statusMessage, out var content, out var headers))
			{
				statusCode = 200;
				using var ms = new MemoryStream();

				content.CopyTo(ms);
				content.Dispose();

				contentType = headers["Content-Type"];
				return ms.ToArray();
			}
			else
			{
				statusCode = 404;
				contentType = string.Empty;
				return Array.Empty<byte>();
			}
		}

		[Export("webView:stopURLSchemeTask:")]
		public void StopUrlSchemeTask(WKWebView webView, IWKUrlSchemeTask urlSchemeTask)
		{
		}
	}
}

