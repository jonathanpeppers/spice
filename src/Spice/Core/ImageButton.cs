namespace Spice;

/// <summary>
/// A button that displays an image instead of text. Use the Clicked event.
/// Android -> Android.Widget.ImageButton
/// iOS -> UIKit.UIButton
/// </summary>
public partial class ImageButton : View
{
	/// <summary>
	/// Source is a string name of an image without the extension. For "spice.svg", just pass "spice".
	/// Currently URLs and absolute file paths are not supported.
	/// </summary>
	[ObservableProperty]
	string _source = "";

	/// <summary>
	/// Action to run when the button is tapped or clicked
	/// </summary>
	[ObservableProperty]
	Action<ImageButton>? _clicked;
}
