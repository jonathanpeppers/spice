namespace Spice;

/// <summary>
/// A container control that wraps around an item of content and provides context menu items 
/// that are revealed by a swipe gesture.
/// Android -> Custom view with gesture detection
/// iOS -> UIKit.UIView with gesture recognizers
/// </summary>
public partial class SwipeView : View
{
	View? _content;

	/// <summary>
	/// Gets or sets the content (child view) of this SwipeView.
	/// </summary>
	public View? Content
	{
		get => _content;
		set
		{
			var oldContent = _content;

			if (SetProperty(ref _content, value))
			{
				OnContentChanged(oldContent, value);
			}
		}
	}

	SwipeItems? _leftItems;
	SwipeItems? _rightItems;
	SwipeItems? _topItems;
	SwipeItems? _bottomItems;

	/// <summary>
	/// Gets or sets the swipe items that can be invoked when the control is swiped from the left side.
	/// </summary>
	public SwipeItems? LeftItems
	{
		get => _leftItems;
		set => SetProperty(ref _leftItems, value);
	}

	/// <summary>
	/// Gets or sets the swipe items that can be invoked when the control is swiped from the right side.
	/// </summary>
	public SwipeItems? RightItems
	{
		get => _rightItems;
		set => SetProperty(ref _rightItems, value);
	}

	/// <summary>
	/// Gets or sets the swipe items that can be invoked when the control is swiped from the top down.
	/// </summary>
	public SwipeItems? TopItems
	{
		get => _topItems;
		set => SetProperty(ref _topItems, value);
	}

	/// <summary>
	/// Gets or sets the swipe items that can be invoked when the control is swiped from the bottom up.
	/// </summary>
	public SwipeItems? BottomItems
	{
		get => _bottomItems;
		set => SetProperty(ref _bottomItems, value);
	}

	/// <summary>
	/// Gets or sets the number of device-independent units that trigger a swipe gesture 
	/// to fully reveal swipe items. Default is 0 (system default).
	/// </summary>
	[ObservableProperty]
	double _threshold;

	/// <summary>
	/// Gets or sets the action that is invoked when a swipe gesture starts.
	/// The action receives the SwipeView and SwipeDirection as parameters.
	/// </summary>
	[ObservableProperty]
	Action<SwipeView, SwipeDirection>? _swipeStarted;

	/// <summary>
	/// Gets or sets the action that is invoked as the swipe gesture changes.
	/// The action receives the SwipeView, SwipeDirection, and offset as parameters.
	/// </summary>
	[ObservableProperty]
	Action<SwipeView, SwipeDirection, double>? _swipeChanging;

	/// <summary>
	/// Gets or sets the action that is invoked when a swipe gesture ends.
	/// The action receives the SwipeView, SwipeDirection, and whether the view is open.
	/// </summary>
	[ObservableProperty]
	Action<SwipeView, SwipeDirection, bool>? _swipeEnded;

	partial void OnContentChanged(View? oldContent, View? newContent);

	/// <summary>
	/// Programmatically opens the swipe items in the specified direction.
	/// </summary>
	/// <param name="direction">The direction to open the swipe items.</param>
	public void Open(SwipeDirection direction)
	{
		OnOpen(direction);
	}

	/// <summary>
	/// Programmatically closes any open swipe items.
	/// </summary>
	public void Close()
	{
		OnClose();
	}

	partial void OnOpen(SwipeDirection direction);
	partial void OnClose();
}
