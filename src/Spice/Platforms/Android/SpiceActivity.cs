using Android.Content.Res;
using AndroidX.AppCompat.App;

namespace Spice;

/// <summary>
/// The SpiceActivity to use in Android applications.
/// OnCreate() sets Platform.Context = this.
/// </summary>
public class SpiceActivity : AppCompatActivity
{
	/// <summary>
	/// The app view that was set using SetContentView
	/// </summary>
	private View? _appView;

	/// <inheritdoc />
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		Platform.Context = this;

		base.OnCreate(savedInstanceState);
	}

	/// <summary>
	/// Sets the content view to a Spice View and stores a reference for disposal.
	/// This method accepts a <see cref="Spice.View"/> rather than <see cref="Android.Views.View"/>,
	/// providing a convenient way to set Spice views as content while ensuring proper lifecycle management.
	/// The view will be automatically disposed when the activity is destroyed.
	/// </summary>
	/// <param name="view">The Spice view to set as content</param>
	public void SetContentView(View view)
	{
		_appView = view;
		base.SetContentView((Android.Views.View)view);
	}

	/// <inheritdoc />
	public override void OnConfigurationChanged(Configuration newConfig)
	{
		base.OnConfigurationChanged(newConfig);

		var nightMode = newConfig.UiMode & UiMode.NightMask;
		PlatformAppearance.OnChanged(nightMode == UiMode.NightYes);
	}

	/// <inheritdoc />
	protected override void OnDestroy()
	{
		// Dispose the app view and all its children
		if (_appView != null)
		{
			View.DisposeRecursive(_appView);
			_appView = null;
		}

		base.OnDestroy();
	}
}