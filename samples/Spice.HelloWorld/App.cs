namespace Spice.HelloWorld;

public class App : Application
{
	public App()
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

		BackgroundColor = Colors.CornflowerBlue;
		Main = new StackView { new Image { Source = "spice" }, label, button };
	}
}