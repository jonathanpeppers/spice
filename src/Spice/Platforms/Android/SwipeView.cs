using Android.Content;
using Android.Views;
using Android.Widget;

namespace Spice;

public partial class SwipeView
{
	/// <summary>
	/// Returns swipeView.NativeView
	/// </summary>
	/// <param name="swipeView">The Spice.SwipeView</param>
	public static implicit operator Android.Views.ViewGroup(SwipeView swipeView) => (Android.Views.ViewGroup)swipeView._nativeView.Value;

	static FrameLayout Create(Context context) => new(context);

	/// <summary>
	/// A container control that wraps around an item of content and provides context menu items 
	/// that are revealed by a swipe gesture.
	/// Android -> Custom view with gesture detection
	/// </summary>
	public SwipeView() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public SwipeView(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected SwipeView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected SwipeView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		SetupGestureDetector();
	}

	/// <summary>
	/// The underlying Android.Views.ViewGroup (FrameLayout)
	/// </summary>
	public new Android.Views.ViewGroup NativeView => (Android.Views.ViewGroup)_nativeView.Value;

	GestureDetector? _gestureDetector;
	Android.Views.View? _contentView;
	LinearLayout? _swipeItemsLayout;
	SwipeDirection? _currentSwipeDirection;
	float _startX;
	float _startY;

	void SetupGestureDetector()
	{
		var listener = new SwipeGestureListener(this);
		_gestureDetector = new GestureDetector(Platform.Context, listener);

		NativeView.Touch += (sender, e) =>
		{
			_gestureDetector?.OnTouchEvent(e.Event);

			if (e.Event?.Action == MotionEventActions.Up || e.Event?.Action == MotionEventActions.Cancel)
			{
				HandleTouchEnd(e.Event);
			}
		};
	}

	class SwipeGestureListener : GestureDetector.SimpleOnGestureListener
	{
		readonly SwipeView _swipeView;

		public SwipeGestureListener(SwipeView swipeView)
		{
			_swipeView = swipeView;
		}

		public override bool OnDown(MotionEvent? e)
		{
			if (e != null)
			{
				_swipeView._startX = e.GetX();
				_swipeView._startY = e.GetY();
				_swipeView._currentSwipeDirection = null;
			}
			return true;
		}

		public override bool OnScroll(MotionEvent? e1, MotionEvent? e2, float distanceX, float distanceY)
		{
			if (e2 == null || _swipeView._contentView == null)
				return false;

			var deltaX = e2.GetX() - _swipeView._startX;
			var deltaY = e2.GetY() - _swipeView._startY;

			// Determine swipe direction
			SwipeDirection? direction = null;
			if (Math.Abs(deltaX) > Math.Abs(deltaY))
			{
				direction = deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left;
			}
			else
			{
				direction = deltaY > 0 ? SwipeDirection.Down : SwipeDirection.Up;
			}

			// Only handle horizontal swipes for now
			if (direction == SwipeDirection.Left || direction == SwipeDirection.Right)
			{
				if (_swipeView._currentSwipeDirection == null)
				{
					_swipeView._currentSwipeDirection = direction;
					_swipeView.SwipeStarted?.Invoke(_swipeView, direction.Value);
				}

				// Update content view position
				_swipeView._contentView.TranslationX = deltaX;
				_swipeView.SwipeChanging?.Invoke(_swipeView, direction.Value, Math.Abs(deltaX));
			}

			return true;
		}
	}

	void HandleTouchEnd(MotionEvent? e)
	{
		if (_currentSwipeDirection == null || _contentView == null)
			return;

		var deltaX = e != null ? e.GetX() - _startX : 0;
		var isOpen = Math.Abs(deltaX) > 150; // Simple threshold

		if (isOpen && _currentSwipeDirection == SwipeDirection.Left && RightItems != null)
		{
			ShowSwipeItems(RightItems, SwipeDirection.Left);
		}
		else if (isOpen && _currentSwipeDirection == SwipeDirection.Right && LeftItems != null)
		{
			ShowSwipeItems(LeftItems, SwipeDirection.Right);
		}
		else
		{
			ResetPosition();
		}

		SwipeEnded?.Invoke(this, _currentSwipeDirection.Value, isOpen);
		_currentSwipeDirection = null;
	}

