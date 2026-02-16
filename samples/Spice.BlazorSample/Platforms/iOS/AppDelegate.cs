using Foundation;

namespace Spice.BlazorSample;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override Application CreateApplication() => new App();
}