using Android.App;
using Android.OS;
using Android.Runtime;

namespace Spice.Tests;

[Instrumentation(Name = "com.jonathanpeppers.spice.tests.TestInstrumentation")]
public class TestInstrumentation : Instrumentation
{
	protected TestInstrumentation(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership) { }

	public override void OnCreate(Bundle? arguments)
	{
		base.OnCreate(arguments);
		Start();
	}

	public override void OnStart()
	{
		base.OnStart();

		// Initialize Spice platform so controls can create native views
		Spice.Platform.Context = Android.App.Application.Context;

		// Prepare a Looper on this thread so Android views (WebView,
		// SwipeRefreshLayout, etc.) that require a Handler can be created.
		if (Looper.MyLooper() == null)
			Looper.Prepare();

		Task.Run(async () =>
		{
			// Also prepare a Looper on the Task.Run thread
			if (Looper.MyLooper() == null)
				Looper.Prepare();

			var bundle = new Bundle();
			try
			{
				var entryPoint = new TestEntryPoint(
					Android.App.Application.Context.CacheDir!.AbsolutePath);
				entryPoint.TestsCompleted += (_, results) =>
				{
					bundle.PutLong("return-code", results.FailedTests == 0 ? 0 : 1);
					bundle.PutString("test-execution-summary",
						$"Tests run: {results.ExecutedTests} " +
						$"Passed: {results.PassedTests} " +
						$"Failed: {results.FailedTests} " +
						$"Skipped: {results.SkippedTests}");
				};

				await entryPoint.RunAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error while running tests: " + ex);
			}

			if (bundle.GetLong("return-code", -1) == -1)
				bundle.PutLong("return-code", 1);

			Finish(Result.Ok, bundle);
		});
	}
}