	void ShowSwipeItems(SwipeItems items, SwipeDirection direction)
	{
		if (_contentView == null)
			return;

		// Clean up existing swipe items layout
		_swipeItemsLayout?.RemoveFromParent();

		// Create swipe items container
		_swipeItemsLayout = new LinearLayout(Platform.Context)
		{
			Orientation = Orientation.Horizontal,
			LayoutParameters = new FrameLayout.LayoutParams(
				Android.Views.ViewGroup.LayoutParams.WrapContent,
				Android.Views.ViewGroup.LayoutParams.MatchParent)
		};

		foreach (var item in items.Items)
		{
			if (!item.IsVisible)
				continue;

			var button = new Android.Widget.Button(Platform.Context)
			{
				Text = item.Text ?? "",
				LayoutParameters = new LinearLayout.LayoutParams(200, Android.Views.ViewGroup.LayoutParams.MatchParent)
			};

			if (item.BackgroundColor != null)
			{
				button.SetBackgroundColor(item.BackgroundColor.Value.ToAndroidColor());
			}

			// Capture item reference for the closure
			var swipeItem = item;
			button.Click += (s, e) =>
			{
				swipeItem.Invoked?.Invoke(swipeItem);

				if (items.SwipeBehaviorOnInvoked == SwipeBehaviorOnInvoked.Close ||
					items.SwipeBehaviorOnInvoked == SwipeBehaviorOnInvoked.Auto)
				{
					Close();
				}
			};

			_swipeItemsLayout.AddView(button);
		}

		// Position swipe items layout
		var layoutParams = (FrameLayout.LayoutParams)_swipeItemsLayout.LayoutParameters;
		if (direction == SwipeDirection.Left)
		{
			layoutParams!.Gravity = GravityFlags.End;
		}
		else
		{
			layoutParams!.Gravity = GravityFlags.Start;
		}

		NativeView.AddView(_swipeItemsLayout, 0);

		// Animate content view to reveal swipe items
		_swipeItemsLayout.Measure(
			Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
			Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
		var itemsWidth = _swipeItemsLayout.MeasuredWidth;

		_contentView.Animate()
			.TranslationX(direction == SwipeDirection.Left ? -itemsWidth : itemsWidth)
			.SetDuration(300)
			.Start();
	}

	void ResetPosition()
	{
		if (_contentView == null)
			return;

		_contentView.Animate()
			.TranslationX(0)
			.SetDuration(300)
			.WithEndAction(new Java.Lang.Runnable(() =>
			{
				_swipeItemsLayout?.RemoveFromParent();
				_swipeItemsLayout = null;
			}))
			.Start();
	}

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Remove old content
		if (oldContent != null)
		{
			NativeView.RemoveView((Android.Views.View)oldContent);
		}

		_contentView?.RemoveFromParent();

		// Add new content
		if (newContent != null)
		{
			_contentView = new FrameLayout(Platform.Context)
			{
				LayoutParameters = new FrameLayout.LayoutParams(
					Android.Views.ViewGroup.LayoutParams.MatchParent,
					Android.Views.ViewGroup.LayoutParams.MatchParent)
			};
			_contentView.AddView(newContent);
			NativeView.AddView(_contentView);
		}
	}

	partial void OnOpen(SwipeDirection direction)
	{
		var items = direction switch
		{
			SwipeDirection.Left => RightItems,
			SwipeDirection.Right => LeftItems,
			SwipeDirection.Up => BottomItems,
			SwipeDirection.Down => TopItems,
			_ => null
		};

		if (items != null)
		{
			ShowSwipeItems(items, direction);
		}
	}

	partial void OnClose()
	{
		ResetPosition();
	}
}
