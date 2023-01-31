using System;

namespace Spice;

public partial class Label
{
	public static implicit operator UILabel(Label label) => label.NativeView;

	public Label() : base(() => new UILabel { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Label(CGRect rect) : base(() => new UILabel(rect) { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Label(Func<UIView> creator) : base(creator) { }

	public new UILabel NativeView => (UILabel)_nativeView.Value;

	partial void OnTextChanged(string value)
	{
		if (_nativeView.Value is UILabel label)
		{
			label.Text = value;
		}
		else if (_nativeView.Value is UIButton button)
		{
			button.SetTitle(value, UIControlState.Normal);
		}
	}

	partial void OnTextColorChanged(Color? value)
	{
		if (_nativeView.Value is UILabel label)
		{
			label.TextColor = value.ToUIColor();
		}
		else if (_nativeView.Value is UIButton button)
		{
			button.SetTitleColor(value.ToUIColor(), UIControlState.Normal);
		}
	}
}