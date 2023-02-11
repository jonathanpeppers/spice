using System.Collections.Specialized;

namespace Spice;

public partial class CollectionView<T>
{
	static UICollectionViewLayout GetLayout() => new UICollectionViewFlowLayout();

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
			collectionView.RegisterClassForCell(typeof(UICollectionViewCell), CellId);
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => SpiceView.Items?.Count() ?? 0;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(CellId, indexPath) as UICollectionViewCell;
			if (cell == null)
			{
				cell = new UICollectionViewCell();
			}
			else
			{
				//TODO: cell was recycled, how to recycle spice views?
			}

			var items = SpiceView.Items;
			if (items == null)
				return cell;

			var itemTemplate = SpiceView.ItemTemplate;
			if (itemTemplate != null)
			{
				var view = itemTemplate(items.Skip(indexPath.Row).First());
				view.NativeView.Frame = new CGRect(CGPoint.Empty, cell.Frame.Size);
				cell.AddSubview(view);
			}

			return cell;
		}
	}
}

