using System.Runtime.Versioning;

using Android.Content;

using Microsoft.AspNetCore.Components.WebView.Maui;

namespace Spice;

[SupportedOSPlatform("android23.0")]
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
		Initialize();
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public BlazorWebView(Context context) : this(context, Create)
	{
		Initialize();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected BlazorWebView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected BlazorWebView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	static AndroidAssetFileProvider? _fileProvider;
	AndroidWebKitWebViewManager? _webViewManager;

	partial void LoadNativeWebView(string contentRootDir, string hostPageRelativePath)
	{
		if (_fileProvider is null)
		{
			_fileProvider = new AndroidAssetFileProvider(NativeView.Context!.Assets, contentRootDir);
		}
		_webViewManager = new AndroidWebKitWebViewManager(NativeView, SpiceServiceProvider.Instance, SpiceDispatcher.Instance, _fileProvider, _jSComponents, contentRootDir, hostPageRelativePath);

		foreach (var component in RootComponents)
		{
			// Since the page isn't loaded yet, this will always complete synchronously
			_ = component.AddToWebViewManagerAsync(_webViewManager);
		}

		_webViewManager.Navigate(StartPath);
	}
}