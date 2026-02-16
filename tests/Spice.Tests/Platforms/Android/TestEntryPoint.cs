using System.Reflection;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using Microsoft.DotNet.XHarness.TestRunners.Xunit;

namespace Spice.Tests;

class TestEntryPoint : AndroidApplicationEntryPoint
{
	readonly string _resultsPath;

	public TestEntryPoint(string cacheDir)
	{
		_resultsPath = Path.Combine(cacheDir, "TestResults.xml");
	}

	protected override bool LogExcludedTests => true;

	public override TextWriter? Logger => null;

	public override string TestsResultsFinalPath => _resultsPath;

	protected override int? MaxParallelThreads => Environment.ProcessorCount;

	protected override IDevice Device { get; } = new TestDevice();

	protected override IEnumerable<TestAssemblyInfo> GetTestAssemblies()
	{
		var asm = typeof(TestEntryPoint).Assembly;
		return [new TestAssemblyInfo(asm, asm.Location)];
	}

	protected override void TerminateWithSuccess() { }
}
