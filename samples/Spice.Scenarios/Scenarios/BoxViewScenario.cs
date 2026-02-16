namespace Spice.Scenarios;

class BoxViewScenario : StackLayout
{
	public BoxViewScenario()
	{
		Title = "Box View";
		Spacing = 20;

		var boxView = new BoxView
		{
			Color = Colors.Red
		};

		var blueButton = new Button
		{
			Text = "Blue Color",
			Clicked = _ => boxView.Color = Colors.Blue
		};

		var greenButton = new Button
		{
			Text = "Green Color",
			Clicked = _ => boxView.Color = Colors.Green
		};

		var redButton = new Button
		{
			Text = "Red Color",
			Clicked = _ => boxView.Color = Colors.Red
		};

		var yellowButton = new Button
		{
			Text = "Yellow Color",
			Clicked = _ => boxView.Color = Colors.Yellow
		};

		Add(new Label { Text = "BoxView Demo", TextColor = Colors.White });
		Add(boxView);
		Add(new Label { Text = "Change Color:", TextColor = Colors.White });
		Add(blueButton);
		Add(greenButton);
		Add(redButton);
		Add(yellowButton);
	}
}
