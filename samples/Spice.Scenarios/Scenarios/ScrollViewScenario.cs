namespace Spice.Scenarios;

/// <summary>
/// Scenario demonstrating ScrollView with a long list of items
/// </summary>
class ScrollViewScenario : ScrollView
{
	public ScrollViewScenario()
	{
		Title = "Scroll View";
		var stackLayout = new StackLayout { Spacing = 10 };

		// Add a title
		stackLayout.Add(new Label
		{
			Text = "ScrollView Demo 📜",
			TextColor = Colors.Blue
		});

		// Add many items to demonstrate scrolling
		for (int i = 1; i <= 30; i++)
		{
			stackLayout.Add(new Label
			{
				Text = $"Item {i}"
			});
		}

		Add(stackLayout);
	}
}
