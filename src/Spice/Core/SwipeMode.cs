namespace Spice;

/// <summary>
/// Defines the effect of a swipe interaction on SwipeItems.
/// </summary>
public enum SwipeMode
{
	/// <summary>
	/// Swipe items are revealed and remain visible until dismissed.
	/// </summary>
	Reveal,

	/// <summary>
	/// Swipe items are executed immediately when the swipe gesture is detected.
	/// </summary>
	Execute
}
