namespace Spice;

/// <summary>
/// Represents a control that allows the user to select a time value.
/// Android -> Android.App.TimePickerDialog
/// iOS -> UIKit.UIDatePicker (Mode = Time)
/// </summary>
public partial class TimePicker : View
{
	/// <summary>
	/// The selected time value. Defaults to the current time.
	/// </summary>
	[ObservableProperty]
	TimeOnly _time = TimeOnly.FromDateTime(DateTime.Now);
}
