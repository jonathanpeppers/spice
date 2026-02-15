namespace Spice;

public partial class RefreshView
{
	/// <summary>
	/// Returns refreshView.NativeView
	/// </summary>
	/// <param name="refreshView">The Spice.RefreshView</param>
	public static implicit operator UIScrollView(RefreshView refreshView) => refreshView.NativeView;

	/// <summary>
	/// A container view that provides pull-to-refresh functionality for scrollable content.
	/// Wrap a scrollable view (such as ScrollView, ListView, or CollectionView) to enable pull-to-refresh.
	/// Android -> AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
	/// iOS -> UIKit.UIScrollView with UIRefreshControl
	/// </summary>
	public RefreshView() : base(_ => new UIScrollView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public RefreshView(CGRect frame) : base(_ => new UIScrollView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected RefreshView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIScrollView
	/// </summary>
	public new UIScrollView NativeView => (UIScrollView)_nativeView.Value;

	WeakReference<UIRefreshControl>? _refreshControl;

	UIRefreshControl GetOrCreateRefreshControl()
	{
		if (_refreshControl != null && _refreshControl.TryGetTarget(out var control))
		{
			return control;
		}

		var refreshControl = new UIRefreshControl();
		refreshControl.ValueChanged += OnRefreshControlValueChanged;
		_refreshControl = new WeakReference<UIRefreshControl>(refreshControl);
		
		// Add to scrollView
		NativeView.RefreshControl = refreshControl;

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

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Remove old content
		if (oldContent != null)
		{
			((UIView)oldContent).RemoveFromSuperview();
		}

		// Add new content
		if (newContent != null)
		{
			NativeView.AddSubview(newContent);
			newContent.UpdateAlign();
			
			// If the content is a scroll view, use its native scroll capabilities
			if (newContent is ScrollView scrollView)
			{
				// Copy the scroll view's content to our scroll view
				var scrollNativeView = (UIScrollView)(UIView)scrollView;
				foreach (UIView subview in scrollNativeView.Subviews)
				{
					subview.RemoveFromSuperview();
					NativeView.AddSubview(subview);
				}
				
				// Sync scroll properties
				NativeView.ContentSize = scrollNativeView.ContentSize;
				NativeView.AlwaysBounceVertical = scrollNativeView.AlwaysBounceVertical;
				NativeView.AlwaysBounceHorizontal = scrollNativeView.AlwaysBounceHorizontal;
				NativeView.ShowsVerticalScrollIndicator = scrollNativeView.ShowsVerticalScrollIndicator;
				NativeView.ShowsHorizontalScrollIndicator = scrollNativeView.ShowsHorizontalScrollIndicator;
			}
			else
			{
				// For non-scroll views, make sure vertical bouncing is enabled
				NativeView.AlwaysBounceVertical = true;
			}
		}
	}

	partial void OnIsRefreshingChanged(bool value)
	{
		var refreshControl = GetOrCreateRefreshControl();
		
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

	partial void OnRefreshColorChanged(Color? value)
	{
		var refreshControl = GetOrCreateRefreshControl();
		refreshControl.TintColor = value.ToUIColor();
	}

	partial void OnCommandChanged(Action? value)
	{
		// Ensure refresh control is created so the command can be triggered
		GetOrCreateRefreshControl();
	}
}
