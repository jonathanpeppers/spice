namespace Spice;

/// <summary>
/// Represents an image on screen. Set the Source property such as "spice".
/// Android -> Android.Widget.ImageView
/// iOS -> UIKit.UIImageView
/// </summary>
public partial class Image : View
{
	/// <summary>
	/// Source is a string name of an image without the extension. For "spice.svg", just pass "spice".
	/// Currently URLs and absolute file paths are not supported.
	/// </summary>
	[ObservableProperty]
	string _source = "";
}