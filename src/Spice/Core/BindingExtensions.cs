using System.ComponentModel;

namespace Spice;

/// <summary>
/// Extension methods for data-binding support in Spice.
/// Provides helpers to wire PropertyChanged subscriptions with minimal boilerplate.
/// </summary>
public static class BindingExtensions
{
	/// <summary>
	/// One-way binding: subscribes to source.PropertyChanged and invokes
	/// <paramref name="apply"/> whenever the named property changes.
	/// Sets the initial value immediately.
	/// </summary>
	/// <typeparam name="TSource">The type of the source object implementing INotifyPropertyChanged.</typeparam>
	/// <typeparam name="TValue">The type of the property value.</typeparam>
	/// <param name="source">The source object to bind to.</param>
	/// <param name="propertyName">The name of the property to observe (use nameof() for compile-time safety).</param>
	/// <param name="getter">Function to retrieve the property value from the source.</param>
	/// <param name="apply">Action to execute when the property changes or on initial sync.</param>
	/// <returns>An IDisposable that can be used to unsubscribe from the PropertyChanged event.</returns>
	/// <example>
	/// <code>
	/// var vm = new MyViewModel();
	/// var label = new Label();
	/// vm.Bind(nameof(vm.Title), v => v.Title, text => label.Text = text);
	/// </code>
	/// </example>
	public static IDisposable Bind<TSource, TValue>(
		this TSource source,
		string propertyName,
		Func<TSource, TValue> getter,
		Action<TValue> apply)
		where TSource : INotifyPropertyChanged
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(propertyName);
		ArgumentNullException.ThrowIfNull(getter);
		ArgumentNullException.ThrowIfNull(apply);

		// Sync initial value
		apply(getter(source));

		PropertyChangedEventHandler handler = (_, e) =>
		{
			if (e.PropertyName == propertyName)
				apply(getter(source));
		};
		source.PropertyChanged += handler;

		return new BindingSubscription(source, handler);
	}
}

/// <summary>
/// Represents a subscription to a property binding that can be disposed to clean up the event handler.
/// </summary>
internal sealed class BindingSubscription : IDisposable
{
	private readonly INotifyPropertyChanged _source;
	private readonly PropertyChangedEventHandler _handler;
	private bool _disposed;

	public BindingSubscription(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
	{
		_source = source;
		_handler = handler;
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_source.PropertyChanged -= _handler;
			_disposed = true;
		}
	}
}
