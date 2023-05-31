namespace Spice;

internal class SpiceServiceProvider : IServiceProvider
{
	public static readonly SpiceServiceProvider Instance = new();

	readonly Dictionary<Type, object> _services = new();

	public object? GetService(Type serviceType)
	{
		_services.TryGetValue(serviceType, out var value);
		return value;
	}
}
