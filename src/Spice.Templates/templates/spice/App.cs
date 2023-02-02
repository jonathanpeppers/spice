namespace Hello;

public class App : Application
{
	public App()
	{
		int count = 0;

		var label = new Label
		{
			Text = "Spicy! 🌶",
		};

		var button = new Button
		{
			Text = "Click Me",
			Clicked = _ => label.Text = $"Times: {++count}"
		};

		Main = new StackView { new Image { Source = "spice" }, label, button };
	}
}