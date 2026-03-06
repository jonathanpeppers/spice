namespace Spice;

/// <summary>
/// Control for text input, like a form field.
/// Android -> Android.Widget.EditText
/// iOS -> UIKit.UITextField
/// </summary>
public partial class Entry : View
{
	/// <summary>
	/// The text input by the user (also displayed)
	/// </summary>
	[ObservableProperty]
	string _text = "";

	/// <summary>
	/// Color of the text
	/// </summary>
	[ObservableProperty]
	Color? _textColor;

	/// <summary>
	/// Set to true to toggle password box
	/// </summary>
	[ObservableProperty]
	bool _isPassword = false;

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		_isApplyingTheme = true;
		if (CanApplyTheme((int)ThemeProperty.TextColor))
			TextColor = theme.TextColor;
		_isApplyingTheme = false;
	}

	partial void OnTextColorChanging(Color? value) => TrackExplicit((int)ThemeProperty.TextColor, value);
}

