namespace Spice;

/// <summary>
/// Represents text on screen. Set the Text property to a string value.
/// Android -> Android.Widget.TextView
/// iOS -> UIKit.UILabel
/// </summary>
public partial class Label : View
{
	bool _isTextColorSet;
	Color? _themedTextColor;

	/// <summary>
	/// The text to display
	/// </summary>
	[ObservableProperty]
	string _text = "";

	/// <summary>
	/// Color of the text
	/// </summary>
	[ObservableProperty]
	Color? _textColor;

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		_isApplyingTheme = true;
		_themedTextColor = theme.TextColor;
		if (!_isTextColorSet)
			TextColor = _themedTextColor;
		_isApplyingTheme = false;
	}

	partial void OnTextColorChanging(Color? value)
	{
		if (!_isApplyingTheme)
			_isTextColorSet = value is not null;
	}
}