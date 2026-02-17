using Foundation;

namespace HeadToHeadSpice;

[Register("AppDelegate")]
public class AppDelegate : SpiceAppDelegate
{
	public override Application CreateApplication() => new App();
}