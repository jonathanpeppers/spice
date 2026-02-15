using Android.App;
using Android.OS;

namespace Spice.Scenarios;

[Activity(
	// TODO: fix splash theme
	// Theme = "@style/Maui.SplashTheme", 
	Theme = "@style/Theme.MaterialComponents.Light.NoActionBar",
	MainLauncher = true)]
[Android.Runtime.Register("com.companyname.spice.scenarios.MainActivity")]
public class MainActivity : SpiceActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		SetContentView(new App());
	}
}