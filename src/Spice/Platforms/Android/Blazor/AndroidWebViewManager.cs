// From: https://github.com/dotnet/maui/blob/260fa08b0f75fbbb945375592896dd9eb374f22f/src/BlazorWebView/src/Maui/Android/AndroidWebKitWebViewManager.cs

using System.Runtime.Versioning;

using Android.Webkit;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

using Spice;

using AUri = Android.Net.Uri;
using AWebView = Android.Webkit.WebView;

namespace Microsoft.AspNetCore.Components.WebView.Maui
{
	/// <summary>
	/// An implementation of <see cref="WebViewManager"/> that uses the Android WebKit WebView browser control
	/// to render web content.
	/// </summary>
	[SupportedOSPlatform("android23.0")]
	internal class AndroidWebViewManager : WebViewManager
	{
		// Using an IP address means that WebView doesn't wait for any DNS resolution,
		// making it substantially faster. Note that this isn't real HTTP traffic, since
		// we intercept all the requests within this origin.
		private static readonly string AppOrigin = $"https://{BlazorWebView.AppHostAddress}/";
		private static readonly Uri AppOriginUri = new(AppOrigin);
		private static readonly AUri AndroidAppOriginUri = AUri.Parse(AppOrigin)!;
		private readonly AWebView _webview;
		private readonly string _contentRootRelativeToAppRoot;

		/// <summary>
		/// Constructs an instance of <see cref="AndroidWebViewManager"/>.
		/// </summary>
		/// <param name="webview">A wrapper to access platform-specific WebView APIs.</param>
		/// <param name="services">A service provider containing services to be used by this class and also by application code.</param>
		/// <param name="dispatcher">A <see cref="Dispatcher"/> instance that can marshal calls to the required thread or sync context.</param>
		/// <param name="fileProvider">Provides static content to the webview.</param>
		/// <param name="jsComponents">Describes configuration for adding, removing, and updating root components from JavaScript code.</param>
		/// <param name="contentRootRelativeToAppRoot">Path to the directory containing application content files.</param>
		/// <param name="hostPageRelativePath">Path to the host page within the <paramref name="fileProvider"/>.</param>
		public AndroidWebViewManager(AWebView webview, IServiceProvider services, Dispatcher dispatcher, IFileProvider fileProvider, JSComponentConfigurationStore jsComponents, string contentRootRelativeToAppRoot, string hostPageRelativePath)
			: base(services, dispatcher, AppOriginUri, fileProvider, jsComponents, hostPageRelativePath)
		{
			ArgumentNullException.ThrowIfNull(webview);
			_webview = webview;
			_contentRootRelativeToAppRoot = contentRootRelativeToAppRoot;
		}

		/// <inheritdoc />
		protected override void NavigateCore(Uri absoluteUri)
		{
			_webview.LoadUrl(absoluteUri.AbsoluteUri);
		}

		/// <inheritdoc />
		protected override void SendMessage(string message)
		{
			_webview.PostWebMessage(new WebMessage(message), AndroidAppOriginUri);
		}

		internal bool TryGetResponseContentInternal(string uri, bool allowFallbackOnHostPage, out int statusCode, out string statusMessage, out Stream content, out IDictionary<string, string> headers)
		{
			return TryGetResponseContent(uri, allowFallbackOnHostPage, out statusCode, out statusMessage, out content, out headers);

			//TODO: hot reload support?
			//var hotReloadedResult = StaticContentHotReloadManager.TryReplaceResponseContent(_contentRootRelativeToAppRoot, uri, ref statusCode, ref content, headers);
			//return defaultResult || hotReloadedResult;
		}

		internal void SetUpMessageChannel()
		{
			// These ports will be closed automatically when the webview gets disposed.
			var nativeToJSPorts = _webview.CreateWebMessageChannel();

			var nativeToJs = new BlazorWebMessageCallback(message =>
			{
				MessageReceived(AppOriginUri, message!);
			});

			var destPort = new[] { nativeToJSPorts[1] };

			nativeToJSPorts[0].SetWebMessageCallback(nativeToJs);

			_webview.PostWebMessage(new WebMessage("capturePort", destPort), AndroidAppOriginUri);
		}

		private class BlazorWebMessageCallback : WebMessagePort.WebMessageCallback
		{
			private readonly Action<string?> _onMessageReceived;

			public BlazorWebMessageCallback(Action<string?> onMessageReceived)
			{
				_onMessageReceived = onMessageReceived ?? throw new ArgumentNullException(nameof(onMessageReceived));
			}

			public override void OnMessage(WebMessagePort? port, WebMessage? message)
			{
				ArgumentNullException.ThrowIfNull(message);
				_onMessageReceived(message.Data);
			}
		}
	}
}