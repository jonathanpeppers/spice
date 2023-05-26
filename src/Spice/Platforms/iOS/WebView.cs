using WebKit;

namespace Spice;

public partial class WebView
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view">The Spice.WebView</param>
	public static implicit operator WKWebView(WebView view) => view.NativeView;

	static WKWebViewConfiguration CreateConfiguration()
	{
		var config = new WKWebViewConfiguration();
		if (OperatingSystem.IsIOSVersionAtLeast(13))
		{
			config.DefaultWebpagePreferences = new WKWebpagePreferences
			{
				AllowsContentJavaScript = true,
			};
		}
		return config;
	}

	static WKWebView Create(CGRect frame)
	{
		var view = new WKWebView(frame, CreateConfiguration())
		{
			AutoresizingMask = UIViewAutoresizing.None,
			NavigationDelegate = new SpiceNavigationDelegate(),
		};
		return view;
	}

	/// <summary>
	/// A "web view" for rendering HTML content on each platform.
	/// </summary>
	public WebView() : base(_ => Create(Platform.Window!.Frame)) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public WebView(CGRect frame) : base(_ => Create(frame)) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected WebView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying WebKit.WKWebView
	/// </summary>
	public new WKWebView NativeView => (WKWebView)_nativeView.Value;

	partial void OnSourceChanged(string value) => NativeView.LoadRequest(new NSUrlRequest(new NSUrl(value)));

	/// <summary>
	/// Returns true if you can go back
	/// </summary>
	public bool CanGoBack => NativeView.CanGoBack;

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
	public bool CanGoForward => NativeView.CanGoForward;

	/// <summary>
	/// Navigates forward if possible
	/// </summary>
	public void Forward()
	{
		NativeView.GoForward();
		OnPropertyChanged(nameof(CanGoForward));
	}

	class SpiceNavigationDelegate : WKNavigationDelegate
	{

	}
}

