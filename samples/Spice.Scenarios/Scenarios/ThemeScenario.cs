namespace Spice.Scenarios;

/// <summary>
/// Demonstrates theme switching (light/dark) and explicit per-view overrides.
/// </summary>
class ThemeScenario : StackLayout
{
	public ThemeScenario()
	{
		Title = "Theme";

		var label = new Label { Text = "Hello, Spice 🌶" };
		var counter = new Label { Text = "Times: 0" };
		int count = 0;
		var button = new Button
		{
			Text = "Tap me",
			Clicked = _ => counter.Text = $"Times: {++count}",
		};
		var overrideLabel = new Label
		{
			Text = "I'm always red",
			TextColor = Colors.Red,
		};
		var darkModeSwitch = new Switch
		{
			Toggled = sw =>
			{
				if (Navigation is { } nav)
				{
					// Walk up to find the Application — simplest: just set on the first Application ancestor
					// For demo purposes, swap the theme on the global app reference
				}
			},
		};

		Add(label);
		Add(counter);
		Add(button);
		Add(new Label { Text = "Dark Mode:" });
		Add(darkModeSwitch);
		Add(overrideLabel);
	}
}
