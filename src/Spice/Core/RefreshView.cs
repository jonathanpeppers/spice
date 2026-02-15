namespace Spice;

/// <summary>
/// A container view that provides pull-to-refresh functionality for scrollable content.
/// Wrap a scrollable view (such as ScrollView, ListView, or CollectionView) to enable pull-to-refresh.
/// Android -> AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
/// iOS -> UIKit.UIView with UIRefreshControl attached to child scroll view
/// </summary>
public partial class RefreshView : View
{
	View? _content;

	/// <summary>
	/// Gets or sets the content (child view) of this RefreshView.
	/// Should be a scrollable view like ScrollView for pull-to-refresh to work properly.
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

	/// <summary>
	/// Gets or sets whether the view is currently refreshing.
	/// Set to true to show the refresh indicator; set to false to hide it.
	/// </summary>
	[ObservableProperty]
	bool _isRefreshing;

	/// <summary>
	/// Gets or sets the command to execute when a refresh is triggered.
	/// </summary>
	[ObservableProperty]
	Action? _command;

	/// <summary>
	/// Gets or sets the color of the refresh indicator.
	/// </summary>
	[ObservableProperty]
	Color? _refreshColor;

	partial void OnContentChanged(View? oldContent, View? newContent);
}
