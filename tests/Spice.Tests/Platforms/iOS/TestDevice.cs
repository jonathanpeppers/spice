using System.Globalization;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using UIKit;

namespace Spice.Tests;

class TestDevice : IDevice
{
	public string BundleIdentifier => Foundation.NSBundle.MainBundle.BundleIdentifier ?? "";
	public string UniqueIdentifier => Guid.NewGuid().ToString("N");
	public string Name => UIDevice.CurrentDevice.Name;
	public string Model => UIDevice.CurrentDevice.Model;
	public string SystemName => UIDevice.CurrentDevice.SystemName;
	public string SystemVersion => UIDevice.CurrentDevice.SystemVersion;
	public string Locale => CultureInfo.CurrentCulture.Name;
}
