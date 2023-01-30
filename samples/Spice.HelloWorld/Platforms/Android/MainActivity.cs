using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Spice.HelloWorld;

[Activity(
	// TODO: fix splash theme
	// Theme = "@style/Maui.SplashTheme", 
	MainLauncher = true,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : SpiceActivity
{
}