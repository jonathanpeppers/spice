//From: https://github.com/dotnet/maui/blob/c8a6093ee805388732af8d75b437b099a301db22/src/BlazorWebView/src/Maui/iOS/IOSWebViewManager.cs
using System.Text.Encodings.Web;
using Foundation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using UIKit;
using WebKit;

namespace Spice;

/// <summary>
/// An implementation of <see cref="WebViewManager"/> that uses the <see cref="WKWebView"/> browser control
/// to render web content.
/// </summary>
internal class iOSWebViewManager : WebViewManager
{
	private readonly WKWebView _webview;
	private readonly string _contentRootRelativeToAppRoot;

	/// <summary>
	/// Initializes a new instance of <see cref="iOSWebViewManager"/>
	/// </summary>
	/// <param name="webview">The <see cref="WKWebView"/> to render web content in.</param>
	/// <param name="provider">The <see cref="IServiceProvider"/> for the application.</param>
	/// <param name="dispatcher">A <see cref="Dispatcher"/> instance instance that can marshal calls to the required thread or sync context.</param>
	/// <param name="fileProvider">Provides static content to the webview.</param>
	/// <param name="jsComponents">Describes configuration for adding, removing, and updating root components from JavaScript code.</param>
	/// <param name="contentRootRelativeToAppRoot">Path to the directory containing application content files.</param>
	/// <param name="hostPageRelativePath">Path to the host page within the fileProvider.</param>

	public iOSWebViewManager(WKWebView webview, IServiceProvider provider, Dispatcher dispatcher, IFileProvider fileProvider, JSComponentConfigurationStore jsComponents, string contentRootRelativeToAppRoot, string hostPageRelativePath)
		: base(provider, dispatcher, BlazorWebView.AppOriginUri, fileProvider, jsComponents, hostPageRelativePath)
	{
		ArgumentNullException.ThrowIfNull(nameof(webview));

		_webview = webview;
		_contentRootRelativeToAppRoot = contentRootRelativeToAppRoot;

		InitializeWebView();
	}

	/// <inheritdoc />
	protected override void NavigateCore(Uri absoluteUri)
	{
		using var nsUrl = new NSUrl(absoluteUri.ToString());
		using var request = new NSUrlRequest(nsUrl);
		_webview.LoadRequest(request);
	}

	internal bool TryGetResponseContentInternal(string uri, bool allowFallbackOnHostPage, out int statusCode, out string statusMessage, out Stream content, out IDictionary<string, string> headers)
	{
		return TryGetResponseContent(uri, allowFallbackOnHostPage, out statusCode, out statusMessage, out content, out headers);

		// TODO: hot reload support?
		//var hotReloadedResult = StaticContentHotReloadManager.TryReplaceResponseContent(_contentRootRelativeToAppRoot, uri, ref statusCode, ref content, headers);
		//return defaultResult || hotReloadedResult;
	}

	/// <inheritdoc />
	protected override void SendMessage(string message)
	{
		var messageJSStringLiteral = JavaScriptEncoder.Default.Encode(message);
		_webview.EvaluateJavaScript(
			javascript: $"__dispatchMessageCallback(\"{messageJSStringLiteral}\")",
			completionHandler: (NSObject result, NSError error) => { });
	}

	internal void MessageReceivedInternal(Uri uri, string message)
	{
		MessageReceived(uri, message);
	}

	private void InitializeWebView()
	{
		_webview.NavigationDelegate = new WebViewNavigationDelegate();
		_webview.UIDelegate = new WebViewUIDelegate();
	}

	internal sealed class WebViewUIDelegate : WKUIDelegate
	{
		private static readonly string LocalOK = NSBundle.FromIdentifier("com.apple.UIKit").GetLocalizedString("OK");
		private static readonly string LocalCancel = NSBundle.FromIdentifier("com.apple.UIKit").GetLocalizedString("Cancel");

		public override void RunJavaScriptAlertPanel(WKWebView webView, string message, WKFrameInfo frame, Action completionHandler)
		{
			PresentAlertController(
				webView,
				message,
				okAction: _ => completionHandler()
			);
		}

		public override void RunJavaScriptConfirmPanel(WKWebView webView, string message, WKFrameInfo frame, Action<bool> completionHandler)
		{
			PresentAlertController(
				webView,
				message,
				okAction: _ => completionHandler(true),
				cancelAction: _ => completionHandler(false)
			);
		}

		public override void RunJavaScriptTextInputPanel(
			WKWebView webView, string prompt, string? defaultText, WKFrameInfo frame, Action<string> completionHandler)
		{
			PresentAlertController(
				webView,
				prompt,
				defaultText: defaultText,
				okAction: x => completionHandler(x.TextFields[0].Text!),
				cancelAction: _ => completionHandler(null!)
			);
		}

