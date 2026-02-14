namespace Spice.Tests;

/// <summary>
/// Tests for DatePicker control
/// </summary>
public class DatePickerTests
{
	[Fact]
	public void DatePicker_DefaultDate_IsToday()
	{
		var datePicker = new DatePicker();
		Assert.Equal(DateTime.Today, datePicker.Date);
	}

	[Fact]
	public void DatePicker_SetDate_UpdatesProperty()
	{
		var datePicker = new DatePicker();
		var newDate = new DateTime(2024, 12, 25);
		datePicker.Date = newDate;
		Assert.Equal(newDate, datePicker.Date);
	}

	[Fact]
	public void DatePicker_SetMinimumDate_UpdatesProperty()
	{
		var datePicker = new DatePicker();
		var minDate = new DateTime(2020, 1, 1);
		datePicker.MinimumDate = minDate;
		Assert.Equal(minDate, datePicker.MinimumDate);
	}

	[Fact]
	public void DatePicker_SetMaximumDate_UpdatesProperty()
	{
		var datePicker = new DatePicker();
		var maxDate = new DateTime(2030, 12, 31);
		datePicker.MaximumDate = maxDate;
		Assert.Equal(maxDate, datePicker.MaximumDate);
	}

	[Fact]
	public void DatePicker_SetTextColor_UpdatesProperty()
	{
		var datePicker = new DatePicker();
		var color = Colors.Red;
		datePicker.TextColor = color;
		Assert.Equal(color, datePicker.TextColor);
	}

	[Fact]
	public void DatePicker_PropertyChanged_Fires()
	{
		string? propertyName = null;
		var datePicker = new DatePicker();
		datePicker.PropertyChanged += (sender, e) => propertyName = e.PropertyName;
		
		var newDate = new DateTime(2025, 6, 15);
		datePicker.Date = newDate;

		Assert.Equal(nameof(datePicker.Date), propertyName);
	}

	[Fact]
	public void DatePicker_MinMaxDates_CanBeNull()
	{
		var datePicker = new DatePicker
		{
			MinimumDate = new DateTime(2020, 1, 1),
			MaximumDate = new DateTime(2030, 12, 31)
		};

		Assert.NotNull(datePicker.MinimumDate);
		Assert.NotNull(datePicker.MaximumDate);

		datePicker.MinimumDate = null;
		datePicker.MaximumDate = null;

		Assert.Null(datePicker.MinimumDate);
		Assert.Null(datePicker.MaximumDate);
	}
}
