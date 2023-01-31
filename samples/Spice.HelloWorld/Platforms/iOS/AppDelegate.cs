using Foundation;

namespace Spice.HelloWorld;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		base.FinishedLaunching(application, launchOptions);

		ArgumentNullException.ThrowIfNull(Window);

		var app = new App();
		app.NativeView.Frame = Window.Frame;

		var vc = new UIViewController();
		vc.View!.AddSubview(app);
		Window.RootViewController = vc;
		Window.MakeKeyAndVisible();

		return true;
	}
}