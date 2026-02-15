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
	/// Sets the content view and stores a reference for disposal
	/// </summary>
	public void SetContentView(View view)
	{
		_appView = view;
		base.SetContentView((Android.Views.View)view);
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