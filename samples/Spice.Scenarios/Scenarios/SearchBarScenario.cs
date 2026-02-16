namespace Spice.Scenarios;

class SearchBarScenario : StackLayout
{
	public SearchBarScenario()
	{
		Title = "Search Bar";
		Spacing = 5;

		var searchLabel = new Label { Text = "Search for something..." };
		var textChangedLabel = new Label { Text = "Type to see text changes..." };

		Add(new SearchBar
		{
			Placeholder = "Search",
			PlaceholderColor = Colors.Gray,
			TextColor = Colors.Blue,
			SearchButtonPressed = s => searchLabel.Text = $"Searched: {s.Text}",
			TextChanged = s => textChangedLabel.Text = $"Current: {s.Text}",
		});
		Add(searchLabel);
		Add(textChangedLabel);
	}
}
