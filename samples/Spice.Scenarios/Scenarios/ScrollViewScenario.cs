namespace Spice.Scenarios;

/// <summary>
/// Scenario demonstrating ScrollView with a long list of items
/// </summary>
class ScrollViewScenario : ScrollView
{
	public ScrollViewScenario()
	{
		var stackView = new StackView { Spacing = 10 };

		// Add a title
		stackView.Add(new Label
		{
			Text = "ScrollView Demo 📜",
			TextColor = Colors.Blue
		});

		// Add many items to demonstrate scrolling
		for (int i = 1; i <= 30; i++)
		{
			stackView.Add(new Label
			{
				Text = $"Item {i}"
			});
		}

		Add(stackView);
	}
}
