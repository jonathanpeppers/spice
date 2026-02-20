using Android.App;
using Android.OS;

namespace Hello;

[Activity(
	Theme = "@style/Theme.Material3.Light.NoActionBar",
	MainLauncher = true)]
public class MainActivity : SpiceActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		SetContentView(new App());
	}
}