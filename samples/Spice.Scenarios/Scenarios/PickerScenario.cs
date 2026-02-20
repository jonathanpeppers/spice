using System.Collections.ObjectModel;

namespace Spice.Scenarios;

class PickerScenario : StackLayout
{
	public PickerScenario()
	{
		Title = "Picker";
		Spacing = 10;
		BackgroundColor = Colors.White;

		Add(new Label
		{
			Text = "Select your favorite color:",
			TextColor = Colors.Black
		});

		var picker = new Picker
		{
			Title = "Choose a color",
			Items = new ObservableCollection<string>
			{
				"Red",
				"Green",
				"Blue",
				"Yellow",
				"Orange",
				"Purple",
				"Pink",
				"Cyan"
			}
		};

		var resultLabel = new Label
		{
			Text = "No selection",
			TextColor = Colors.Gray
		};

		picker.PropertyChanged += (sender, e) =>
		{
			if (e.PropertyName == nameof(picker.SelectedIndex))
			{
				if (picker.SelectedItem != null)
				{
					resultLabel.Text = $"You selected: {picker.SelectedItem}";
					resultLabel.TextColor = picker.SelectedItem switch
					{
						"Red" => Colors.Red,
						"Green" => Colors.Green,
						"Blue" => Colors.Blue,
						"Yellow" => Color.FromRgb(200, 200, 0),
						"Orange" => Colors.Orange,
						"Purple" => Colors.Purple,
						"Pink" => Colors.Pink,
						"Cyan" => Colors.Cyan,
						_ => Colors.Black
					};
				}
				else
				{
					resultLabel.Text = "No selection";
					resultLabel.TextColor = Colors.Gray;
				}
			}
		};

		Add(picker);
		Add(resultLabel);
	}
}
