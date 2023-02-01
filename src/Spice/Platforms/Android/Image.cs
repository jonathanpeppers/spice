using Android.Content;

namespace Spice;

public partial class Image
{
	public static implicit operator ImageView(Image image) => image.NativeView;

	public Image() : this(Platform.Context!, c => new ImageView(c)) { }

	public Image(Context context) : this(context, c => new ImageView(c)) { }

	public Image(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	public new ImageView NativeView => (ImageView)_nativeView.Value;

	partial void OnSourceChanged(string value) => Interop.SetImage(NativeView, value);
}