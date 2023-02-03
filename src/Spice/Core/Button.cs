namespace Spice;

/// <summary>
/// You know the button! Use the Clicked event.
/// Android -> Android.Widget.Button
/// iOS -> UIKit.UIButton
/// </summary>
public partial class Button : View
{
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
}