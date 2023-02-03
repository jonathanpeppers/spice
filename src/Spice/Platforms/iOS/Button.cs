namespace Spice;

public partial class Button
{
	/// <summary>
	/// Returns button.NativeView
	/// </summary>
	/// <param name="button">The Spice.Button</param>
	public static implicit operator UIButton(Button button) => button.NativeView;

	/// <summary>
	/// You know the button! Use the Clicked event.
	/// Android -> Android.Widget.Button
	/// iOS -> UIKit.UIButton
	/// </summary>
	public Button() : base(_ => new UIButton { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Button(CGRect frame) : base(_ => new UIButton(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Button(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UIButton
	/// </summary>
	public new UIButton NativeView => (UIButton)_nativeView.Value;

	partial void OnTextChanged(string value)
	{
		NativeView.SetTitle(value, UIControlState.Normal);
		if (HorizontalAlign != Align.Stretch && VerticalAlign != Align.Stretch)
		{
			NativeView.SizeToFit();
		}
	}

	partial void OnTextColorChanged(Color? value) => NativeView.SetTitleColor(value.ToUIColor(), UIControlState.Normal);

	EventHandler? _click;

	partial void OnClickedChanged(Action<Button>? value)
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