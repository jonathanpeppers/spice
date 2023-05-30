namespace Spice;

/// <summary>
/// A "web view" for rendering HTML content on each platform.
/// </summary>
public partial class WebView : View
{
	/// <summary>
	/// The url to navigate to
	/// </summary>
	[ObservableProperty]
	string _source = "";

	/// <summary>
	/// If JavaScript is enabled on the WebView. Defaults to true.
	/// </summary>
	[ObservableProperty]
	bool _isJavaScriptEnabled = true;
}