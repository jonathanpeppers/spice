using Android.Content;
using Android.Widget;

namespace Spice;

public partial class Slider
{
	/// <summary>
	/// Returns slider.NativeView
	/// </summary>
	/// <param name="slider">The Spice.Slider</param>
	public static implicit operator SeekBar(Slider slider) => slider.NativeView;

	static SeekBar Create(Context context) => new(context);

	/// <summary>
	/// A slider control for selecting a numeric value from a range.
	/// Android -> Android.Widget.SeekBar
	/// iOS -> UIKit.UISlider
	/// </summary>
	public Slider() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Slider(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Slider(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Slider(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.SeekBar
	/// </summary>
	public new SeekBar NativeView => (SeekBar)_nativeView.Value;

	partial void OnMinimumChanged(double value)
	{
		NativeView.Min = (int)value;
		// Re-apply value in case it's now out of range
		NativeView.Progress = (int)Value;
	}

	partial void OnMaximumChanged(double value)
	{
		NativeView.Max = (int)value;
		// Re-apply value in case it's now out of range
		NativeView.Progress = (int)Value;
	}

	partial void OnValueChanged(double value)
	{
		int intValue = (int)value;
		if (NativeView.Progress != intValue)
		{
			NativeView.Progress = intValue;
		}
	}

	SeekBarChangeListener? _listener;

	partial void OnValueChangedChanged(Action<Slider>? value)
	{
		if (value == null)
		{
			if (_listener != null)
			{
				NativeView.SetOnSeekBarChangeListener(null);
				_listener = null;
			}
		}
		else
		{
			_listener = new SeekBarChangeListener(this);
			NativeView.SetOnSeekBarChangeListener(_listener);
		}
	}

	class SeekBarChangeListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener
	{
		readonly Slider _slider;

		public SeekBarChangeListener(Slider slider)
		{
			_slider = slider;
		}

		public void OnProgressChanged(SeekBar? seekBar, int progress, bool fromUser)
		{
			_slider.Value = progress;
			_slider.ValueChanged?.Invoke(_slider);
		}

		public void OnStartTrackingTouch(SeekBar? seekBar) { }

		public void OnStopTrackingTouch(SeekBar? seekBar) { }
	}
}
