using System.Reflection;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using Microsoft.DotNet.XHarness.TestRunners.Xunit;
using ObjCRuntime;
using UIKit;

namespace Spice.Tests;

class TestEntryPoint : iOSApplicationEntryPoint
{
	protected override bool LogExcludedTests => true;

	protected override int? MaxParallelThreads => Environment.ProcessorCount;

	protected override IDevice Device { get; } = new TestDevice();

	protected override IEnumerable<TestAssemblyInfo> GetTestAssemblies()
	{
		var asm = typeof(TestEntryPoint).Assembly;
		return [new TestAssemblyInfo(asm, asm.Location)];
	}

	protected override void TerminateWithSuccess()
	{
		var selector = new Selector("terminateWithSuccess");
		UIApplication.SharedApplication.PerformSelector(
			selector, UIApplication.SharedApplication, 0);
	}
}
