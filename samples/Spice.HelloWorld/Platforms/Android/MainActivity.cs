using Android.Content.PM;

namespace Spice.HelloWorld;

[Activity(
	// TODO: fix splash theme
	// Theme = "@style/Maui.SplashTheme", 
	Theme = "@style/Theme.MaterialComponents.Light.NoActionBar",
	MainLauncher = true)]
public class MainActivity : SpiceActivity
{
	protected override void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		SetContentView(new App());
	}
}