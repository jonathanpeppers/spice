using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Spice;

/// <summary>
/// A view with a scrolling collection of items
/// Android -> ???
/// iOS -> UICollectionView
/// </summary>
public partial class CollectionView<T> : View
{
	[ObservableProperty]
	ObservableCollection<T>? _items;

	partial void OnItemsChanging(ObservableCollection<T>? value)
	{
		if (_items != null)
			_items.CollectionChanged -= OnCollectionChanged;
	}

	partial void OnItemsChanged(ObservableCollection<T>? value)
	{
		if (value != null)
			value.CollectionChanged += OnCollectionChanged;
	}

	void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{

	}

	[ObservableProperty]
	Func<T, View>? _itemTemplate;
}

/// <summary>
/// Non-generic version of CollectionView&lt;T&gt;. A view with a scrolling collection of items
/// Android -> ???
/// iOS -> UICollectionView
/// </summary>
public class CollectionView : CollectionView<object> { }
