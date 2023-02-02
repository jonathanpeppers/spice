using Android.App;
using Android.OS;

namespace HeadToHeadSpice;

[Activity(
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