namespace Spice;

/// <summary>
/// The UIApplicationDelegate to use in Spice applications
/// </summary>
public class SpiceAppDelegate : UIApplicationDelegate
{
	/// <summary>
	/// The UIWindow for the application
	/// </summary>
	public override UIWindow? Window
	{
		get;
		set;
	}

	/// <summary>
	/// Main entry point for iOS
	/// </summary>
	/// <param name="application">UIApplication</param>
	/// <param name="launchOptions">NSDictionary</param>
	/// <returns>true</returns>
	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		Window = Platform.Window = new UIWindow(UIScreen.MainScreen.Bounds);
		return true;
	}
}
