namespace Spice.Scenarios;

/// <summary>
/// Example showing CheckBox usage
/// </summary>
class CheckBoxScenario : StackLayout
{
	public CheckBoxScenario()
	{
		var label = new Label
		{
			Text = "No items selected",
		};

		var checkBox1 = new CheckBox();

		var label1 = new Label
		{
			Text = "Option 1",
		};

		var checkBox2 = new CheckBox();

		var label2 = new Label
		{
			Text = "Option 2",
		};

		var checkBox3 = new CheckBox
		{
			IsChecked = true,
		};

		var label3 = new Label
		{
			Text = "Option 3 (default checked)",
		};

		checkBox1.CheckedChanged = _ => UpdateLabel();
		checkBox2.CheckedChanged = _ => UpdateLabel();
		checkBox3.CheckedChanged = _ => UpdateLabel();

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
