namespace Spice;

/// <summary>
/// You know the button! Use the Clicked event.
/// Android -> Android.Widget.Button
/// iOS -> UIKit.UIButton
/// </summary>
public partial class Button : View
{
	bool _isTextColorSet;
	Color? _themedTextColor;

	/// <summary>
	/// Text on the button
	/// </summary>
	[ObservableProperty]
	string _text = "";

	/// <summary>
	/// Color of the text
	/// </summary>
	[ObservableProperty]
	Color? _textColor;

	/// <summary>
	/// Action to run when the button is tapped or clicked
	/// </summary>
	[ObservableProperty]
	Action<Button>? _clicked;

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		_isApplyingTheme = true;
		_themedTextColor = theme.TextColor;
		if (!_isTextColorSet)
			TextColor = _themedTextColor;
		_themedBackgroundColor = theme.AccentColor;
		if (!_isBackgroundColorSet)
			BackgroundColor = _themedBackgroundColor;
		_isApplyingTheme = false;
	}

	partial void OnTextColorChanging(Color? value)
	{
		if (!_isApplyingTheme)
			_isTextColorSet = value is not null;
	}
}