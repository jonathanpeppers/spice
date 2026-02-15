using System.Collections;
using System.Collections.Specialized;

namespace Spice;

public partial class CollectionView
{
	/// <summary>
	/// Returns collectionView.NativeView
	/// </summary>
	/// <param name="collectionView">The Spice.CollectionView</param>
	public static implicit operator UICollectionView(CollectionView collectionView) => collectionView.NativeView;

	/// <summary>
	/// A view for presenting collections of data using a lambda-based item template.
	/// Android -> AndroidX.RecyclerView.Widget.RecyclerView
	/// iOS -> UIKit.UICollectionView
	/// </summary>
	public CollectionView() : base(v => new SpiceCollectionView((CollectionView)v) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected CollectionView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UICollectionView
	/// </summary>
	public new UICollectionView NativeView => (UICollectionView)_nativeView.Value;

	SpiceCollectionViewDataSource? _dataSource;
	SpiceCollectionViewDelegate? _delegate;
	INotifyCollectionChanged? _observableCollection;

	partial void OnItemsSourceChanged(IEnumerable? value)
	{
		// Unsubscribe from old collection
		if (_observableCollection != null)
		{
			_observableCollection.CollectionChanged -= OnCollectionChanged;
			_observableCollection = null;
		}

		// Subscribe to new collection if it's observable
		if (value is INotifyCollectionChanged observable)
		{
			_observableCollection = observable;
			observable.CollectionChanged += OnCollectionChanged;
		}

		RefreshData();
	}

	partial void OnItemTemplateChanged(Func<object, View>? value)
	{
		RefreshData();
	}

	partial void OnOrientationChanged(Orientation value)
	{
		if (!_nativeView.IsValueCreated)
			return;

		var layout = (UICollectionViewFlowLayout)NativeView.CollectionViewLayout;
		layout.ScrollDirection = value == Orientation.Horizontal
			? UICollectionViewScrollDirection.Horizontal
			: UICollectionViewScrollDirection.Vertical;

		RefreshData();
	}

	partial void OnSelectionModeChanged(SelectionMode value)
	{
		if (!_nativeView.IsValueCreated)
			return;

		NativeView.AllowsSelection = value != SelectionMode.None;
		NativeView.AllowsMultipleSelection = value == SelectionMode.Multiple;
	}

	partial void OnSelectedItemChanged(object? value)
	{
		if (!_nativeView.IsValueCreated || value == null)
			return;

		if (_dataSource != null)
		{
			// Use cached items for O(1) lookup
			int index = _dataSource._itemsCache.IndexOf(value);
			if (index >= 0)
			{
				var indexPath = NSIndexPath.FromRowSection(index, 0);
				NativeView.SelectItem(indexPath, false, UICollectionViewScrollPosition.None);
			}
		}
	}

	partial void OnItemSpacingChanged(double value)
	{
		if (!_nativeView.IsValueCreated)
			return;

		var layout = (UICollectionViewFlowLayout)NativeView.CollectionViewLayout;
		layout.MinimumLineSpacing = (nfloat)value;
		layout.MinimumInteritemSpacing = (nfloat)value;
	}

	void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		RefreshData();
	}

	void RefreshData()
	{
		if (!_nativeView.IsValueCreated)
			return;

		if (_dataSource != null)
			_dataSource.UpdateItemsCache(this);

		NativeView.ReloadData();
	}

	class SpiceCollectionView : UICollectionView
	{
		readonly WeakReference<CollectionView> _parentRef;

		public SpiceCollectionView(CollectionView parent) : base(CGRect.Empty, new UICollectionViewFlowLayout
		{
			ScrollDirection = UICollectionViewScrollDirection.Vertical,
			MinimumLineSpacing = 0,
			MinimumInteritemSpacing = 0
		})
		{
			_parentRef = new WeakReference<CollectionView>(parent);
			RegisterClassForCell(typeof(SpiceCollectionViewCell), "SpiceCell");

			parent._dataSource = new SpiceCollectionViewDataSource(parent);
			parent._delegate = new SpiceCollectionViewDelegate(parent);
			DataSource = parent._dataSource;
			Delegate = parent._delegate;

			BackgroundColor = UIColor.Clear;
		}
	}

	class SpiceCollectionViewDataSource : UICollectionViewDataSource
	{
		readonly WeakReference<CollectionView> _parentRef;
		List<object> _itemsCache = new();

		public SpiceCollectionViewDataSource(CollectionView parent)
		{
			_parentRef = new WeakReference<CollectionView>(parent);
			UpdateItemsCache(parent);
		}

		public void UpdateItemsCache(CollectionView parent)
		{
			_itemsCache.Clear();
			if (parent.ItemsSource != null)
			{
				foreach (var item in parent.ItemsSource)
					_itemsCache.Add(item);
			}
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return _itemsCache.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (SpiceCollectionViewCell)collectionView.DequeueReusableCell("SpiceCell", indexPath);

			if (!_parentRef.TryGetTarget(out var parent) || parent.ItemTemplate == null)
				return cell;

			// Get the item at this index from cache
			if (indexPath.Row >= 0 && indexPath.Row < _itemsCache.Count)
			{
				var item = _itemsCache[(int)indexPath.Row];

				// Clear existing content
				foreach (var subview in cell.ContentView.Subviews)
					subview.RemoveFromSuperview();

				// Create and add the view
				var view = parent.ItemTemplate(item);
				cell.ContentView.AddSubview(view);
				view.UpdateAlign();
			}

			return cell;
		}
	}

	class SpiceCollectionViewDelegate : UICollectionViewDelegateFlowLayout
	{
		readonly WeakReference<CollectionView> _parentRef;

		public SpiceCollectionViewDelegate(CollectionView parent)
		{
			_parentRef = new WeakReference<CollectionView>(parent);
		}

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			if (!_parentRef.TryGetTarget(out var parent))
				return new CGSize(100, 44);

			var flowLayout = (UICollectionViewFlowLayout)layout;
			
			if (parent.Orientation == Orientation.Horizontal)
			{
				// Horizontal: full height, width determined by content
				return new CGSize(100, collectionView.Frame.Height);
			}
			else
			{
				// Vertical: full width, height determined by content
				return new CGSize(collectionView.Frame.Width, 44);
			}
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (!_parentRef.TryGetTarget(out var parent) || parent.SelectionMode == SelectionMode.None)
				return;

			// Get the item at this index from data source cache
			var dataSource = collectionView.DataSource as SpiceCollectionViewDataSource;
			if (dataSource != null && indexPath.Row >= 0 && indexPath.Row < dataSource._itemsCache.Count)
			{
				var item = dataSource._itemsCache[(int)indexPath.Row];
				
				if (parent.SelectionMode == SelectionMode.Single)
				{
					parent.SelectedItem = item;
				}
			}
		}
	}

	class SpiceCollectionViewCell : UICollectionViewCell
	{
		public SpiceCollectionViewCell(IntPtr handle) : base(handle) { }
	}
}
