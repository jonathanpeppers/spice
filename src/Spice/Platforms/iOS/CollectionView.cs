﻿namespace Spice;

public partial class CollectionView<T>
{
	/// <summary>
	/// Returns collectionView.NativeView
	/// </summary>
	/// <param name="image">The Spice.CollectionView</param>
	public static implicit operator UICollectionView(CollectionView collectionView) => collectionView.NativeView;

	/// <summary>
	/// Represents an image on screen. Set the Source property such as "spice".
	/// Android -> ???
	/// iOS -> UIKit.UICollectionView
	/// </summary>
	public CollectionView() : base(_ => new UICollectionView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public CollectionView(CGRect frame) : base(_ => new UICollectionView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected CollectionView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UIImageView
	/// </summary>
	public new UICollectionView NativeView => (UICollectionView)_nativeView.Value;
}
