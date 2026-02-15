using System.Collections;
using System.Collections.ObjectModel;

namespace Spice;

/// <summary>
/// The base "View" type for all Spice views.
/// </summary>
public partial class View : ObservableObject, IEnumerable<View>
{
	/// <summary>
	/// Child views of this view
	/// </summary>
	public ObservableCollection<View> Children { get; set; } = new();

	/// <summary>
	/// Calls Children.Add(), collection initializer support
	/// </summary>
	public void Add(View item) => Children.Add(item);

	/// <summary>
	/// You can just foreach over this
	/// </summary>
	public IEnumerator<View> GetEnumerator() => Children.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

	/// <summary>
	/// The view's horizontal layout options.
	/// Platform implementations: UIKit.UIView.Frame / Android.Widget.RelativeLayout alignment rules
	/// </summary>
	[ObservableProperty]
	LayoutOptions _horizontalOptions;

	/// <summary>
	/// The view's vertical layout options.
	/// Platform implementations: UIKit.UIView.Frame / Android.Widget.RelativeLayout alignment rules
	/// </summary>
	[ObservableProperty]
	LayoutOptions _verticalOptions;

	/// <summary>
	/// Background color of the view
	/// </summary>
	[ObservableProperty]
	Color? _backgroundColor;

	/// <summary>
	/// Whether the view is visible. Defaults to true.
	/// </summary>
	[ObservableProperty]
	bool _isVisible = true;

	/// <summary>
	/// Whether the view is enabled (can respond to user interaction). Defaults to true.
	/// </summary>
	[ObservableProperty]
	bool _isEnabled = true;

	double _opacity = 1.0;

	/// <summary>
	/// Gets or sets the opacity of the view, ranging from 0.0 (fully transparent) to 1.0 (fully opaque). Defaults to 1.0.
	/// Values outside this range will be clamped.
	/// Platform implementations: UIKit.UIView.Alpha / Android.Views.View.Alpha
	/// </summary>
	public double Opacity
	{
		get => _opacity;
		set
		{
			var clampedValue = Math.Clamp(value, 0.0, 1.0);
			if (SetProperty(ref _opacity, clampedValue))
			{
				OnOpacityChanged(clampedValue);
			}
		}
	}

	/// <summary>
	/// Gets or sets the automation identifier for UI testing.
	/// Platform implementations: UIKit.UIView.AccessibilityIdentifier / Android.Views.View.ContentDescription
	/// </summary>
	[ObservableProperty]
	string? _automationId;

	/// <summary>
	/// Gets or sets the title of the view. Used by NavigationView for the navigation bar title and by Tab for the tab label.
	/// Ignored when the view is not inside a NavigationView or Tab.
	/// </summary>
	[ObservableProperty]
	string _title = "";

	/// <summary>
	/// Gets the NavigationView that contains this view, if any. Set automatically when the view is pushed onto a NavigationView.
	/// </summary>
	public NavigationView? Navigation { get; internal set; }

	/// <summary>
	/// Presents a view modally over the current content.
	/// </summary>
	/// <param name="view">The view to present.</param>
	/// <returns>A task that completes when the view has been presented.</returns>
	public Task PresentAsync(View view)
	{
		ArgumentNullException.ThrowIfNull(view);
		return PresentAsyncCore(view);
	}

	/// <summary>
	/// Presents a view created by a factory function modally over the current content.
	/// </summary>
	/// <param name="factory">A function that creates the view to present.</param>
	/// <returns>A task that completes when the view has been presented.</returns>
	public Task PresentAsync(Func<View> factory)
	{
		ArgumentNullException.ThrowIfNull(factory);
		var view = factory();
		return PresentAsync(view);
	}

	/// <summary>
	/// Presents a view of type T modally over the current content.
	/// The view is created using its parameterless constructor.
	/// </summary>
	/// <typeparam name="T">The type of view to present. Must have a parameterless constructor.</typeparam>
	/// <returns>A task that completes when the view has been presented.</returns>
	public Task PresentAsync<T>() where T : View, new()
	{
		return PresentAsync(new T());
	}

	/// <summary>
	/// Dismisses the currently presented modal view.
	/// </summary>
	/// <returns>A task that completes when the view has been dismissed.</returns>
	public Task DismissAsync()
	{
		return DismissAsyncCore();
	}

	/// <summary>
	/// Platform-specific implementation of PresentAsync.
	/// </summary>
#if VANILLA
	private Task PresentAsyncCore(View view) => Task.CompletedTask;
#else
	private partial Task PresentAsyncCore(View view);
#endif

	/// <summary>
	/// Platform-specific implementation of DismissAsync.
	/// </summary>
#if VANILLA
	private Task DismissAsyncCore() => Task.CompletedTask;
#else
	private partial Task DismissAsyncCore();
#endif

	/// <summary>
	/// Called when the Opacity property changes
	/// </summary>
	partial void OnOpacityChanged(double value);

	/// <summary>
	/// Space around the view. Supports uniform (10), horizontal/vertical (10,20), or individual sides (10,20,30,40).
	/// Aligns with Microsoft.Maui Margin property.
	/// </summary>
	[ObservableProperty]
	Thickness _margin;

	/// <summary>
	/// Gets or sets the desired width of the view. 
	/// Platform implementations: UIKit.UIView.Frame.Size / Android.Views.ViewGroup.LayoutParams.Width
	/// </summary>
	[ObservableProperty]
	double _widthRequest = -1;

	/// <summary>
	/// Gets or sets the desired height of the view.
	/// Platform implementations: UIKit.UIView.Frame.Size / Android.Views.ViewGroup.LayoutParams.Height
	/// </summary>
	[ObservableProperty]
	double _heightRequest = -1;

	/// <summary>
	/// Gets the actual rendered width of the view.
	/// Platform implementations: UIKit.UIView.Frame.Width / Android.Views.View.Width
	/// </summary>
#if VANILLA
	public double Width => 0;
#else
	public double Width => GetWidth();
#endif

	/// <summary>
	/// Gets the actual rendered height of the view.
	/// Platform implementations: UIKit.UIView.Frame.Height / Android.Views.View.Height
	/// </summary>
#if VANILLA
	public double Height => 0;
#else
	public double Height => GetHeight();
#endif

#if !VANILLA
	/// <summary>
	/// Platform-specific implementation to get actual width
	/// </summary>
	private partial double GetWidth();

	/// <summary>
	/// Platform-specific implementation to get actual height
	/// </summary>
	private partial double GetHeight();

#endif
}