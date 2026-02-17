using Foundation;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using UIKit;

namespace Spice.Tests;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
	{
#pragma warning disable CA1422 // Validate platform compatibility (xharness simulators)
		Window = new UIWindow(UIScreen.MainScreen.Bounds)
		{
			RootViewController = new UIViewController()
		};
#pragma warning restore CA1422
		Window.MakeKeyAndVisible();

		// Initialize Spice platform so controls can create native views
		Spice.Platform.Window = Window;

		Task.Run(async () =>
		{
			try
			{
				var runner = new TestEntryPoint();

				runner.TestsCompleted += (_, results) =>
				{
					Console.WriteLine(
						$"Tests run: {results.ExecutedTests} " +
						$"Passed: {results.PassedTests} " +
						$"Failed: {results.FailedTests} " +
						$"Skipped: {results.SkippedTests}");
				};

				await runner.RunAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error while running tests: " + ex);
			}
		});

		return true;
	}
}
