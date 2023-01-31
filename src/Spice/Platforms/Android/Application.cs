using Android.Content;

namespace Spice;

public partial class Application
{
	public Application() { }

	public Application(Context context) : base(context) { }

	protected override Android.Views.ViewGroup.LayoutParams CreateLayoutParameters() =>
		new RelativeLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

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