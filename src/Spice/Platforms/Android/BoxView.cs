using Android.Content;

namespace Spice;

public partial class BoxView
{
	/// <summary>
	/// Returns boxView.NativeView
	/// </summary>
	/// <param name="boxView">The Spice.BoxView</param>
	public static implicit operator Android.Views.View(BoxView boxView) => boxView.NativeView;

	static Android.Views.View Create(Context context) => new(context);

	/// <summary>
	/// Represents a colored rectangle used for decoration, backgrounds, or dividing lines.
	/// Android -> Android.Views.View
	/// iOS -> UIKit.UIView
	/// </summary>
	public BoxView() : base(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public BoxView(Context context) : base(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected BoxView(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected BoxView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Views.View
	/// </summary>
	public new Android.Views.View NativeView => _nativeView.Value;

	partial void OnColorChanged(Color? value) => NativeView.SetBackgroundColor(value.ToAndroidColor());
}
