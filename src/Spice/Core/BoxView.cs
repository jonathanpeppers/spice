namespace Spice;

/// <summary>
/// Represents a colored rectangle used for decoration, backgrounds, or dividing lines.
/// Android -> Android.Views.View
/// iOS -> UIKit.UIView
/// </summary>
public partial class BoxView : View
{
	/// <summary>
	/// Color of the box
	/// </summary>
	[ObservableProperty]
	Color? _color;
}
