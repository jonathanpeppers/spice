using Android.Content;

namespace Spice;

public partial class Label
{
	/// <summary>
	/// Returns label.NativeView
	/// </summary>
	/// <param name="label">The Spice.Label</param>
	public static implicit operator TextView(Label label) => label.NativeView;

	static TextView Create(Context context) => new(context);

	/// <summary>
	/// Label ctor
	/// </summary>
	public Label() : base(Platform.Context, Create) { }

	/// <summary>
	/// Label ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Label(Context context) : base(context, Create) { }

	/// <summary>
	/// Label ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Label(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <summary>
	/// Label ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Label(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.TextView
	/// </summary>
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