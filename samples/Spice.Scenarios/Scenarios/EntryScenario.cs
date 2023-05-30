namespace Spice.Scenarios;

class EntryScenario : StackView
{
	public EntryScenario()
	{
		Spacing = 5;

		Add(new Entry { Text = "jonathan.peppers@gmail.com" });
		Add(new Entry { IsPassword = true });
		Add(new Button { Text = "Login" });
	}
}
