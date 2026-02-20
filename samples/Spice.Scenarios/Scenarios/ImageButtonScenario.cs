namespace Spice.Scenarios;

/// <summary>
/// ImageButton scenario demonstrating clickable image buttons
/// </summary>
class ImageButtonScenario : StackLayout
{
	public ImageButtonScenario()
	{
		Title = "Image Button";
		int count = 0;

		var label = new Label
		{
			Text = "Click the image button below!",
			TextColor = Colors.Blue
		};

		var imageButton = new ImageButton
		{
			Source = "spice",
			Clicked = _ => label.Text = $"Clicked {++count} time(s)!"
		};

		Add(label);
		Add(imageButton);

		// Add another image button with different source
		var secondLabel = new Label
		{
			Text = "Or click this one:",
			TextColor = Colors.Green
		};

		var secondImageButton = new ImageButton
		{
			Source = "dotnet_bot",
			Clicked = _ => secondLabel.Text = "You clicked the bot!"
		};

		Add(secondLabel);
		Add(secondImageButton);
	}
}
