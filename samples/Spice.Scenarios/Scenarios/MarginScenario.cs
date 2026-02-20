namespace Spice.Scenarios;

class MarginScenario : StackLayout
{
	public MarginScenario()
	{
		Title = "Margin";
		Spacing = 20;

		Add(new Label { Text = "Margin Demo", TextColor = Colors.White });

		// No margin
		Add(new Label
		{
			Text = "No Margin",
			BackgroundColor = Colors.Blue,
			TextColor = Colors.White
		});

		// Uniform margin
		Add(new Label
		{
			Text = "Uniform Margin = 20",
			BackgroundColor = Colors.Green,
			TextColor = Colors.White,
			Margin = 20
		});

		// Horizontal/Vertical margin
		Add(new Label
		{
			Text = "Horizontal 10, Vertical 30",
			BackgroundColor = Colors.Orange,
			TextColor = Colors.White,
			Margin = new Thickness(10, 30)
		});

		// Individual margins
		Add(new Label
		{
			Text = "Individual: L=0, T=10, R=40, B=20",
			BackgroundColor = Colors.Purple,
			TextColor = Colors.White,
			Margin = new Thickness(0, 10, 40, 20)
		});

		// Button with margin
		Add(new Button
		{
			Text = "Button with Margin = 15",
			Margin = 15
		});

		// BoxView with margin
		Add(new BoxView
		{
			Color = Colors.Red,
			Margin = new Thickness(30, 10, 30, 10)
		});

		Add(new Label
		{
			Text = "BoxView above has Margin = 30,10,30,10",
			TextColor = Colors.White
		});

		// Border with margin and padding
		Add(new Border
		{
			Stroke = Colors.White,
			StrokeThickness = 2,
			CornerRadius = 10,
			Padding = 10,
			Margin = 20,
			Content = new Label
			{
				Text = "Border with Padding=10, Margin=20",
				TextColor = Colors.White
			}
		});
	}
}
