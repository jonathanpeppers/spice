using System.Collections.ObjectModel;

namespace Spice;

/// <summary>
/// A "web view" for rendering Blazor/.razor content on each platform.
/// Android -> Android.Webkit.WebView
/// iOS -> WebKit.WKWebView
/// </summary>
public partial class BlazorWebView : WebView
{
	internal const string AppHostAddress = "0.0.0.0";

	/// <summary>
	/// Gets or sets the path to the HTML file to render.
	/// <para>This is an app relative path to the file such as <c>wwwroot\index.html</c></para>
	/// </summary>
	[ObservableProperty]
	string? _hostPage;

	/// <summary>
	/// Gets or sets the path for initial navigation within the Blazor navigation context when the Blazor component is finished loading.
	/// </summary>
	[ObservableProperty]
	string? _startPage;

	[ObservableProperty]
	ObservableCollection<RootComponent> _rootComponents = new ObservableCollection<RootComponent>();
}