using Microsoft.Extensions.DependencyInjection;

namespace Spice;

/// <summary>
/// Implements IServiceProvider to make ASP.NET able to launch
/// </summary>
internal class SpiceServiceProvider
{
	public static readonly IServiceProvider Instance;

	static SpiceServiceProvider()
	{
		var services = new ServiceCollection();
		services.AddBlazorWebView();
		Instance = services.BuildServiceProvider();
	}
}
