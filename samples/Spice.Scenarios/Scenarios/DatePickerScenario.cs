namespace Spice.Scenarios;

class DatePickerScenario : StackLayout
{
	public DatePickerScenario()
	{
		Title = "Date Picker";
		Spacing = 10;

		Add(new Label { Text = "Select a date:" });
		
		var datePicker = new DatePicker
		{
			Date = DateTime.Today,
			MinimumDate = new DateTime(2020, 1, 1),
			MaximumDate = new DateTime(2030, 12, 31)
		};
		Add(datePicker);

		var selectedLabel = new Label { Text = $"Selected: {datePicker.Date:D}" };
		Add(selectedLabel);

		datePicker.PropertyChanged += (sender, e) =>
		{
			if (e.PropertyName == nameof(datePicker.Date))
			{
				selectedLabel.Text = $"Selected: {datePicker.Date:D}";
			}
		};

		Add(new Label { Text = "\nDatePicker with custom date:" });
		
		// TextColor is supported on Android only; iOS compact style uses system theme colors
		var customDatePicker = new DatePicker
		{
			Date = new DateTime(2025, 12, 25),
			TextColor = Colors.Blue
		};
		Add(customDatePicker);

		var customLabel = new Label { Text = $"Christmas 2025: {customDatePicker.Date:D}" };
		Add(customLabel);

		customDatePicker.PropertyChanged += (sender, e) =>
		{
			if (e.PropertyName == nameof(customDatePicker.Date))
			{
				customLabel.Text = $"Selected: {customDatePicker.Date:D}";
			}
		};
	}
}
