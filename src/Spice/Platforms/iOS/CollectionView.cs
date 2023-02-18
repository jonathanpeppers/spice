using System.Collections.Specialized;
using ObjCRuntime;

namespace Spice;

public partial class CollectionView<T>
{
	static UICollectionViewLayout GetLayout() => new UICollectionViewFlowLayout
	{
		ItemSize = new CGSize(Platform.Window!.Frame.Width, 200)
	};

	/// <summary>
	/// Returns collectionView.NativeView
	/// </summary>
	/// <param name="collectionView">The Spice.CollectionView</param>
	public static implicit operator UICollectionView(CollectionView<T> collectionView) => collectionView.NativeView;

	/// <summary>
	/// Represents an image on screen. Set the Source property such as "spice".
	/// Android -> ???
	/// iOS -> UIKit.UICollectionView
	/// </summary>
	public CollectionView() : base(v => new UICollectionView(Platform.Window!.Frame, GetLayout())
		{
			AutoresizingMask = UIViewAutoresizing.None,
			DataSource = new Source((CollectionView<T>)v)
		})
	{ }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public CollectionView(CGRect frame) : base(v => new UICollectionView(frame, GetLayout())
		{
			AutoresizingMask = UIViewAutoresizing.None,
			DataSource = new Source((CollectionView<T>)v)
		})
	{ }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected CollectionView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UIImageView
	/// </summary>
	public new UICollectionView NativeView => (UICollectionView)_nativeView.Value;

	void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => NativeView.CollectionViewLayout.InvalidateLayout();

	partial void OnItemTemplateChanged(Func<T, View>? value) => NativeView.CollectionViewLayout.InvalidateLayout();

	class Source : UICollectionViewDataSource
	{
		static readonly NSString CellId = (NSString)"__SpiceCollectionViewCell__";
		readonly CollectionView<T> SpiceView;

		public Source(CollectionView<T> spiceView)
		{
			SpiceView = spiceView;
		}

		//TODO: implement multiple sections
		public override nint NumberOfSections(UICollectionView collectionView)
		{
			//TODO: do this somewhere else
			collectionView.RegisterClassForCell(typeof(SpiceCell), CellId);
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => SpiceView.Items?.Count() ?? 0;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var items = SpiceView.Items;
			ArgumentNullException.ThrowIfNull(items);

			var item = items.Skip(indexPath.Row).First();

			var cell = (SpiceCell)collectionView.DequeueReusableCell(CellId, indexPath);
			ArgumentNullException.ThrowIfNull(cell);

			if (cell.View == null)
			{
				var itemTemplate = SpiceView.ItemTemplate;
				if (itemTemplate != null)
				{
					cell.AddSubview(cell.View = itemTemplate(item));
				}
			}
			else
			{
				SpiceView.Recycled?.Invoke(cell.View, item);
				
			}

			cell.SetNeedsLayout();
			return cell;
		}
	}
}

class SpiceCell : UICollectionViewCell
{
	public SpiceCell() { }

	public SpiceCell(NativeHandle handle) : base(handle) { }

	public View? View { get; set; }

	public override void LayoutSubviews()
	{
		base.LayoutSubviews();

		if (View != null)
		{
			var size = Frame.Size;
			View.NativeView.Frame = new CGRect(0, 0, size.Width, size.Height);
		}
	}
}

