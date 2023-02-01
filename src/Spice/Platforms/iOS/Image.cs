namespace Spice;

public partial class Image
{
	public static implicit operator UIImageView(Image image) => image.NativeView;

	public Image() : base(_ => new UIImageView { AutoresizingMask = UIViewAutoresizing.None }) { }

	public Image(CGRect rect) : base(_ => new UIImageView(rect) { AutoresizingMask = UIViewAutoresizing.None }) { }

	public Image(Func<View, UIView> creator) : base(creator) { }

	public new UIImageView NativeView => (UIImageView)_nativeView.Value;

	partial void OnSourceChanged(string value) => NativeView.Image = UIImage.FromFile($"{value}.png");
}