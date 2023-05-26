using Android.Content;
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

	static Android.Webkit.WebView Create(Context context)
	{
		var view = new Android.Webkit.WebView(context);
		view.SetWebViewClient(new SpiceWebViewClient());
		view.Settings.JavaScriptEnabled = true; // This is the default
		return view;
	}

	/// <summary>
	/// A "web view" for rendering HTML content on each platform.
	/// </summary>
	public WebView() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public WebView(Context context) : this(context, Create) { }

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

		public SpiceWebViewClient(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView? view, IWebResourceRequest? request)
		{
			// Return false for http/https to navigate to the URL within the WebView
			// Otherwise it launches the default browser from the OS
			var scheme = request?.Url?.Scheme;
			return scheme != "http" && scheme != "https";
		}
	}
}