namespace Spice;

public partial class Button
{
	public static implicit operator UIButton(Button button) => button.NativeView;

	public Button() : base(() => new UIButton { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Button(CGRect rect) : base(() => new UIButton(rect) { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Button(Func<UIView> creator) : base(creator) { }

	public new UIButton NativeView => (UIButton)_nativeView.Value;

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