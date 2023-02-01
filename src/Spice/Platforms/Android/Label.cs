﻿using Android.Content;

namespace Spice;

public partial class Label
{
	public static implicit operator TextView(Label label) => label.NativeView;

	static TextView Create(Context context) => new(context);

	public Label() : base(Platform.Context, Create) { }

	public Label(Context context) : base(context, Create) { }

	protected Label(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	protected Label(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	public new TextView NativeView => (TextView)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value)
	{
		if (value != null)
		{
			NativeView.SetTextColor(Interop.GetDefaultColorStateList(value.ToAndroidInt()));
		}
		else
		{
			NativeView.SetTextColor(null);
		}
	}
}