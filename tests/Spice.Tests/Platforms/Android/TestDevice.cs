using System.Globalization;
using Microsoft.DotNet.XHarness.TestRunners.Common;

namespace Spice.Tests;

class TestDevice : IDevice
{
	private readonly string _uniqueIdentifier = Guid.NewGuid().ToString("N");
	public string BundleIdentifier => "com.jonathanpeppers.spice.tests";
	public string UniqueIdentifier => _uniqueIdentifier;
	public string Name => Android.OS.Build.Model ?? "Unknown";
	public string Model => Android.OS.Build.Device ?? "Unknown";
	public string SystemName => "Android";
	public string SystemVersion => Android.OS.Build.VERSION.Release ?? "Unknown";
	public string Locale => CultureInfo.CurrentCulture.Name;
}
