using Android.Content;
using Android.App;
using Android.Text.Format;

namespace Spice;

public partial class TimePicker
{
	/// <summary>
	/// Returns timePicker.NativeView
	/// </summary>
	/// <param name="timePicker">The Spice.TimePicker</param>
	public static implicit operator Button(TimePicker timePicker) => timePicker.NativeView;

	static Button Create(Context context) => new(context);

	/// <summary>
	/// Represents a control that allows the user to select a time value.
	/// Android -> Android.App.TimePickerDialog
	/// iOS -> UIKit.UIDatePicker (Mode = Time)
	/// </summary>
	public TimePicker() : base(Platform.Context, Create)
	{
		NativeView.Click += OnNativeClick;
		UpdateButtonText();
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public TimePicker(Context context) : base(context, Create)
	{
		NativeView.Click += OnNativeClick;
		UpdateButtonText();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected TimePicker(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected TimePicker(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.Button
	/// </summary>
	public new Button NativeView => (Button)_nativeView.Value;

	partial void OnTimeChanged(TimeOnly value)
	{
		UpdateButtonText();
	}

	void OnNativeClick(object? sender, EventArgs e)
	{
		if (NativeView.Context is not Context context)
			return;

		var is24HourView = DateFormat.Is24HourFormat(context);
		var dialog = new TimePickerDialog(
			context,
			OnTimeSet,
			Time.Hour,
			Time.Minute,
			is24HourView);

		dialog.Show();
	}

	void OnTimeSet(object? sender, TimePickerDialog.TimeSetEventArgs e)
	{
		Time = new TimeOnly(e.HourOfDay, e.Minute);
	}

	void UpdateButtonText()
	{
		var context = NativeView.Context;
		if (context == null)
			return;

		var is24HourView = DateFormat.Is24HourFormat(context);
		var format = is24HourView ? "HH:mm" : "h:mm tt";
		NativeView.Text = Time.ToString(format);
	}
}
