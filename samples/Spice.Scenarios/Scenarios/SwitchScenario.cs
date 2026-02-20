namespace Spice.Scenarios;

/// <summary>
/// Demonstrates Switch control usage
/// </summary>
class SwitchScenario : StackLayout
{
	public SwitchScenario()
	{
		Title = "Switch";
		var label = new Label
		{
			Text = "Switch is OFF",
			TextColor = Colors.Red
		};

		var switchControl = new Switch
		{
			IsOn = false,
			Toggled = s =>
			{
				label.Text = s.IsOn ? "Switch is ON" : "Switch is OFF";
				label.TextColor = s.IsOn ? Colors.Green : Colors.Red;
			}
		};

		var switchControl2 = new Switch
		{
			IsOn = true
		};

		var label2 = new Label
		{
			Text = "Second switch (starts ON)"
		};

		Add(new Label { Text = "Toggle Switch Demo 🌶" });
		Add(switchControl);
		Add(label);
		Add(label2);
		Add(switchControl2);
		Add(new Button
		{
			Text = "Toggle First Switch Programmatically",
			Clicked = _ => switchControl.IsOn = !switchControl.IsOn
		});
	}
}
