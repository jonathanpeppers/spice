using Android.Content;

namespace Spice;

public partial class Image
{
	public static implicit operator ImageView(Image image) => image.NativeView;

	static ImageView Create(Context context) => new(context);

	public Image() : this(Platform.Context, Create) { }

	public Image(Context context) : this(context, Create) { }

	protected Image(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	protected Image(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	public new ImageView NativeView => (ImageView)_nativeView.Value;

	partial void OnSourceChanged(string value) => Interop.SetImage(NativeView, value);
}