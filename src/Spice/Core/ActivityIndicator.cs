namespace Spice;

/// <summary>
/// Represents a loading spinner on screen. Set the IsRunning property to true to show the spinner.
/// Android -> Android.Widget.ProgressBar (indeterminate)
/// iOS -> UIKit.UIActivityIndicatorView
/// </summary>
public partial class ActivityIndicator : View
{
	bool _isColorSet;
	Color? _themedColor;

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

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		_isApplyingTheme = true;
		_themedColor = theme.AccentColor;
		if (!_isColorSet)
			Color = _themedColor;
		_isApplyingTheme = false;
	}

	partial void OnColorChanging(Color? value)
	{
		if (!_isApplyingTheme)
			_isColorSet = value is not null;
	}
}
