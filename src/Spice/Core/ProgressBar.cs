namespace Spice;

/// <summary>
/// Displays a progress indicator showing the completion percentage of a task.
/// Android -> Android.Widget.ProgressBar
/// iOS -> UIKit.UIProgressView
/// </summary>
public partial class ProgressBar : View
{
	/// <summary>
	/// The progress value, ranging from 0.0 (0%) to 1.0 (100%)
	/// </summary>
	[ObservableProperty]
	double _progress;
}
