namespace Spice;

/// <summary>
/// The root "view" of a Spice application. Set Main to a single view.
/// </summary>
public partial class Application : View
{
	/// <summary>
	/// Gets the current Application instance. Only one Application instance should exist per process.
	/// </summary>
	public static Application? Current { get; internal set; }

#if VANILLA
	/// <summary>
	/// Creates a new Application instance.
	/// </summary>
	public Application()
	{
		Current = this;
	}
#endif

	/// <summary>
	/// The single, main "view" of this application
	/// </summary>
	[ObservableProperty]
	View? _main;

	/// <summary>
	/// Presents a view modally over the current view.
	/// </summary>
	/// <param name="view">The view to present.</param>
	/// <returns>A task that completes when the view has been presented.</returns>
	public Task PresentAsync(View view)
	{
		ArgumentNullException.ThrowIfNull(view);
		return PresentAsyncCore(view);
	}

	/// <summary>
	/// Presents a view created by a factory function modally over the current view.
	/// </summary>
	/// <param name="factory">A function that creates the view to present.</param>
	/// <returns>A task that completes when the view has been presented.</returns>
	public Task PresentAsync(Func<View> factory)
	{
		ArgumentNullException.ThrowIfNull(factory);
		var view = factory();
		return PresentAsync(view);
	}

	/// <summary>
	/// Presents a view of type T modally over the current view.
	/// The view is created using its parameterless constructor.
	/// </summary>
	/// <typeparam name="T">The type of view to present. Must have a parameterless constructor.</typeparam>
	/// <returns>A task that completes when the view has been presented.</returns>
	public Task PresentAsync<T>() where T : View, new()
	{
		return PresentAsync(new T());
	}

	/// <summary>
	/// Dismisses the currently presented modal view.
	/// </summary>
	/// <returns>A task that completes when the view has been dismissed.</returns>
	public Task DismissAsync()
	{
		return DismissAsyncCore();
	}

	/// <summary>
	/// Platform-specific implementation of PresentAsync.
	/// </summary>
#if VANILLA
	private Task PresentAsyncCore(View view) => Task.CompletedTask;
#else
	private partial Task PresentAsyncCore(View view);
#endif

	/// <summary>
	/// Platform-specific implementation of DismissAsync.
	/// </summary>
#if VANILLA
	private Task DismissAsyncCore() => Task.CompletedTask;
#else
	private partial Task DismissAsyncCore();
#endif
}