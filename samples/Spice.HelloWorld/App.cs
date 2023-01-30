namespace Spice.HelloWorld;

public class App : Application
{
	public App()
	{
		int count = 0;

		var label = new Label
		{
			Text = "Hello, Spice! 🌶",
		};

		var button = new Button
		{
			Text = "Click Me",
			Clicked = _ => label.Text = $"Times: {++count}"
		};

		Main = new StackView {
			HorizontalAlign = Align.Center,
			VerticalAlign = Align.Center,
			Children = {
				label,
				button,
			}
		};
	}
}