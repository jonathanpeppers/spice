namespace Spice.Scenarios;

class SliderScenario : StackView
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

		// Third slider for percentage (0-1)
		var percentLabel = new Label { Text = "Percent: 50%" };
		var percentSlider = new Slider
		{
			Minimum = 0,
			Maximum = 1,
			Value = 0.5
		};
		percentSlider.ValueChanged = s =>
		{
			percentLabel.Text = $"Percent: {s.Value * 100:F0}%";
		};

		Add(new Label { Text = "Percentage" });
		Add(percentSlider);
		Add(percentLabel);
	}
}
