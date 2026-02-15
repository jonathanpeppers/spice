namespace Spice;

/// <summary>
/// A scrollable container view that can hold a single child view.
/// Android -> Android.Widget.ScrollView (vertical) / Android.Widget.HorizontalScrollView (horizontal)
/// iOS -> UIKit.UIScrollView
/// </summary>
public partial class ScrollView : View
{
	/// <summary>
	/// The orientation of the scroll view. Defaults to Vertical.
	/// </summary>
	[ObservableProperty]
	Orientation _orientation;

	/// <summary>
	/// Gets or sets the padding inside the ScrollView in device-independent units (uniform padding on all sides).
	/// Platform implementations: Android.Views.ViewGroup.SetPadding / UIKit.UIScrollView.ContentInset
	/// </summary>
	[ObservableProperty]
	double _padding;
}
