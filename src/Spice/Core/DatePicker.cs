namespace Spice;

/// <summary>
/// Control for date selection.
/// Android -> Android.App.DatePickerDialog
/// iOS -> UIKit.UIDatePicker
/// </summary>
public partial class DatePicker : View
{
	/// <summary>
	/// The selected date
	/// </summary>
	[ObservableProperty]
	DateTime _date = DateTime.Today;

	/// <summary>
	/// The minimum selectable date
	/// </summary>
	[ObservableProperty]
	DateTime? _minimumDate;

	/// <summary>
	/// The maximum selectable date
	/// </summary>
	[ObservableProperty]
	DateTime? _maximumDate;

	/// <summary>
	/// Color of the text
	/// </summary>
	[ObservableProperty]
	Color? _textColor;

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		if (CanApplyTheme((int)ThemeProperty.TextColor))
			TextColor = theme.TextColor;
	}

	partial void OnTextColorChanging(Color? value) => TrackExplicit((int)ThemeProperty.TextColor, value);
}
