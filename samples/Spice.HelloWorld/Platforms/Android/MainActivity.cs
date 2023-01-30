using Android.Content.PM;

namespace Spice.HelloWorld;

[Activity(
	// TODO: fix splash theme
	// Theme = "@style/Maui.SplashTheme", 
	MainLauncher = true,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : SpiceActivity
{
	protected override void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		SetContentView(new App());
	}
}