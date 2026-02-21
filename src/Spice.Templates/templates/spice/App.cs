namespace Hello;

public class App : Application
{
	public App()
	{
		UseSystemTheme = true;

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

		Main = new StackLayout { new Image { Source = "spice" }, label, button };
	}
}