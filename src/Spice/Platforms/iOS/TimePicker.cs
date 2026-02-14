namespace Spice;

public partial class TimePicker
{
	/// <summary>
	/// Returns timePicker.NativeView
	/// </summary>
	/// <param name="timePicker">The Spice.TimePicker</param>
	public static implicit operator UIDatePicker(TimePicker timePicker) => timePicker.NativeView;

	/// <summary>
	/// Represents a control that allows the user to select a time value.
	/// Android -> Android.App.TimePickerDialog
	/// iOS -> UIKit.UIDatePicker (Mode = Time)
	/// </summary>
	public TimePicker() : base(_ => new UIDatePicker
	{
		AutoresizingMask = UIViewAutoresizing.None,
		Mode = UIDatePickerMode.Time,
		PreferredDatePickerStyle = UIDatePickerStyle.Wheels
	})
	{
		NativeView.ValueChanged += OnNativeValueChanged;
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public TimePicker(CGRect frame) : base(_ => new UIDatePicker(frame)
	{
		AutoresizingMask = UIViewAutoresizing.None,
		Mode = UIDatePickerMode.Time,
		PreferredDatePickerStyle = UIDatePickerStyle.Wheels
	})
	{
		NativeView.ValueChanged += OnNativeValueChanged;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected TimePicker(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIDatePicker
	/// </summary>
	public new UIDatePicker NativeView => (UIDatePicker)_nativeView.Value;

	partial void OnTimeChanged(TimeOnly value)
	{
		// Convert TimeOnly to NSDate
		var dateTime = DateTime.Today.Add(value.ToTimeSpan());
		NativeView.Date = (NSDate)dateTime;
	}

	void OnNativeValueChanged(object? sender, EventArgs e)
	{
		// Convert NSDate to TimeOnly
		var dateTime = (DateTime)NativeView.Date;
		Time = TimeOnly.FromTimeSpan(dateTime.TimeOfDay);
	}
}
