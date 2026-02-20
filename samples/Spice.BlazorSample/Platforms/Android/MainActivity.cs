using Android.App;
using Android.OS;

namespace Spice.BlazorSample;

[Activity(
	// TODO: fix splash theme
	// Theme = "@style/Maui.SplashTheme", 
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