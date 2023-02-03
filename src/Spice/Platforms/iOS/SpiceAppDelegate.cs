namespace Spice;

/// <summary>
/// The UIApplicationDelegate to use in Spice applications
/// </summary>
public class SpiceAppDelegate : UIApplicationDelegate
{
	/// <inheritdoc />
	public override UIWindow? Window
	{
		get;
		set;
	}

	/// <inheritdoc />
	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		Window = Platform.Window = new UIWindow(UIScreen.MainScreen.Bounds);
		return true;
	}
}
