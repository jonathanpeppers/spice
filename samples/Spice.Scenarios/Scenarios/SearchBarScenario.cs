namespace Spice.Scenarios;

class SearchBarScenario : StackLayout
{
	public SearchBarScenario()
	{
		Spacing = 5;

		var label = new Label { Text = "Search for something..." };

		Add(new SearchBar
		{
			Placeholder = "Search",
			SearchButtonPressed = s => label.Text = $"Searched: {s.Text}",
		});
		Add(label);
	}
}
