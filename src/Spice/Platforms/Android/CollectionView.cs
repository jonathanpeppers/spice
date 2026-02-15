using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using System.Collections;
using System.Collections.Specialized;

namespace Spice;

public partial class CollectionView
{
	/// <summary>
	/// Returns collectionView.NativeView
	/// </summary>
	/// <param name="collectionView">The Spice.CollectionView</param>
	public static implicit operator RecyclerView(CollectionView collectionView) => collectionView.NativeView;

	static RecyclerView Create(Context context) => new(context);

	/// <summary>
	/// A view for presenting collections of data using a lambda-based item template.
	/// Android -> AndroidX.RecyclerView.Widget.RecyclerView
	/// iOS -> UIKit.UICollectionView
	/// </summary>
	public CollectionView() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public CollectionView(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected CollectionView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected CollectionView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		// Set up RecyclerView
		var layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Vertical, false);
		NativeView.SetLayoutManager(layoutManager);
	}

	/// <summary>
	/// The underlying AndroidX.RecyclerView.Widget.RecyclerView
	/// </summary>
	public new RecyclerView NativeView => (RecyclerView)_nativeView.Value;

	SpiceRecyclerViewAdapter? _adapter;
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

		var layoutManager = NativeView.GetLayoutManager() as LinearLayoutManager;
		if (layoutManager != null)
		{
			layoutManager.Orientation = value == Orientation.Horizontal
				? LinearLayoutManager.Horizontal
				: LinearLayoutManager.Vertical;
		}

		RefreshData();
	}

	partial void OnSelectionModeChanged(SelectionMode value)
	{
		// Selection is handled in the adapter
		RefreshData();
	}

	partial void OnSelectedItemChanged(object? value)
	{
		if (_adapter != null)
		{
			_adapter.NotifyDataSetChanged();
		}
	}

	partial void OnItemSpacingChanged(double value)
	{
		if (!_nativeView.IsValueCreated)
			return;

		// Remove existing decoration
		while (NativeView.ItemDecorationCount > 0)
		{
			NativeView.RemoveItemDecorationAt(0);
		}

		// Add new decoration with updated spacing
		if (value > 0)
		{
			NativeView.AddItemDecoration(new SpaceItemDecoration(value.ToPixels()));
		}
	}

	void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		RefreshData();
	}

	void RefreshData()
	{
		if (!_nativeView.IsValueCreated)
			return;

		if (_adapter == null)
		{
			_adapter = new SpiceRecyclerViewAdapter(this);
			NativeView.SetAdapter(_adapter);
		}
		else
		{
			_adapter.UpdateItems(this);
			_adapter.NotifyDataSetChanged();
		}
	}

	class SpiceRecyclerViewAdapter : RecyclerView.Adapter
	{
		readonly WeakReference<CollectionView> _parentRef;
		internal List<object> _items = new();

		public SpiceRecyclerViewAdapter(CollectionView parent)
		{
			_parentRef = new WeakReference<CollectionView>(parent);
			UpdateItems(parent);
		}

		public void UpdateItems(CollectionView parent)
		{
			_items.Clear();
			if (parent.ItemsSource != null)
			{
				foreach (var item in parent.ItemsSource)
				{
					_items.Add(item);
				}
			}
		}

		public override int ItemCount => _items.Count;

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			if (!_parentRef.TryGetTarget(out var parent) || parent.ItemTemplate == null)
				return;

			var viewHolder = (SpiceViewHolder)holder;

			if (position >= 0 && position < _items.Count)
			{
				var item = _items[position];

				// Recycle existing item views
				if (viewHolder.CurrentItemView != null)
				{
					parent.RecycleItemView(viewHolder.CurrentItemView);
					viewHolder.Container.RemoveAllViews();
					viewHolder.CurrentItemView = null;
				}

				// Create and add the view
				var view = parent.CreateItemView(item);
				viewHolder.CurrentItemView = view;
				viewHolder.Container.AddView((Android.Views.View)view);

				// Handle selection
				viewHolder.ItemView.Clickable = parent.SelectionMode != SelectionMode.None;
				if (parent.SelectionMode == SelectionMode.Single)
				{
					viewHolder.ItemView.Selected = Equals(item, parent.SelectedItem);
				}
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var container = new Android.Widget.FrameLayout(parent.Context!)
			{
				LayoutParameters = new ViewGroup.LayoutParams(
					ViewGroup.LayoutParams.MatchParent,
					ViewGroup.LayoutParams.WrapContent)
			};

			var holder = new SpiceViewHolder(container, _parentRef);
			return holder;
		}
	}

	class SpiceViewHolder : RecyclerView.ViewHolder
	{
		readonly WeakReference<CollectionView> _parentRef;

		public Android.Widget.FrameLayout Container { get; }
		public View? CurrentItemView { get; set; }

		public SpiceViewHolder(Android.Widget.FrameLayout container, WeakReference<CollectionView> parentRef)
			: base(container)
		{
			Container = container;
			_parentRef = parentRef;

			ItemView.Click += OnItemClick;
		}

		void OnItemClick(object? sender, EventArgs e)
		{
			if (!_parentRef.TryGetTarget(out var parent))
				return;

			if (parent.SelectionMode == SelectionMode.None)
				return;

			var position = BindingAdapterPosition;
			if (position == RecyclerView.NoPosition)
				return;

			// Get adapter to access cached items
			var adapter = ItemView.Parent is RecyclerView recyclerView 
				? recyclerView.GetAdapter() as SpiceRecyclerViewAdapter 
				: null;

			if (adapter != null && position >= 0 && position < adapter._items.Count)
			{
				var item = adapter._items[position];
				if (parent.SelectionMode == SelectionMode.Single)
				{
					parent.SelectedItem = item;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ItemView.Click -= OnItemClick;
			}
			base.Dispose(disposing);
		}
	}

	class SpaceItemDecoration : RecyclerView.ItemDecoration
	{
		readonly int _space;

		public SpaceItemDecoration(int space)
		{
			_space = space;
		}

		public override void GetItemOffsets(Android.Graphics.Rect outRect, Android.Views.View view, RecyclerView parent, RecyclerView.State state)
		{
			outRect.Bottom = _space;

			// Add top margin only for the first item
			if (parent.GetChildAdapterPosition(view) == 0)
			{
				outRect.Top = _space;
			}
		}
	}
}
