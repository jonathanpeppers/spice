namespace Spice;

/// <summary>
/// A "web view" for rendering Blazor/.razor content on each platform.
/// Android -> Android.Webkit.WebView
/// iOS -> WebKit.WKWebView
/// </summary>
public partial class BlazorWebView : View
{
	internal const string AppHostAddress = "0.0.0.0";
}