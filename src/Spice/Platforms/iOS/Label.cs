namespace Spice;

public partial class Label
{
	public static implicit operator UILabel(Label label) => label.NativeView;

	public Label() : base(() => new UILabel { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Label(CGRect rect) : base(() => new UILabel(rect) { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Label(Func<UIView> creator) : base(creator) { }

	public new UILabel NativeView => (UILabel)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value) => NativeView.TextColor = value.ToUIColor();
}