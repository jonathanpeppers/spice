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
}
