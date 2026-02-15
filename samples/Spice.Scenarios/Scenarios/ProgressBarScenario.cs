namespace Spice.Scenarios;

/// <summary>
/// Demonstrates the ProgressBar control with various progress values
/// </summary>
class ProgressBarScenario : StackLayout
{
	const double ProgressIncrement = 0.1; // 10% increment per click
	ProgressBar _progressBar;
	Label _progressLabel;

	public ProgressBarScenario()
	{
		_progressLabel = new Label
		{
			Text = "Progress: 0%",
			TextColor = Colors.White
		};

		_progressBar = new ProgressBar
		{
			Progress = 0.0,
			HorizontalOptions = LayoutOptions.Fill
		};

		var incrementButton = new Button
		{
			Text = "Increment Progress (+10%)",
			Clicked = _ => IncrementProgress()
		};

		var resetButton = new Button
		{
			Text = "Reset Progress",
			Clicked = _ => SetProgress(0.0)
		};

		var setHalfButton = new Button
		{
			Text = "Set to 50%",
			Clicked = _ => SetProgress(0.5)
		};

		var setCompleteButton = new Button
		{
			Text = "Set to 100%",
			Clicked = _ => SetProgress(1.0)
		};

		Add(_progressLabel);
		Add(_progressBar);
		Add(incrementButton);
		Add(setHalfButton);
		Add(setCompleteButton);
		Add(resetButton);
	}

	void IncrementProgress()
	{
		// ProgressBar.Progress property automatically clamps to [0.0, 1.0]
		SetProgress(_progressBar.Progress + ProgressIncrement);
	}

	void SetProgress(double value)
	{
		_progressBar.Progress = value;
		// Read back the clamped value from the property
		_progressLabel.Text = $"Progress: {(_progressBar.Progress * 100):F0}%";
	}
}
