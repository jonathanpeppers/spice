using System.Collections;

namespace Spice;

/// <summary>
/// A view for presenting collections of data using a lambda-based item template.
/// Android -> AndroidX.RecyclerView.Widget.RecyclerView
/// iOS -> UIKit.UICollectionView
/// </summary>
public partial class CollectionView : View, IDisposable
{
	/// <summary>
	/// The collection of items to display. Can be any IEnumerable.
	/// </summary>
	[ObservableProperty]
	IEnumerable? _itemsSource;

	/// <summary>
	/// A lambda function that creates a View for each item in the ItemsSource.
	/// The function receives the item object and should return a View to display it.
	/// </summary>
	[ObservableProperty]
	Func<object, View>? _itemTemplate;

	/// <summary>
	/// The orientation of the collection view. Defaults to Vertical.
	/// </summary>
	[ObservableProperty]
	Orientation _orientation;

	/// <summary>
	/// The selection mode for the collection view. Defaults to None.
	/// </summary>
	[ObservableProperty]
	SelectionMode _selectionMode;

	/// <summary>
	/// The currently selected item. Only valid when SelectionMode is Single.
	/// </summary>
	[ObservableProperty]
	object? _selectedItem;

	/// <summary>
	/// The spacing between items in the collection view.
	/// </summary>
	[ObservableProperty]
	double _itemSpacing = 0;

	/// <summary>
	/// Tracks views created by the item template for disposal.
	/// </summary>
	internal readonly List<View> _activeItemViews = [];

	/// <summary>
	/// Creates a view for an item using the ItemTemplate and tracks it for disposal.
	/// Platform implementations should call this instead of ItemTemplate directly.
	/// </summary>
	internal View CreateItemView(object item)
	{
		if (ItemTemplate is null)
			throw new InvalidOperationException("ItemTemplate is not set.");

		var view = ItemTemplate(item);
		_activeItemViews.Add(view);
		return view;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		foreach (var view in _activeItemViews)
		{
			DisposeRecursive(view);
		}
		_activeItemViews.Clear();
	}
}
