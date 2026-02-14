namespace Spice.Scenarios;

class SliderScenario : StackLayout
{
	public SliderScenario()
	{
		Spacing = 10;

		var valueLabel = new Label { Text = "Value: 50" };
		var slider = new Slider
		{
			Minimum = 0,
			Maximum = 100,
			Value = 50
		};
		slider.ValueChanged = s =>
		{
			valueLabel.Text = $"Value: {s.Value:F0}";
		};

		Add(new Label { Text = "Slider Demo" });
		Add(slider);
		Add(valueLabel);

		// Second slider for volume style (0-10)
		var volumeLabel = new Label { Text = "Volume: 5" };
		var volumeSlider = new Slider
		{
			Minimum = 0,
			Maximum = 10,
			Value = 5
		};
		volumeSlider.ValueChanged = s =>
		{
			volumeLabel.Text = $"Volume: {s.Value:F0}";
		};

		Add(new Label { Text = "Volume Control" });
		Add(volumeSlider);
		Add(volumeLabel);

		// Third slider for percentage (0-100)
		var percentLabel = new Label { Text = "Percent: 50%" };
		var percentSlider = new Slider
		{
			Minimum = 0,
			Maximum = 100,
			Value = 50
		};
		percentSlider.ValueChanged = s =>
		{
			percentLabel.Text = $"Percent: {s.Value:F0}%";
		};

		Add(new Label { Text = "Percentage" });
		Add(percentSlider);
		Add(percentLabel);
	}
}
