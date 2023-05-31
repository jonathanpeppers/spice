using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Webkit;

namespace Spice;

public partial class WebView
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view">The Spice.WebView</param>
	public static implicit operator Android.Webkit.WebView(WebView view) => view.NativeView;

	internal static Android.Webkit.WebView Create(Context context, WebViewClient webViewClient = null, WebChromeClient webChromeClient = null)
	{
		var view = new Android.Webkit.WebView(context);
		view.SetWebViewClient(webViewClient ?? new SpiceWebViewClient());
		view.SetWebChromeClient(webChromeClient ?? new SpiceWebChromeClient());

		var settings = view.Settings;
		settings.JavaScriptEnabled = true; // This is the default for IsJavaScriptEnabled
		settings.DomStorageEnabled = true;
		settings.SetSupportMultipleWindows(support: true);
		return view;
	}

	/// <summary>
	/// A "web view" for rendering HTML content on each platform.
	/// Android -> Android.Webkit.WebView
	/// iOS -> WebKit.WKWebView
	/// </summary>
	public WebView() : this(Platform.Context, c => Create(c))
	{
		// Most users would want this default instead of center
		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public WebView(Context context) : this(context, c => Create(c))
	{
		// Most users would want this default instead of center
		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected WebView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected WebView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.WebKit.WebView
	/// </summary>
	public new Android.Webkit.WebView NativeView => (Android.Webkit.WebView)_nativeView.Value;

	partial void OnSourceChanged(string value) => NativeView.LoadUrl(value);

	partial void OnIsJavaScriptEnabledChanged(bool value) => NativeView.Settings.JavaScriptEnabled = value;

	/// <summary>
	/// Returns true if you can go back
	/// </summary>
	public bool CanGoBack => NativeView.CanGoBack();

	/// <summary>
	/// Navigates back if possible
	/// </summary>
	public void Back()
	{
		NativeView.GoBack();
		OnPropertyChanged(nameof(CanGoBack));
	}

	/// <summary>
	/// Returns true if you can go forward
	/// </summary>
	public bool CanGoForward => NativeView.CanGoForward();

	/// <summary>
	/// Navigates forward if possible
	/// </summary>
	public void Forward()
	{
		NativeView.GoForward();
		OnPropertyChanged(nameof(CanGoForward));
	}

	class SpiceWebViewClient : WebViewClient
	{
		public SpiceWebViewClient() { }

		protected SpiceWebViewClient(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView? view, IWebResourceRequest? request)
		{
			// Return false for http/https to navigate to the URL within the WebView
			// Otherwise it launches the default browser from the OS
			var scheme = request?.Url?.Scheme;
			return scheme != "http" && scheme != "https";
		}
	}

	class SpiceWebChromeClient : WebChromeClient
	{
		public SpiceWebChromeClient() { }

		protected SpiceWebChromeClient(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		// From: https://github.com/dotnet/maui/blob/260fa08b0f75fbbb945375592896dd9eb374f22f/src/BlazorWebView/src/Maui/Android/BlazorWebChromeClient.cs#L17
		public override bool OnCreateWindow(Android.Webkit.WebView? view, bool isDialog, bool isUserGesture, Message? resultMsg)
		{
			if (view?.Context is not null)
			{
				// Intercept _blank target <a> tags to always open in device browser
				// regardless of UrlLoadingStrategy.OpenInWebview
				var requestUrl = view.GetHitTestResult().Extra;
				var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(requestUrl));
				view.Context.StartActivity(intent);
			}

			// We don't actually want to create a new WebView window so we just return false 
			return false;
		}
	}
}