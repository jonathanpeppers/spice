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

	public override async void OnStart()
	{
		base.OnStart();

		var bundle = new Bundle();
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

		if (File.Exists(entryPoint.TestsResultsFinalPath))
		{
			var externalDir = Android.App.Application.Context.GetExternalFilesDir(null)!.AbsolutePath;
			var dest = Path.Combine(externalDir, "TestResults.xml");
			File.Copy(entryPoint.TestsResultsFinalPath, dest, overwrite: true);
			bundle.PutString("test-results-path", dest);
		}

		if (bundle.GetLong("return-code", -1) == -1)
			bundle.PutLong("return-code", 1);

		Finish(Result.Ok, bundle);
	}
}