		private static string GetJsAlertTitle(WKWebView webView)
		{
			// Emulate the behavior of UIWebView dialogs.
			// The scheme and host are used unless local html content is what the webview is displaying,
			// in which case the bundle file name is used.
			if (webView.Url != null && webView.Url.AbsoluteString != $"file://{NSBundle.MainBundle.BundlePath}/")
				return $"{webView.Url.Scheme}://{webView.Url.Host}";

			return new NSString(NSBundle.MainBundle.BundlePath).LastPathComponent;
		}

		private static UIAlertAction AddOkAction(UIAlertController controller, Action handler)
		{
			var action = UIAlertAction.Create(LocalOK, UIAlertActionStyle.Default, (_) => handler());
			controller.AddAction(action);
			controller.PreferredAction = action;
			return action;
		}

		private static UIAlertAction AddCancelAction(UIAlertController controller, Action handler)
		{
			var action = UIAlertAction.Create(LocalCancel, UIAlertActionStyle.Cancel, (_) => handler());
			controller.AddAction(action);
			return action;
		}

		private static void PresentAlertController(
			WKWebView webView,
			string message,
			string? defaultText = null,
			Action<UIAlertController>? okAction = null,
			Action<UIAlertController>? cancelAction = null)
		{
			var controller = UIAlertController.Create(GetJsAlertTitle(webView), message, UIAlertControllerStyle.Alert);

			if (defaultText != null)
				controller.AddTextField((textField) => textField.Text = defaultText);

			if (okAction != null)
				AddOkAction(controller, () => okAction(controller));

			if (cancelAction != null)
				AddCancelAction(controller, () => cancelAction(controller));

#pragma warning disable CA1416, CA1422 // TODO:  'UIApplication.Windows' is unsupported on: 'ios' 15.0 and later
			GetTopViewController(UIApplication.SharedApplication.Windows.FirstOrDefault(m => m.IsKeyWindow)?.RootViewController)?
				.PresentViewController(controller, true, null);
#pragma warning restore CA1416, CA1422
		}

		private static UIViewController? GetTopViewController(UIViewController? viewController)
		{
			if (viewController is UINavigationController navigationController)
				return GetTopViewController(navigationController.VisibleViewController);

			if (viewController is UITabBarController tabBarController)
				return GetTopViewController(tabBarController.SelectedViewController!);

			if (viewController?.PresentedViewController != null)
				return GetTopViewController(viewController.PresentedViewController);

			return viewController;
		}
	}

	internal class WebViewNavigationDelegate : WKNavigationDelegate
	{
		private WKNavigation? _currentNavigation;
		private Uri? _currentUri;

		public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
		{
			_currentNavigation = navigation;
		}

		public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
		{
			var requestUrl = navigationAction.Request.Url;
			var uri = new Uri(requestUrl.ToString());
			bool openExternally = false;

			// TargetFrame is null for navigation to a new window (`_blank`)
			if (navigationAction.TargetFrame is null)
			{
				// Open in a new browser window regardless of UrlLoadingStrategy
				openExternally = true;
			}
			else
			{
				openExternally = !BlazorWebView.AppOriginUri.IsBaseOf(uri);
			}

			if (openExternally)
			{
#pragma warning disable CA1416, CA1422 // TODO: OpenUrl(...) has [UnsupportedOSPlatform("ios10.0")]
				UIApplication.SharedApplication.OpenUrl(requestUrl);
#pragma warning restore CA1416, CA1422

				// Cancel any further navigation as we've either opened the link in the external browser
				// or canceled the underlying navigation action.
				decisionHandler(WKNavigationActionPolicy.Cancel);
				return;
			}

			if (navigationAction.TargetFrame!.MainFrame)
			{
				_currentUri = requestUrl;
			}

			decisionHandler(WKNavigationActionPolicy.Allow);
		}

		public override void DidReceiveServerRedirectForProvisionalNavigation(WKWebView webView, WKNavigation navigation)
		{
			// We need to intercept the redirects to the app scheme because Safari will block them.
			// We will handle these redirects through the Navigation Manager.
			if (_currentUri?.Host == BlazorWebView.AppHostAddress)
			{
				var uri = _currentUri;
				_currentUri = null;
				_currentNavigation = null;
				if (uri is not null)
				{
					var request = new NSUrlRequest(new NSUrl(uri.AbsoluteUri));
					webView.LoadRequest(request);
				}
			}
		}

		public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
		{
			_currentUri = null;
			_currentNavigation = null;
		}

		public override void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, NSError error)
		{
			_currentUri = null;
			_currentNavigation = null;
		}

		public override void DidCommitNavigation(WKWebView webView, WKNavigation navigation)
		{
			if (_currentUri != null && _currentNavigation == navigation)
			{
				// TODO: Determine whether this is needed
				//_webView.HandleNavigationStarting(_currentUri);
			}
		}

		public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
		{
			if (_currentUri != null && _currentNavigation == navigation)
			{
				// TODO: Determine whether this is needed
				//_webView.HandleNavigationFinished(_currentUri);
				_currentUri = null;
				_currentNavigation = null;
			}
		}
	}
}