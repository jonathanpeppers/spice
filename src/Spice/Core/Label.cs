namespace Spice;

/// <summary>
/// Represents text on screen. Set the Text property to a string value.
/// Android -> Android.Widget.TextView
/// iOS -> UIKit.UILabel
/// </summary>
public partial class Label : View
{
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
}