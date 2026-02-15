namespace Spice;

/// <summary>
/// A parent view for laying out child controls in a row. Defaults to Orientation=Vertical.
/// Android -> Android.Widget.LinearLayout
/// iOS -> UIKit.UIStackView
/// </summary>
public partial class StackLayout : View
{
	/// <summary>
	/// Set to specify if layout is vertical or horizontal. Defaults to vertical.
	/// </summary>
	[ObservableProperty]
	Orientation _orientation;

	/// <summary>
	/// Spacing between children
	/// </summary>
	[ObservableProperty]
	double _spacing = 0;

	/// <summary>
	/// Gets or sets the padding inside the StackLayout in device-independent units (uniform padding on all sides).
	/// Platform implementations: Android.Views.ViewGroup.SetPadding / UIKit.UIStackView.LayoutMargins
	/// </summary>
	[ObservableProperty]
	double _padding;
}