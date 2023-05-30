using Android.Content;

namespace Spice;

public partial class BlazorWebView
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view">The Spice.BlazorWebView</param>
	public static implicit operator Android.Webkit.WebView(BlazorWebView view) => view.NativeView;

	/// <summary>
	/// A "web view" for rendering Blazor/.razor content on each platform.
	/// Android -> Android.Webkit.WebView
	/// iOS -> WebKit.WKWebView
	/// </summary>
	public BlazorWebView() : this(Platform.Context, Create)
	{
		// Most users would want this default instead of center
		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public BlazorWebView(Context context) : this(context, Create)
	{
		// Most users would want this default instead of center
		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected BlazorWebView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected BlazorWebView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }
}