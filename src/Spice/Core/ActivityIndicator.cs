namespace Spice;

/// <summary>
/// Represents a loading spinner on screen. Set the IsRunning property to true to show the spinner.
/// Android -> Android.Widget.ProgressBar (indeterminate)
/// iOS -> UIKit.UIActivityIndicatorView
/// </summary>
public partial class ActivityIndicator : View
{
	/// <summary>
	/// Whether the activity indicator is running (showing the spinner)
	/// </summary>
	[ObservableProperty]
	bool _isRunning;

	/// <summary>
	/// Color of the activity indicator
	/// </summary>
	[ObservableProperty]
	Color? _color;
}
