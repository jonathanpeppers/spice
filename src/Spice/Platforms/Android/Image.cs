using Android.Content;

namespace Spice;

public partial class Image
{
	/// <summary>
	/// Returns image.NativeView
	/// </summary>
	/// <param name="image">The Spice.Image</param>
	public static implicit operator ImageView(Image image) => image.NativeView;

	static ImageView Create(Context context) => new(context);

	/// <summary>
	/// Image ctor
	/// </summary>
	public Image() : this(Platform.Context, Create) { }

	/// <summary>
	/// Image ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Image(Context context) : this(context, Create) { }

	/// <summary>
	/// Image ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Image(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <summary>
	/// Image ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Image(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.ImageView
	/// </summary>
	public new ImageView NativeView => (ImageView)_nativeView.Value;

	partial void OnSourceChanged(string value) => Interop.SetImage(NativeView, value);
}