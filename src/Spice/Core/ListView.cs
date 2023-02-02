using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Spice;

public partial class ListView<T> : View
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
