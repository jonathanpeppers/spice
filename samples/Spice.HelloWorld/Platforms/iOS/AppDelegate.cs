using Foundation;

namespace Spice.HelloWorld;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		base.FinishedLaunching(application, launchOptions);

		ArgumentNullException.ThrowIfNull(Window);

		var vc = new UIViewController();
		vc.View!.AddSubview(new App());
		Window.RootViewController = vc;
		Window.MakeKeyAndVisible();

		// Uncomment to debug UI layout
		//vc.View.DumpHierarchy();

		return true;
	}
}