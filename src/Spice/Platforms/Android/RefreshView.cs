using Android.Content;
using AndroidX.SwipeRefreshLayout.Widget;

namespace Spice;

public partial class RefreshView
{
	/// <summary>
	/// Returns refreshView.NativeView
	/// </summary>
	/// <param name="refreshView">The Spice.RefreshView</param>
	public static implicit operator SwipeRefreshLayout(RefreshView refreshView) => refreshView.NativeView;

	static SwipeRefreshLayout Create(Context context)
	{
		var swipeRefresh = new SwipeRefreshLayout(context);
		return swipeRefresh;
	}

	/// <summary>
	/// A container view that provides pull-to-refresh functionality for scrollable content.
	/// Wrap a scrollable view (such as ScrollView, ListView, or CollectionView) to enable pull-to-refresh.
	/// Android -> AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
	/// iOS -> UIKit.UIView with UIRefreshControl attached to child scroll view
	/// </summary>
	public RefreshView() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public RefreshView(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected RefreshView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected RefreshView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		// Set up the refresh listener
		NativeView.Refresh += OnRefresh;
	}

	/// <summary>
	/// The underlying AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
	/// </summary>
	public new SwipeRefreshLayout NativeView => (SwipeRefreshLayout)_nativeView.Value;

	void OnRefresh(object? sender, EventArgs e)
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
			NativeView.RemoveView((Android.Views.View)oldContent);
		}

		// Add new content
		if (newContent != null)
		{
			NativeView.AddView(newContent);
		}
	}

	partial void OnIsRefreshingChanged(bool value)
	{
		NativeView.Refreshing = value;
	}

	partial void OnRefreshColorChanged(Color? value)
	{
		if (value != null)
		{
			NativeView.SetColorSchemeColors(value.ToAndroidInt());
		}
	}
}
