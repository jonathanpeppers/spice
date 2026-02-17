using Foundation;

namespace Hello;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override Application CreateApplication() => new App();
}