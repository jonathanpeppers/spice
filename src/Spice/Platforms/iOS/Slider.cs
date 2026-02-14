namespace Spice;

public partial class Slider
{
	/// <summary>
	/// Returns slider.NativeView
	/// </summary>
	/// <param name="slider">The Spice.Slider</param>
	public static implicit operator UISlider(Slider slider) => slider.NativeView;

	/// <summary>
	/// A slider control for selecting a numeric value from a range.
	/// Android -> Android.Widget.SeekBar
	/// iOS -> UIKit.UISlider
	/// </summary>
	public Slider() : base(_ => new UISlider { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Slider(CGRect frame) : base(_ => new UISlider(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Slider(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UISlider
	/// </summary>
	public new UISlider NativeView => (UISlider)_nativeView.Value;

	partial void OnMinimumChanged(double value) => NativeView.MinimumValue = (float)value;

	partial void OnMaximumChanged(double value) => NativeView.MaximumValue = (float)value;

	partial void OnValueChanged(double value) => NativeView.Value = (float)value;

	EventHandler? _valueChangedEvent;

	partial void OnValueChangedChanged(Action<Slider>? value)
	{
		if (value == null)
		{
			if (_valueChangedEvent != null)
			{
				NativeView.ValueChanged -= _valueChangedEvent;
				_valueChangedEvent = null;
			}
		}
		else
		{
			NativeView.ValueChanged += _valueChangedEvent = (sender, e) =>
			{
				Value = NativeView.Value;
				ValueChanged?.Invoke(this);
			};
		}
	}
}
