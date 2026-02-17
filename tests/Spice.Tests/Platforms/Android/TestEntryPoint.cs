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
		var path = Path.Combine(AppContext.BaseDirectory, asm.GetName().Name + ".dll");
		// On Android, assemblies are embedded as native .so files; create a
		// placeholder so TestAssemblyInfo's file-existence check passes.
		if (!File.Exists(path))
			File.Create(path).Dispose();
		return [new TestAssemblyInfo(asm, path)];
	}

	protected override void TerminateWithSuccess() { }
}
