using Android.Content;

namespace Spice;

public partial class Application
{
	public Application() { }

	public Application(Context context) : base(context) { }

	partial void OnMainChanging(View? value)
	{
		if (_main != null)
		{
			NativeView.RemoveView(_main);
		}
	}

	partial void OnMainChanged(View? value)
	{
		if (value != null)
		{
			NativeView.AddView(value);
		}
	}
}