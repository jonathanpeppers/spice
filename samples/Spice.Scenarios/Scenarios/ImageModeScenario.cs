namespace Spice.Scenarios;

class ImageModeScenario : StackView
{
	public ImageModeScenario()
	{
		Add(new Label { Text = "50 x 50" });
		Add(new Image { Source = "spice" });


	}
}

