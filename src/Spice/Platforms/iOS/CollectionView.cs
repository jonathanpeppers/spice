using System.Collections.Specialized;

namespace Spice;

public partial class CollectionView<T>
{
	static UICollectionViewLayout GetLayout() => new UICollectionViewFlowLayout();

	/// <summary>
	/// Returns collectionView.NativeView
	/// </summary>
	/// <param name="image">The Spice.CollectionView</param>
	public static implicit operator UICollectionView(CollectionView<T> collectionView) => collectionView.NativeView;

	/// <summary>
	/// Represents an image on screen. Set the Source property such as "spice".
	/// Android -> ???
	/// iOS -> UIKit.UICollectionView
	/// </summary>
	public CollectionView() : base(_ => new UICollectionView(CGRect.Empty, GetLayout()) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public CollectionView(CGRect frame) : base(_ => new UICollectionView(frame, GetLayout()) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected CollectionView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UIImageView
	/// </summary>
	public new UICollectionView NativeView => (UICollectionView)_nativeView.Value;

	void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => NativeView.CollectionViewLayout.InvalidateLayout();

	partial void OnItemTemplateChanged(Func<T, View>? value) => NativeView.CollectionViewLayout.InvalidateLayout();
}

