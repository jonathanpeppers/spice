namespace Spice;

public partial class Image
{
	/// <summary>
	/// Returns image.NativeView
	/// </summary>
	/// <param name="image">The Spice.Image</param>
	public static implicit operator UIImageView(Image image) => image.NativeView;

	/// <summary>
	/// Image ctor
	/// </summary>
	public Image() : base(_ => new UIImageView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <summary>
	/// Image ctor
	/// </summary>
	/// <param name="frame">Pass the underlying view a frame</param>
	public Image(CGRect frame) : base(_ => new UIImageView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <summary>
	/// Image ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Image(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UIImageView
	/// </summary>
	public new UIImageView NativeView => (UIImageView)_nativeView.Value;

	partial void OnSourceChanged(string value) => NativeView.Image = UIImage.FromFile($"{value}.png");
}