namespace Spice.Scenarios;

/// <summary>
/// Demonstrates the Border control with various configurations
/// </summary>
class BorderScenario : StackLayout
{
	public BorderScenario()
	{
		// Example 1: Simple border with text
		var simpleBorder = new Border
		{
			Stroke = Colors.Blue,
			StrokeThickness = 2.0,
			CornerRadius = 8.0,
			Padding = 10.0,
			Content = new Label
			{
				Text = "Simple Border",
				TextColor = Colors.White
			},
			HorizontalOptions = LayoutOptions.Fill
		};

		// Example 2: Border with background color
		var coloredBorder = new Border
		{
			Stroke = Colors.Green,
			StrokeThickness = 3.0,
			CornerRadius = 15.0,
			Padding = 15.0,
			BackgroundColor = Colors.LightGreen,
			Content = new Label
			{
				Text = "Border with Background",
				TextColor = Colors.DarkGreen
			},
			HorizontalOptions = LayoutOptions.Fill
		};

		// Example 3: Rounded border (high corner radius)
		var roundedBorder = new Border
		{
			Stroke = Colors.Purple,
			StrokeThickness = 2.0,
			CornerRadius = 25.0,
			Padding = 12.0,
			Content = new Label
			{
				Text = "Rounded Border",
				TextColor = Colors.White
			},
			HorizontalOptions = LayoutOptions.Fill
		};

		// Example 4: Border with button inside
		var buttonBorder = new Border
		{
			Stroke = Colors.Orange,
			StrokeThickness = 2.0,
			CornerRadius = 10.0,
			Padding = 5.0,
			Content = new Button
			{
				Text = "Button in Border",
				Clicked = _ => System.Diagnostics.Debug.WriteLine("Button clicked!")
			},
			HorizontalOptions = LayoutOptions.Fill
		};

		// Example 5: Nested content with StackLayout
		var nestedBorder = new Border
		{
			Stroke = Colors.Red,
			StrokeThickness = 2.0,
			CornerRadius = 12.0,
			Padding = 10.0,
			BackgroundColor = Colors.LightCoral,
			Content = new StackLayout
			{
				new Label { Text = "Nested Content", TextColor = Colors.White },
				new Label { Text = "Multiple items inside", TextColor = Colors.White },
			},
			HorizontalOptions = LayoutOptions.Fill
		};

		// Title
		Add(new Label
		{
			Text = "Border Examples",
			TextColor = Colors.White
		});

		// Add all border examples
		Add(simpleBorder);
		Add(coloredBorder);
		Add(roundedBorder);
		Add(buttonBorder);
		Add(nestedBorder);
	}
}
