using System.Globalization;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using UIKit;

namespace Spice.Tests;

class TestDevice : IDevice
{
	private readonly string _uniqueIdentifier = Guid.NewGuid().ToString("N");

	public string BundleIdentifier => Foundation.NSBundle.MainBundle.BundleIdentifier ?? "";
	public string UniqueIdentifier => _uniqueIdentifier;
	public string Name => UIDevice.CurrentDevice.Name;
	public string Model => UIDevice.CurrentDevice.Model;
	public string SystemName => UIDevice.CurrentDevice.SystemName;
	public string SystemVersion => UIDevice.CurrentDevice.SystemVersion;
	public string Locale => CultureInfo.CurrentCulture.Name;
}
