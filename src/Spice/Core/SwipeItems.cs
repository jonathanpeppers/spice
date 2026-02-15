using System.Collections.ObjectModel;

namespace Spice;

/// <summary>
/// Represents a collection of SwipeItem objects for a specific swipe direction.
/// Defines the behavior and appearance of swipe items revealed by a swipe gesture.
/// </summary>
public partial class SwipeItems : ObservableObject
{
	/// <summary>
	/// Gets or sets the swipe mode, which determines how swipe items behave.
	/// Note: Current platform implementations only support Reveal mode.
	/// </summary>
	[ObservableProperty]
	SwipeMode _mode = SwipeMode.Reveal;

	/// <summary>
	/// Gets or sets the behavior of the SwipeView after a swipe item is invoked.
	/// </summary>
	[ObservableProperty]
	SwipeBehaviorOnInvoked _swipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Auto;

	/// <summary>
	/// Gets the collection of SwipeItem objects.
	/// </summary>
	public ObservableCollection<SwipeItem> Items { get; } = new();

	/// <summary>
	/// Initializes a new instance of the SwipeItems class.
	/// </summary>
	public SwipeItems()
	{
	}

	/// <summary>
	/// Initializes a new instance of the SwipeItems class with the specified items.
	/// </summary>
	/// <param name="items">The collection of SwipeItem objects to add.</param>
	public SwipeItems(IEnumerable<SwipeItem> items)
	{
		foreach (var item in items)
		{
			Items.Add(item);
		}
	}
}
