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
		UIImage? image = null;

		if (!string.IsNullOrEmpty(value))
		{
			image = UIImage.FromFile($"{value}.png");
		}

		// Always update the button image so Source changes (including invalid/empty) are reflected.
		NativeView.SetImage(image, UIControlState.Normal);

		if (image != null && HorizontalOptions.Alignment != LayoutAlignment.Fill && VerticalOptions.Alignment != LayoutAlignment.Fill)
		{
			NativeView.SizeToFit();
		}
	}

	EventHandler? _click;

	partial void OnClickedChanged(Action<ImageButton>? value)
	{
		// Always detach any existing handler to avoid multiple subscriptions
		if (_click != null)
		{
			NativeView.TouchUpInside -= _click;
			_click = null;
		}

		if (value != null)
		{
			_click = (sender, e) => Clicked?.Invoke(this);
			NativeView.TouchUpInside += _click;
		}
	}
}
