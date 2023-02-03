using AndroidX.AppCompat.App;

namespace Spice;

/// <summary>
/// The SpiceActivity to use in Android applications.
/// OnCreate() sets Platform.Context = this.
/// </summary>
public class SpiceActivity : AppCompatActivity
{
	/// <inheritdoc cref="F:AndroidX.AppCompat.App.AppCompatActivity.OnCreate" />
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		Platform.Context = this;

		base.OnCreate(savedInstanceState);
	}
}