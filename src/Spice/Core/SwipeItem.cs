namespace Spice;

/// <summary>
/// Represents an individual swipe action item that can be invoked.
/// Each SwipeItem can display text, an optional icon, and a background color.
/// </summary>
public partial class SwipeItem : ObservableObject
{
	/// <summary>
	/// Gets or sets the text displayed on the swipe item.
	/// </summary>
	[ObservableProperty]
	string? _text;

	/// <summary>
	/// Gets or sets the background color of the swipe item.
	/// </summary>
	[ObservableProperty]
	Color? _backgroundColor;

	/// <summary>
	/// Gets or sets a value indicating whether this swipe item is visible.
	/// </summary>
	[ObservableProperty]
	bool _isVisible = true;

	/// <summary>
	/// Gets or sets the action to invoke when the swipe item is tapped or clicked.
	/// </summary>
	[ObservableProperty]
	Action<SwipeItem>? _invoked;
}
