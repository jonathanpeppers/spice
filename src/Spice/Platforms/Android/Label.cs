using Android.Content;

namespace Spice;

public partial class Label
{
	public static implicit operator TextView(Label label) => label.NativeView;

	public Label() : base(Platform.Context!, c => new TextView(c)) { }

	public Label(Context context) : base(context, c => new TextView(c)) { }

	public Label(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	public new TextView NativeView => (TextView)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value)
	{
		if (value != null)
		{
			NativeView.SetTextColor(Interop.GetDefaultColorStateList(value.ToAndroidColor()));
		}
		else
		{
			NativeView.SetTextColor(null);
		}
	}
}