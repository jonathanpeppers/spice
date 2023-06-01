using WebKit;

namespace Spice;

public partial class WebView
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view">The Spice.WebView</param>
	public static implicit operator WKWebView(WebView view) => view.NativeView;

	internal static WKWebViewConfiguration CreateConfiguration()
	{
		var config = new WKWebViewConfiguration();

		// By default, setting inline media playback to allowed, including autoplay
		// and picture in picture, since these things MUST be set during the webview
		// creation, and have no effect if set afterwards.
		// A custom handler factory delegate could be set to disable these defaults
		// but if we do not set them here, they cannot be changed once the
		// handler's platform view is created, so erring on the side of wanting this
		// capability by default.
		if (OperatingSystem.IsMacCatalystVersionAtLeast(10) || OperatingSystem.IsIOSVersionAtLeast(10))
		{
			config.AllowsPictureInPictureMediaPlayback = true;
			config.AllowsInlineMediaPlayback = true;
			config.MediaTypesRequiringUserActionForPlayback = WKAudiovisualMediaTypes.None;
		}

		return config;
	}

	internal static WKWebView Create(CGRect frame, Func<WKWebViewConfiguration> createConfiguration)
	{
		var view = new WKWebView(frame, createConfiguration())
		{
			AutoresizingMask = UIViewAutoresizing.None,
		};
		return view;
	}

	/// <summary>
	/// A "web view" for rendering HTML content on each platform.
	/// Android -> Android.Webkit.WebView
	/// iOS -> WebKit.WKWebView
	/// </summary>
	public WebView() : this(_ => Create(CGRect.Empty, CreateConfiguration)) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public WebView(CGRect frame) : this(_ => Create(frame, CreateConfiguration)) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected WebView(Func<View, UIView> creator) : base(creator) => Initialize();

	// Called by ctor
	void Initialize()
	{
		// Most users would want this default instead of center
		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
	}

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
}

