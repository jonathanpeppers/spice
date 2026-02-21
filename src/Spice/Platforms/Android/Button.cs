using Android.Content;

namespace Spice;

public partial class Button
{
	/// <summary>
	/// Returns button.NativeView
	/// </summary>
	/// <param name="button">The Spice.Button</param>
	public static implicit operator Android.Widget.Button(Button button) => button.NativeView;

	static Android.Widget.Button Create(Context context) => new(context);

	/// <summary>
	/// You know the button! Use the Clicked event.
	/// Android -> Android.Widget.Button
	/// iOS -> UIKit.UIButton
	/// </summary>
	public Button() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Button(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Button(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Button(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.Button
	/// </summary>
	public new Android.Widget.Button NativeView => (Android.Widget.Button)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnTextColorChanged(Color? value)
	{
		if (value != null)
			NativeView.SetTextColor(Interop.GetDefaultColorStateList(value.ToAndroidInt()));
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