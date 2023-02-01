namespace Spice;

public partial class Button
{
	public static implicit operator UIButton(Button button) => button.NativeView;

	public Button() : base(_ => new UIButton { AutoresizingMask = UIViewAutoresizing.FlexibleDimensions }) { }

	public Button(CGRect frame) : base(_ => new UIButton(frame) { AutoresizingMask = UIViewAutoresizing.FlexibleDimensions }) { }

	public Button(Func<View, UIView> creator) : base(creator) { }

	public new UIButton NativeView => (UIButton)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.SetTitle(value, UIControlState.Normal);

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