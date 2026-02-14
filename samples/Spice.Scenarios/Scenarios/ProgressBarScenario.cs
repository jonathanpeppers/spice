namespace Spice.Scenarios;

/// <summary>
/// Demonstrates the ProgressBar control with various progress values
/// </summary>
class ProgressBarScenario : StackView
{
	const double ProgressIncrement = 0.1; // 10% increment per click
	ProgressBar _progressBar;
	Label _progressLabel;
	double _progress = 0.0;

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
			HorizontalAlign = Align.Stretch
		};

		var incrementButton = new Button
		{
			Text = "Increment Progress (+10%)",
			Clicked = _ => IncrementProgress()
		};

		var resetButton = new Button
		{
			Text = "Reset Progress",
			Clicked = _ => ResetProgress()
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
		_progress += ProgressIncrement;
		if (_progress > 1.0)
			_progress = 1.0;

		UpdateProgress();
	}

	void ResetProgress()
	{
		_progress = 0.0;
		UpdateProgress();
	}

	void SetProgress(double value)
	{
		_progress = value;
		UpdateProgress();
	}

	void UpdateProgress()
	{
		_progressBar.Progress = _progress;
		_progressLabel.Text = $"Progress: {(_progress * 100):F0}%";
	}
}
