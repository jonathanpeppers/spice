using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Components.Web;

namespace Spice;

/// <summary>
/// A "web view" for rendering Blazor/.razor content on each platform.
/// Android -> Android.Webkit.WebView
/// iOS -> WebKit.WKWebView
/// </summary>
public partial class BlazorWebView : WebView
{
	internal const string AppHostAddress = "0.0.0.0";

	readonly JSComponentConfigurationStore _jSComponents = new();

	/// <summary>
	/// Gets or sets the path to the HTML file to render.
	/// <para>This is an app relative path to the file such as <c>wwwroot\index.html</c></para>
	/// </summary>
	[ObservableProperty]
	string _hostPage = "";

	/// <summary>
	/// Gets or sets the path for initial navigation within the Blazor navigation context when the Blazor component is finished loading.
	/// </summary>
	[ObservableProperty]
	string? _startPage;

	/// <summary>
	/// Gets a collection of <see cref="RootComponent"/> items.
	/// </summary>
	[ObservableProperty]
	ObservableCollection<RootComponent> _rootComponents = new();

	/// <summary>
	/// Gets or sets the path for initial navigation within the Blazor navigation context when the Blazor component is finished loading.
	/// </summary>
	[ObservableProperty]
	string _startPath = "/";

	/// <summary>
	/// Called by the ctor
	/// </summary>
	void Initialize()
	{
		if (_rootComponents is not null)
			_rootComponents.CollectionChanged += OnCollectionChanged;
	}

	/// <summary>
	/// Called when HostPage or RootComponents change
	/// </summary>
	void LoadWebView()
	{
		if (string.IsNullOrEmpty(_hostPage) || _rootComponents?.Count == 0)
			return;

		// We assume the host page is always in the root of the content directory, because it's
		// unclear there's any other use case. We can add more options later if so.
		var contentRootDir = Path.GetDirectoryName(HostPage!) ?? string.Empty;
		var hostPageRelativePath = Path.GetRelativePath(contentRootDir, HostPage!);

		LoadNativeWebView(contentRootDir, hostPageRelativePath);
	}

	/// <summary>
	/// Implemented in each platform
	/// </summary>
	partial void LoadNativeWebView(string contentRootDir, string hostPageRelativePath);

	void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadWebView();

	partial void OnHostPageChanged(string value) => LoadWebView();

	partial void OnRootComponentsChanging(ObservableCollection<RootComponent> value)
	{
		if (_rootComponents is not null)
			_rootComponents.CollectionChanged -= OnCollectionChanged;
	}

	partial void OnRootComponentsChanged(ObservableCollection<RootComponent> value)
	{
		if (value is not null)
			value.CollectionChanged += OnCollectionChanged;

		LoadWebView();
	}
}