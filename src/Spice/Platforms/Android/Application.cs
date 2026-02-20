using Android.Content;

namespace Spice;

public partial class Application
{
	/// <summary>
	/// The root "view" of a Spice application. Set Main to a single view.
	/// </summary>
	public Application() { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Application(Context context) : base(context) { }

	/// <inheritdoc />
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
