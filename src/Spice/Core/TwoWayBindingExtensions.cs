using System.ComponentModel;

namespace Spice;

/// <summary>
/// Extension methods for two-way data-binding support in Spice.
/// </summary>
public static class TwoWayBindingExtensions
{
	/// <summary>
	/// Two-way binding: subscribes to PropertyChanged on both source and target,
	/// syncing changes in either direction. Includes guard to prevent infinite loops.
	/// Sets the initial value from source to target immediately.
	/// </summary>
	/// <typeparam name="TSource">The type of the source object implementing INotifyPropertyChanged.</typeparam>
	/// <typeparam name="TValue">The type of the property value.</typeparam>
	/// <param name="source">The source object (typically a ViewModel).</param>
	/// <param name="sourceProperty">The name of the source property (use nameof()).</param>
	/// <param name="sourceGetter">Function to retrieve the property value from the source.</param>
	/// <param name="sourceSetter">Action to set the property value on the source.</param>
	/// <param name="target">The target object (typically a View).</param>
	/// <param name="targetProperty">The name of the target property (use nameof()).</param>
	/// <param name="targetGetter">Function to retrieve the property value from the target.</param>
	/// <param name="targetSetter">Action to set the property value on the target.</param>
	/// <returns>An IDisposable that can be used to unsubscribe from both PropertyChanged events.</returns>
	/// <example>
	/// <code>
	/// var vm = new MyViewModel();
	/// var entry = new Entry();
	/// vm.BindTwoWay(
	///     nameof(vm.Username), v => v.Username, (v, val) => v.Username = val,
	///     entry,
	///     nameof(Entry.Text), () => entry.Text, val => entry.Text = val);
	/// </code>
	/// </example>
	public static IDisposable BindTwoWay<TSource, TValue>(
		this TSource source,
		string sourceProperty,
		Func<TSource, TValue> sourceGetter,
		Action<TSource, TValue> sourceSetter,
		INotifyPropertyChanged target,
		string targetProperty,
		Func<TValue> targetGetter,
		Action<TValue> targetSetter)
		where TSource : INotifyPropertyChanged
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(sourceProperty);
		ArgumentNullException.ThrowIfNull(sourceGetter);
		ArgumentNullException.ThrowIfNull(sourceSetter);
		ArgumentNullException.ThrowIfNull(target);
		ArgumentNullException.ThrowIfNull(targetProperty);
		ArgumentNullException.ThrowIfNull(targetGetter);
		ArgumentNullException.ThrowIfNull(targetSetter);

		bool updating = false;

		// Initial sync: source → target
		targetSetter(sourceGetter(source));

		PropertyChangedEventHandler sourceHandler = (_, e) =>
		{
			if (!updating && e.PropertyName == sourceProperty)
			{
				updating = true;
				try
				{
					targetSetter(sourceGetter(source));
				}
				finally
				{
					updating = false;
				}
			}
		};

		PropertyChangedEventHandler targetHandler = (_, e) =>
		{
			if (!updating && e.PropertyName == targetProperty)
			{
				updating = true;
				try
				{
					sourceSetter(source, targetGetter());
				}
				finally
				{
					updating = false;
				}
			}
		};

		source.PropertyChanged += sourceHandler;
		target.PropertyChanged += targetHandler;

		return new TwoWayBindingSubscription(source, sourceHandler, target, targetHandler);
	}
}

/// <summary>
/// Represents a two-way binding subscription that can be disposed to clean up both event handlers.
/// </summary>
internal sealed class TwoWayBindingSubscription : IDisposable
{
	private readonly INotifyPropertyChanged _source;
	private readonly PropertyChangedEventHandler _sourceHandler;
	private readonly INotifyPropertyChanged _target;
	private readonly PropertyChangedEventHandler _targetHandler;
	private bool _disposed;

	public TwoWayBindingSubscription(
		INotifyPropertyChanged source,
		PropertyChangedEventHandler sourceHandler,
		INotifyPropertyChanged target,
		PropertyChangedEventHandler targetHandler)
	{
		_source = source;
		_sourceHandler = sourceHandler;
		_target = target;
		_targetHandler = targetHandler;
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_source.PropertyChanged -= _sourceHandler;
			_target.PropertyChanged -= _targetHandler;
			_disposed = true;
		}
	}
}
