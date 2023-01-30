using Android.Content;

namespace Spice;

public partial class Application
{
	public Application() { }

	public Application(Context context) : base(context) { }

#pragma warning disable MVVMTK0034
	partial void OnMainChanging(View? value)
	{
		if (_main != null)
		{
			NativeView.RemoveView(_main);
		}
	}
#pragma warning restore MVVMTK0034

	partial void OnMainChanged(View? value)
	{
		if (value != null)
		{
			NativeView.AddView(value);
		}
	}
}