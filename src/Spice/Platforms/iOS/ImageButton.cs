namespace Spice;

public partial class ImageButton
{
	/// <summary>
	/// Returns imageButton.NativeView
	/// </summary>
	/// <param name="imageButton">The Spice.ImageButton</param>
	public static implicit operator UIButton(ImageButton imageButton) => imageButton.NativeView;

	/// <summary>
	/// A button that displays an image instead of text. Use the Clicked event.
	/// Android -> Android.Widget.ImageButton
	/// iOS -> UIKit.UIButton
	/// </summary>
	public ImageButton() : base(_ => new UIButton { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ImageButton(CGRect frame) : base(_ => new UIButton(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ImageButton(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UIButton
	/// </summary>
	public new UIButton NativeView => (UIButton)_nativeView.Value;

	partial void OnSourceChanged(string value)
	{
		var image = UIImage.FromFile($"{value}.png");
		if (image != null)
		{
			NativeView.SetImage(image, UIControlState.Normal);
			if (HorizontalAlign != Align.Stretch && VerticalAlign != Align.Stretch)
			{
				NativeView.SizeToFit();
			}
		}
	}

	EventHandler? _click;

	partial void OnClickedChanged(Action<ImageButton>? value)
	{
		if (value == null)
		{
			if (_click != null)
			{
				NativeView.TouchUpInside -= _click;
				_click = null;
			}
		}
		else
		{
			NativeView.TouchUpInside += _click = (sender, e) => Clicked?.Invoke(this);
		}
	}
}
