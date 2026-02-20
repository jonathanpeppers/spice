namespace Spice.Scenarios;

/// <summary>
/// Example showing RadioButton usage
/// </summary>
class RadioButtonScenario : StackLayout
{
	public RadioButtonScenario()
	{
		Title = "Radio Button";
		var label = new Label
		{
			Text = "No option selected",
		};

		var radioButton1 = new RadioButton
		{
			Content = "Option 1",
			GroupName = "Options"
		};

		var radioButton2 = new RadioButton
		{
			Content = "Option 2",
			GroupName = "Options"
		};

		var radioButton3 = new RadioButton
		{
			Content = "Option 3 (default checked)",
			GroupName = "Options",
			IsChecked = true
		};

		radioButton1.CheckedChanged = rb => UpdateLabel(rb, "Option 1");
		radioButton2.CheckedChanged = rb => UpdateLabel(rb, "Option 2");
		radioButton3.CheckedChanged = rb => UpdateLabel(rb, "Option 3");

		Add(new Image { Source = "spice" });
		Add(label);
		Add(radioButton1);
		Add(radioButton2);
		Add(radioButton3);

		UpdateLabel(radioButton3, "Option 3");

		void UpdateLabel(RadioButton rb, string optionName)
		{
			if (rb.IsChecked)
			{
				label.Text = $"{optionName} is selected";
			}
			else
			{
				label.Text = $"{optionName} was unchecked";
			}
		}
	}
}
