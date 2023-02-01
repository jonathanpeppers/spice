namespace Spice;

public partial class Label
{
	public static implicit operator UILabel(Label label) => label.NativeView;

	public Label() : base(_ => new UILabel { AutoresizingMask = UIViewAutoresizing.FlexibleDimensions }) { }

	public Label(CGRect rect) : base(_ => new UILabel(rect) { AutoresizingMask = UIViewAutoresizing.FlexibleDimensions }) { }

	public Label(Func<View, UIView> creator) : base(creator) { }

	public new UILabel NativeView => (UILabel)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value) => NativeView.TextColor = value.ToUIColor();
}