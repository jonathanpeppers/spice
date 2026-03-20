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
	/// Represents text on screen. Set the Text property to a string value.
	/// Android -> Android.Widget.TextView
	/// iOS -> UIKit.UILabel
	/// </summary>
	public Label() : base(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Label(Context context) : base(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Label(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
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
			NativeView.SetTextColor(Interop.GetDefaultColorStateList(value.ToAndroidInt()));
	}
}