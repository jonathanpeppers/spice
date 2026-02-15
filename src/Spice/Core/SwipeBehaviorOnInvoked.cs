namespace Spice;

/// <summary>
/// Defines the behavior of a SwipeView after a swipe item is invoked.
/// </summary>
public enum SwipeBehaviorOnInvoked
{
	/// <summary>
	/// The SwipeView will automatically determine whether to close after invocation.
	/// </summary>
	Auto,

	/// <summary>
	/// The SwipeView will close after a swipe item is invoked.
	/// </summary>
	Close,

	/// <summary>
	/// The SwipeView will remain open after a swipe item is invoked.
	/// </summary>
	RemainOpen
}
