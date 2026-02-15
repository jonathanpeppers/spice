using System.Collections;

namespace Spice;

/// <summary>
/// A view for presenting collections of data using a lambda-based item template.
/// Android -> AndroidX.RecyclerView.Widget.RecyclerView
/// iOS -> UIKit.UICollectionView
/// </summary>
public partial class CollectionView : View
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
}
