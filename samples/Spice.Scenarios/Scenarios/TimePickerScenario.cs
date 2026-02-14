namespace Spice.Scenarios;

class TimePickerScenario : StackLayout
{
	public TimePickerScenario()
	{
		Spacing = 10;

		Add(new Label
		{
			Text = "Select a time:",
		});

		var timePicker = new TimePicker
		{
			Time = new TimeOnly(9, 30)
		};

		Add(timePicker);

		var resultLabel = new Label
		{
			Text = GetTimeDisplayText(timePicker.Time),
		};

		Add(resultLabel);

		// Update label when time changes
		timePicker.PropertyChanged += (sender, e) =>
		{
			if (e.PropertyName == nameof(timePicker.Time))
			{
				resultLabel.Text = GetTimeDisplayText(timePicker.Time);
			}
		};
	}

	static string GetTimeDisplayText(TimeOnly time) =>
		$"Selected: {time:hh\\:mm tt}";
}
