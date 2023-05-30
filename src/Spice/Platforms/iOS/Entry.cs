namespace Spice;

public partial class Entry
{
	/// <summary>
	/// Returns entry.NativeView
	/// </summary>
	/// <param name="entry">The Spice.Entry</param>
	public static implicit operator UITextField(Entry entry) => entry.NativeView;

	/// <summary>
	/// Represents text on screen. Set the Text property to a string value.
	/// Android -> Android.Widget.EditText
	/// iOS -> UIKit.UITextField
	/// </summary>
	public Entry() : base(_ => new UITextField { AutoresizingMask = UIViewAutoresizing.None, BorderStyle = UITextBorderStyle.RoundedRect }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Entry(CGRect frame) : base(_ => new UITextField(frame) { AutoresizingMask = UIViewAutoresizing.None, BorderStyle = UITextBorderStyle.RoundedRect }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Entry(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UITextField
	/// </summary>
	public new UITextField NativeView => (UITextField)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value) => NativeView.TextColor = value.ToUIColor();

	partial void OnIsPasswordChanged(bool value) => NativeView.SecureTextEntry = value;
}
