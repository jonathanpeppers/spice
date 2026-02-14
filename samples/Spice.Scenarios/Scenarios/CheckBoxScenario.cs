namespace Spice.Scenarios;

/// <summary>
/// Example showing CheckBox usage
/// </summary>
class CheckBoxScenario : StackView
{
	public CheckBoxScenario()
	{
		var label = new Label
		{
			Text = "No items selected",
		};

		var checkBox1 = new CheckBox
		{
			CheckedChanged = _ => UpdateLabel()
		};

		var label1 = new Label
		{
			Text = "Option 1",
		};

		var checkBox2 = new CheckBox
		{
			CheckedChanged = _ => UpdateLabel()
		};

		var label2 = new Label
		{
			Text = "Option 2",
		};

		var checkBox3 = new CheckBox
		{
			IsChecked = true,
			CheckedChanged = _ => UpdateLabel()
		};

		var label3 = new Label
		{
			Text = "Option 3 (default checked)",
		};

		Add(new Image { Source = "spice" });
		Add(label);
		Add(checkBox1);
		Add(label1);
		Add(checkBox2);
		Add(label2);
		Add(checkBox3);
		Add(label3);

		UpdateLabel();

		void UpdateLabel()
		{
			var count = 0;
			if (checkBox1.IsChecked) count++;
			if (checkBox2.IsChecked) count++;
			if (checkBox3.IsChecked) count++;

			label.Text = count switch
			{
				0 => "No items selected",
				1 => "1 item selected",
				_ => $"{count} items selected"
			};
		}
	}
}
