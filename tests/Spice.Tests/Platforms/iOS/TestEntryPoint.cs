using Microsoft.DotNet.XHarness.TestRunners.Common;
using Microsoft.DotNet.XHarness.TestRunners.Xunit;
using ObjCRuntime;
using UIKit;

namespace Spice.Tests;

class TestEntryPoint : iOSApplicationEntryPoint
{
	protected override bool LogExcludedTests => true;

	protected override int? MaxParallelThreads => 1;

	protected override IDevice Device { get; } = new TestDevice();

	protected override IEnumerable<TestAssemblyInfo> GetTestAssemblies()
	{
		var asm = typeof(TestEntryPoint).Assembly;
		var path = Path.Combine(AppContext.BaseDirectory, asm.GetName().Name + ".dll");
		return [new TestAssemblyInfo(asm, path)];
	}

	protected override void TerminateWithSuccess()
	{
		UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
		{
			var selector = new Selector("terminateWithSuccess");
			UIApplication.SharedApplication.PerformSelector(
				selector, UIApplication.SharedApplication, 0);
		});
	}
}
