namespace Spice.Scenarios;

public class ContentViewScenario : Application
{
	public ContentViewScenario()
	{
		BackgroundColor = Colors.White;

		// Create a simple card-like ContentView
		var card = new ContentView
		{
			BackgroundColor = Colors.LightGray,
			Content = new StackView
			{
				new Label
				{
					Text = "ContentView Example",
					TextColor = Colors.Black,
				},
				new Label
				{
					Text = "This is a card built with ContentView",
					TextColor = Colors.DarkGray,
				},
			}
		};

		// Create another ContentView with just a label
		var simpleContent = new ContentView
		{
			BackgroundColor = Colors.LightBlue,
			Content = new Label
			{
				Text = "Simple ContentView",
				TextColor = Colors.Navy,
			}
		};

		// Nested ContentView
		var nestedContent = new ContentView
		{
			BackgroundColor = Colors.LightGreen,
			Content = new ContentView
			{
				BackgroundColor = Colors.LightYellow,
				Content = new Label
				{
					Text = "Nested ContentView",
					TextColor = Colors.Black,
				}
			}
		};

		Main = new ScrollView
		{
			Children =
			{
				new StackView
				{
					card,
					simpleContent,
					nestedContent,
					new Button
					{
						Text = "Back",
						Clicked = _ => Main = new App(),
					}
				}
			}
		};
	}
}
