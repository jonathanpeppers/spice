namespace Spice;

public partial class Label
{
	/// <summary>
	/// Returns label.NativeView
	/// </summary>
	/// <param name="label">The Spice.Label</param>
	public static implicit operator UILabel(Label label) => label.NativeView;

	/// <summary>
	/// Label ctor
	/// </summary>
	public Label() : base(_ => new UILabel { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <summary>
	/// Label ctor
	/// </summary>
	/// <param name="frame">Pass the underlying view a frame</param>
	public Label(CGRect frame) : base(_ => new UILabel(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <summary>
	/// Label ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Label(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UILabel
	/// </summary>
	public new UILabel NativeView => (UILabel)_nativeView.Value;

	partial void OnTextChanged(string value)
	{
		NativeView.Text = value;
		if (HorizontalAlign != Align.Stretch && VerticalAlign != Align.Stretch)
		{
			NativeView.SizeToFit();
		}
	}

	partial void OnTextColorChanged(Color? value) => NativeView.TextColor = value.ToUIColor();
}