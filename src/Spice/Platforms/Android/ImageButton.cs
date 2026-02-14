using Android.Content;

namespace Spice;

public partial class ImageButton
{
	/// <summary>
	/// Returns imageButton.NativeView
	/// </summary>
	/// <param name="imageButton">The Spice.ImageButton</param>
	public static implicit operator Android.Widget.ImageButton(ImageButton imageButton) => imageButton.NativeView;

	static Android.Widget.ImageButton Create(Context context) => new(context);

	/// <summary>
	/// A button that displays an image instead of text. Use the Clicked event.
	/// Android -> Android.Widget.ImageButton
	/// iOS -> UIKit.UIButton
	/// </summary>
	public ImageButton() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public ImageButton(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ImageButton(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ImageButton(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.ImageButton
	/// </summary>
	public new Android.Widget.ImageButton NativeView => (Android.Widget.ImageButton)_nativeView.Value;

	partial void OnSourceChanged(string value)
	{
		// Clear any existing image so invalid/empty sources don't leave a stale image
		NativeView.SetImageDrawable(null);

		Interop.SetImage(NativeView, value);
	}

	EventHandler? _click;

	partial void OnClickedChanged(Action<ImageButton>? value)
	{
		// Always detach any existing handler to avoid multiple subscriptions
		if (_click != null)
		{
			NativeView.Click -= _click;
			_click = null;
		}

		// Attach a new handler only when a non-null delegate is provided
		if (value != null)
		{
			_click = (sender, e) => value(this);
			NativeView.Click += _click;
		}
	}
}
