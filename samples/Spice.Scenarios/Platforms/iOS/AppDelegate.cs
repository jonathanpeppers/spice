using Foundation;

namespace Spice.Scenarios;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override Application CreateApplication() => new App();
}