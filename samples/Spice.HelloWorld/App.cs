namespace Spice.HelloWorld;

public class App : Application
{
	public App()
	{
		int count = 0;

		var label = new Label
		{
			Text = "Hello, Spice! 🌶",
			VerticalAlign = Align.Center,
			HorizontalAlign = Align.Center,
		};

		var button = new Button
		{
			Text = "Click Me",
			VerticalAlign = Align.Center,
			HorizontalAlign = Align.Center,
			Clicked = _ => label.Text = $"Times: {++count}"
		};

		Main = new View {
			label,
			button,
		};
	}
}