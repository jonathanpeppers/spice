using Foundation;

namespace HelloBlazor;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override Application CreateApplication() => new App();
}