namespace Spice;

/// <summary>
/// Control for multi-line text input.
/// Android -> Android.Widget.EditText (multiline)
/// iOS -> UIKit.UITextView
/// </summary>
public partial class Editor : View
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
	/// Placeholder text displayed when the editor is empty
	/// </summary>
	[ObservableProperty]
	string? _placeholder;

	/// <summary>
	/// Color of the placeholder text
	/// </summary>
	[ObservableProperty]
	Color? _placeholderColor;

	/// <summary>
	/// Whether the editor automatically expands to fit content
	/// </summary>
	[ObservableProperty]
	bool _autoSize = false;
}
