namespace Spice;

/// <summary>
/// A slider control for selecting a numeric value from a range.
/// Android -> Android.Widget.SeekBar
/// iOS -> UIKit.UISlider
/// </summary>
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
