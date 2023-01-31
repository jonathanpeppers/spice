namespace Spice;

public partial class Label
{
	public static implicit operator UILabel(Label label) => label.NativeView;

	public Label() : base(() => new UILabel { TranslatesAutoresizingMaskIntoConstraints = false }) { }

	public Label(CGRect rect) : base(() => new UILabel(rect) { TranslatesAutoresizingMaskIntoConstraints = false }) { }

	public Label(Func<UIView> creator) : base(creator) { }

	public new UILabel NativeView => (UILabel)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value) => NativeView.TextColor = value.ToUIColor();
}