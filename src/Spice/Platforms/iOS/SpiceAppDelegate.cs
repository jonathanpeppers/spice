using Foundation;

namespace Spice;

public class SpiceAppDelegate : UIApplicationDelegate
{
	public override UIWindow? Window
	{
		get;
		set;
	}

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		Window = new UIWindow(UIScreen.MainScreen.Bounds);
		return true;
	}
}
