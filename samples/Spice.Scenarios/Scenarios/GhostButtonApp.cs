namespace Spice.Scenarios;

/// <summary>
/// See GhostButton for an example of platform-specific logic
/// </summary>
class GhostButtonApp : StackView
{
	public GhostButtonApp()
	{
		int count = 0;

		var label = new Label
		{
			Text = "Pretty spooky! 👻",
		};

		var button = new GhostButton
		{
			Text = "Ghost button",
			Clicked = _ => label.Text = $"Times: {++count}"
		};

		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
		BackgroundColor = Colors.LimeGreen;
		Add(new Image { Source = "spice" });
		Add(label);
		Add(button);
		Add(new Button { Text = "Regular Button" });
	}

	class GhostButton : Button
	{
		public GhostButton() => NativeView.Alpha = 0.5f;
	}
}
