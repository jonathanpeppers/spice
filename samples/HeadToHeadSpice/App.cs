namespace HeadToHeadSpice;

/// <summary>
/// NOTE: this app is for comparing head-to-head `dotnet new maui`
/// </summary>
public class App : Application
{
	public App()
	{
		int count = 0;

		Main = new StackLayout
		{
			new Image { Source = "dotnet_bot" },
			new Label { Text = "Hello, World!" },
			new Label { Text = "Welcome to Spice 🌶!" },
			new Button
			{
				Text = "Click me",
				Clicked = b =>
				{
					if (++count == 1)
						b.Text = $"Clicked {count} time";
					else
						b.Text = $"Clicked {count} times";
				}
			}
		};
	}
}