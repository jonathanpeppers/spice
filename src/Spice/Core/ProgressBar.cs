namespace Spice;

/// <summary>
/// Displays a progress indicator showing the completion percentage of a task.
/// Android -> Android.Widget.ProgressBar
/// iOS -> UIKit.UIProgressView
/// </summary>
public partial class ProgressBar : View
{
	double _progress;

	/// <summary>
	/// The progress value, ranging from 0.0 (0%) to 1.0 (100%).
	/// Values outside this range will be clamped.
	/// </summary>
	public double Progress
	{
		get => _progress;
		set
		{
			var clampedValue = Math.Clamp(value, 0.0, 1.0);
			if (SetProperty(ref _progress, clampedValue))
			{
				OnProgressChanged(clampedValue);
			}
		}
	}

	/// <summary>
	/// Called when the Progress property changes
	/// </summary>
	partial void OnProgressChanged(double value);
}
