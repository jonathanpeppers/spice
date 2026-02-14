namespace Spice;

/// <summary>
/// A slider control for selecting a numeric value from a range.
/// Android -> Android.Widget.SeekBar
/// iOS -> UIKit.UISlider
/// </summary>
/// <remarks>
/// Note: Android SeekBar only supports integer values. While the API accepts doubles,
/// values will be cast to integers on Android. For fractional ranges, use a larger
/// integer range (e.g., 0-100 instead of 0.0-1.0) and scale in your code.
/// </remarks>
public partial class Slider : View
{
	/// <summary>
	/// The minimum value of the slider range
	/// </summary>
	[ObservableProperty]
	double _minimum = 0;

	/// <summary>
	/// The maximum value of the slider range
	/// </summary>
	[ObservableProperty]
	double _maximum = 1;

	/// <summary>
	/// The current value of the slider
	/// </summary>
	[ObservableProperty]
	double _value = 0;

	/// <summary>
	/// Action invoked when the slider value changes
	/// </summary>
	[ObservableProperty]
	Action<Slider>? _valueChanged;
}
