namespace Spice.Scenarios;

class EditorScenario : StackLayout
{
	public EditorScenario()
	{
		Spacing = 10;

		Add(new Label { Text = "Editor Control" });
		Add(new Editor 
		{ 
			Text = "This is a multi-line text editor.\nYou can type multiple lines here.",
			HeightRequest = 150
		});
		
		Add(new Label { Text = "With Placeholder" });
		Add(new Editor 
		{ 
			Placeholder = "Enter your comments here...",
			HeightRequest = 100
		});
		
		Add(new Label { Text = "Styled Editor" });
		Add(new Editor 
		{ 
			Text = "Colored text",
			TextColor = Colors.Blue,
			Placeholder = "Type something...",
			PlaceholderColor = Colors.Gray,
			HeightRequest = 100
		});
	}
}
