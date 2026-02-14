namespace Spice;

/// <summary>
/// Represents a border around a single child view. Set Content to a View, and customize with Stroke, StrokeThickness, CornerRadius, and Padding.
/// Android -> Custom drawable with GradientDrawable
/// iOS -> UIView with CALayer border
/// </summary>
public partial class Border : View
{
	/// <summary>
	/// The single child view to display inside the border
	/// </summary>
	[ObservableProperty]
	View? _content;

	/// <summary>
	/// The color of the border stroke
	/// </summary>
	[ObservableProperty]
	Color? _stroke;

	/// <summary>
	/// The thickness of the border stroke in pixels
	/// </summary>
	[ObservableProperty]
	double _strokeThickness = 1.0;

	/// <summary>
	/// The corner radius of the border in pixels
	/// </summary>
	[ObservableProperty]
	double _cornerRadius;

	/// <summary>
	/// The padding between the border and its content (uniform padding on all sides)
	/// </summary>
	[ObservableProperty]
	double _padding;
}
