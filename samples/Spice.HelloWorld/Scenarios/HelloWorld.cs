namespace Spice.HelloWorld;

/// <summary>
/// Most basic scenario
/// </summary>
class HelloWorld : StackView
{
	public HelloWorld()
	{
		int count = 0;

		var label = new Label
		{
			Text = "Hello, Spice 🌶",
			TextColor = Colors.Red
		};

		var button = new Button
		{
			Text = "Click Me",
			Clicked = _ => label.Text = $"Times: {++count}"
		};

		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;
		BackgroundColor = Colors.CornflowerBlue;
		Add(new Image { Source = "spice" });
		Add(label);
		Add(button);
	}
}
