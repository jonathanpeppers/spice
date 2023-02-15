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
	IEnumerable<T>? _items;

#if !VANILLA
	partial void OnItemsChanging(IEnumerable<T>? value)
	{
		if (_items is ObservableCollection<T> obs)
			obs.CollectionChanged -= OnCollectionChanged;
	}

	partial void OnItemsChanged(IEnumerable<T>? value)
	{
		if (value is ObservableCollection<T> obs)
			obs.CollectionChanged += OnCollectionChanged;
	}
#endif

	[ObservableProperty]
	Func<T, View>? _itemTemplate;

	[ObservableProperty]
	Action<View, T>? _recycled;
}

/// <summary>
/// Non-generic version of CollectionView&lt;T&gt;. A view with a scrolling collection of items
/// Android -> ???
/// iOS -> UICollectionView
/// </summary>
public class CollectionView : CollectionView<object> { }
