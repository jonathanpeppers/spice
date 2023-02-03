namespace Spice.Tests;

/// <summary>
/// Asserts the end-to-end idea works
/// </summary>
public class ApplicationTests
{
	class App : Application
	{
		public App()
		{
			int count = 0;

			var label = new Label
			{
				Text = "Hello, Spice 🌶",
			};

			var button = new Button
			{
				Text = "Click Me",
				Clicked = _ => label.Text = $"Times: {++count}"
			};

			Main = new StackView { label, button };
		}
	}

	[Fact]
	public void Application()
	{
		var app = new App();
		Assert.NotNull(app.Main);
		Assert.Equal(2, app.Main.Children.Count);

		var label = app.Main.Children[0] as Label;
		Assert.NotNull(label);
		var button = app.Main.Children[1] as Button;
		Assert.NotNull(button);
		Assert.NotNull(button.Clicked);

		button.Clicked(button);
		Assert.Equal("Times: 1", label.Text);

		button.Clicked(button);
		Assert.Equal("Times: 2", label.Text);
	}
}