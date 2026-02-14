namespace Spice.Scenarios;

class ActivityIndicatorScenario : StackView
{
	public ActivityIndicatorScenario()
	{
		Spacing = 20;

		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			Color = Colors.Red
		};

		var button = new Button
		{
			Text = "Toggle Running",
			Clicked = _ => activityIndicator.IsRunning = !activityIndicator.IsRunning
		};

		var blueButton = new Button
		{
			Text = "Blue Color",
			Clicked = _ => activityIndicator.Color = Colors.Blue
		};

		var greenButton = new Button
		{
			Text = "Green Color",
			Clicked = _ => activityIndicator.Color = Colors.Green
		};

		var redButton = new Button
		{
			Text = "Red Color",
			Clicked = _ => activityIndicator.Color = Colors.Red
		};

		Add(new Label { Text = "ActivityIndicator Demo", TextColor = Colors.White });
		Add(activityIndicator);
		Add(button);
		Add(new Label { Text = "Change Color:", TextColor = Colors.White });
		Add(blueButton);
		Add(greenButton);
		Add(redButton);
	}
}
