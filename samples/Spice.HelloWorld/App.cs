namespace Spice.HelloWorld;

public class App : Application
{
	public App()
	{
		int count = 0;

		var label = new Label
		{
			Text = "Hello, Spice! 🌶",
			HorizontalAlign = Align.Center,
			VerticalAlign = Align.Center,
		};

		var button = new Button
		{
			Text = "Click Me",
			HorizontalAlign = Align.Center,
			VerticalAlign = Align.Center,
			Clicked = _ => label.Text = $"Times: {++count}"
		};

		Main = new StackView {
			label,
			button,
		};
	}
}