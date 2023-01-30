using AndroidX.AppCompat.App;

namespace Spice;

public class SpiceActivity : AppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		Platform.Context = this;

		base.OnCreate(savedInstanceState);
	}
}