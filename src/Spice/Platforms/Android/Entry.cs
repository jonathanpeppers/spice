using Android.Content;
using Android.Text;

namespace Spice;

public partial class Entry
{
	/// <summary>
	/// Returns entry.NativeView
	/// </summary>
	/// <param name="entry">The Spice.Entry</param>
	public static implicit operator EditText(Entry entry) => entry.NativeView;

	static EditText Create(Context context) => new(context);

	/// <summary>
	/// Control for text input, like a form field.
	/// Android -> Android.Widget.EditText
	/// iOS -> UIKit.UITextField
	/// </summary>
	public Entry() : base(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Entry(Context context) : base(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Entry(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Entry(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.EditText
	/// </summary>
	public new EditText NativeView => (EditText)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value)
	{
		if (value != null)
		{
			int color = value.ToAndroidInt();
			NativeView.SetTextColor(Interop.GetEditTextColorStateList(color, color));
		}
	}

	partial void OnIsPasswordChanged(bool value) =>
		NativeView.InputType = value ?
			InputTypes.ClassText | InputTypes.TextVariationPassword :
			InputTypes.ClassText;
}