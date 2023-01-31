using Android.Content;

namespace Spice;

public partial class Button
{
	public static implicit operator Android.Widget.Button(Button button) => button.NativeView;

	public Button() : base(Platform.Context!, c => new Android.Widget.Button(c)) { }

	public Button(Context context) : base(context, c => new Android.Widget.Button(c)) { }

	public Button(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	public new Android.Widget.Button NativeView => (Android.Widget.Button)_nativeView.Value;

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

	EventHandler? _click;

	partial void OnClickedChanged(Action<Button>? value)
	{
		if (value == null)
		{
			if (_click != null)
			{
				NativeView.Click -= _click;
				_click = null;
			}
		}
		else
		{
			NativeView.Click += _click = (sender, e) => Clicked?.Invoke(this);
		}
	}
}