namespace Spice;

public partial class SwipeView
{
	/// <summary>
	/// Returns swipeView.NativeView
	/// </summary>
	/// <param name="swipeView">The Spice.SwipeView</param>
	public static implicit operator UIView(SwipeView swipeView) => swipeView.NativeView;

	/// <summary>
	/// A container control that wraps around an item of content and provides context menu items 
	/// that are revealed by a swipe gesture.
	/// iOS -> UIKit.UIView with gesture recognizers
	/// </summary>
	public SwipeView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None, ClipsToBounds = true })
	{
		SetupGestureRecognizers();
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public SwipeView(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None, ClipsToBounds = true })
	{
		SetupGestureRecognizers();
	}

	UIPanGestureRecognizer? _panGesture;
	UIView? _contentView;
	UIView? _swipeItemsView;
	SwipeDirection? _currentSwipeDirection;
	nfloat _startPosition;

	void SetupGestureRecognizers()
	{
		_panGesture = new UIPanGestureRecognizer(HandlePan);
		NativeView.AddGestureRecognizer(_panGesture);
	}

	void HandlePan(UIPanGestureRecognizer gesture)
	{
		var translation = gesture.TranslationInView(NativeView);
		var velocity = gesture.VelocityInView(NativeView);

		switch (gesture.State)
		{
			case UIGestureRecognizerState.Began:
				_startPosition = _contentView?.Frame.X ?? 0;
				_currentSwipeDirection = null;
				break;

			case UIGestureRecognizerState.Changed:
				if (_contentView == null)
					break;

				// Determine swipe direction based on velocity
				SwipeDirection? direction = null;
				if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
				{
					direction = velocity.X > 0 ? SwipeDirection.Right : SwipeDirection.Left;
				}
				else
				{
					direction = velocity.Y > 0 ? SwipeDirection.Down : SwipeDirection.Up;
				}

				// Only handle horizontal swipes for now (most common use case)
				if (direction == SwipeDirection.Left || direction == SwipeDirection.Right)
				{
					if (_currentSwipeDirection == null)
					{
						_currentSwipeDirection = direction;
						SwipeStarted?.Invoke(this, direction.Value);
					}

					var newX = _startPosition + translation.X;
					_contentView.Frame = new CGRect(newX, _contentView.Frame.Y, _contentView.Frame.Width, _contentView.Frame.Height);

					SwipeChanging?.Invoke(this, direction.Value, Math.Abs(translation.X));
				}
				break;

			case UIGestureRecognizerState.Ended:
			case UIGestureRecognizerState.Cancelled:
				if (_currentSwipeDirection != null)
				{
					var isOpen = Math.Abs(translation.X) > 50; // Simple threshold
					
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
				break;
		}
	}

	void ShowSwipeItems(SwipeItems items, SwipeDirection direction)
	{
		if (_contentView == null)
			return;

		// Clean up existing swipe items view
		_swipeItemsView?.RemoveFromSuperview();

		// Create swipe items container
		_swipeItemsView = new UIView
		{
			BackgroundColor = UIColor.Clear
		};

		nfloat x = 0;
		nfloat itemWidth = 75;
		nfloat totalWidth = items.Items.Count * itemWidth;

		foreach (var item in items.Items)
		{
			if (!item.IsVisible)
				continue;

			var button = UIButton.FromType(UIButtonType.System);
			button.Frame = new CGRect(x, 0, itemWidth, NativeView.Frame.Height);
			button.SetTitle(item.Text ?? "", UIControlState.Normal);
			button.BackgroundColor = item.BackgroundColor?.ToUIColor() ?? UIColor.LightGray;
			button.SetTitleColor(UIColor.White, UIControlState.Normal);

			// Capture item reference for the closure
			var swipeItem = item;
			button.TouchUpInside += (s, e) =>
			{
				swipeItem.Invoked?.Invoke(swipeItem);
				
				if (items.SwipeBehaviorOnInvoked == SwipeBehaviorOnInvoked.Close ||
					items.SwipeBehaviorOnInvoked == SwipeBehaviorOnInvoked.Auto)
				{
					Close();
				}
			};

			_swipeItemsView.AddSubview(button);
			x += itemWidth;
		}

		_swipeItemsView.Frame = direction == SwipeDirection.Left
			? new CGRect(NativeView.Frame.Width, 0, totalWidth, NativeView.Frame.Height)
			: new CGRect(-totalWidth, 0, totalWidth, NativeView.Frame.Height);

		NativeView.InsertSubviewBelow(_swipeItemsView, _contentView);

		// Animate content view to reveal swipe items
		UIView.Animate(0.3, () =>
		{
			if (_contentView != null)
			{
				var newX = direction == SwipeDirection.Left ? -totalWidth : totalWidth;
				_contentView.Frame = new CGRect(newX, _contentView.Frame.Y, _contentView.Frame.Width, _contentView.Frame.Height);
			}
		});
	}

	void ResetPosition()
	{
		if (_contentView == null)
			return;

		UIView.Animate(0.3, () =>
		{
			_contentView.Frame = new CGRect(0, 0, NativeView.Frame.Width, NativeView.Frame.Height);
		}, () =>
		{
			_swipeItemsView?.RemoveFromSuperview();
			_swipeItemsView = null;
		});
	}

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Remove old content
		if (oldContent != null)
		{
			((UIView)oldContent).RemoveFromSuperview();
		}

		_contentView?.RemoveFromSuperview();

		// Add new content
		if (newContent != null)
		{
			_contentView = new UIView
			{
				Frame = new CGRect(0, 0, NativeView.Frame.Width, NativeView.Frame.Height),
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
			};
			_contentView.AddSubview(newContent);
			newContent.UpdateAlign();
			NativeView.AddSubview(_contentView);
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
