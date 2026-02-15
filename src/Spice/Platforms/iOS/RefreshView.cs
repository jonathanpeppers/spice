namespace Spice;

public partial class RefreshView
{
	/// <summary>
	/// Returns refreshView.NativeView
	/// </summary>
	/// <param name="refreshView">The Spice.RefreshView</param>
	public static implicit operator UIView(RefreshView refreshView) => refreshView.NativeView;

	/// <summary>
	/// A container view that provides pull-to-refresh functionality for scrollable content.
	/// Wrap a scrollable view (such as ScrollView, ListView, or CollectionView) to enable pull-to-refresh.
	/// Android -> AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
	/// iOS -> UIKit.UIView with UIRefreshControl attached to child scroll view
	/// </summary>
	public RefreshView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public RefreshView(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected RefreshView(Func<View, UIView> creator) : base(creator) { }

	WeakReference<UIRefreshControl>? _refreshControl;
	WeakReference<UIScrollView>? _scrollView;

	UIRefreshControl GetOrCreateRefreshControl()
	{
		if (_refreshControl != null && _refreshControl.TryGetTarget(out var control))
		{
			return control;
		}

		var refreshControl = new UIRefreshControl();
		refreshControl.ValueChanged += OnRefreshControlValueChanged;
		_refreshControl = new WeakReference<UIRefreshControl>(refreshControl);

		return refreshControl;
	}

	void OnRefreshControlValueChanged(object? sender, EventArgs e)
	{
		// Trigger command if set
		Command?.Invoke();
		
		// Update IsRefreshing to true when user pulls to refresh
		// The user should set IsRefreshing = false when refresh completes
		if (!IsRefreshing)
		{
			IsRefreshing = true;
		}
	}

	bool TryAttachRefreshControl(UIView view)
	{
		// Look for a UIScrollView in the view hierarchy
		if (view is UIScrollView scrollView)
		{
			var refreshControl = GetOrCreateRefreshControl();
			scrollView.RefreshControl = refreshControl;
			scrollView.AlwaysBounceVertical = true;
			_scrollView = new WeakReference<UIScrollView>(scrollView);
			return true;
		}

		// Check subviews
		if (view.Subviews != null)
		{
			foreach (var subview in view.Subviews)
			{
				if (TryAttachRefreshControl(subview))
				{
					return true;
				}
			}
		}

		return false;
	}

	void TryDetachRefreshControl()
	{
		if (_scrollView != null && _scrollView.TryGetTarget(out var scrollView))
		{
			scrollView.RefreshControl = null;
			_scrollView = null;
		}
	}

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Detach refresh control from old content
		TryDetachRefreshControl();

		// Remove old content
		if (oldContent != null)
		{
			((UIView)oldContent).RemoveFromSuperview();
		}

		// Add new content
		if (newContent != null)
		{
			var contentView = (UIView)newContent;
			NativeView.AddSubview(contentView);
			newContent.UpdateAlign();
			
			// Try to attach refresh control to scrollable content
			TryAttachRefreshControl(contentView);
		}
	}

	partial void OnIsRefreshingChanged(bool value)
	{
		if (_refreshControl != null && _refreshControl.TryGetTarget(out var refreshControl))
		{
			if (value)
			{
				if (!refreshControl.Refreshing)
				{
					refreshControl.BeginRefreshing();
				}
			}
			else
			{
				if (refreshControl.Refreshing)
				{
					refreshControl.EndRefreshing();
				}
			}
		}
	}

	partial void OnRefreshColorChanged(Color? value)
	{
		if (_refreshControl != null && _refreshControl.TryGetTarget(out var refreshControl))
		{
			if (value != null)
			{
				refreshControl.TintColor = value.ToUIColor();
			}
		}
	}
}
