namespace Spice;

/// <summary>
/// A view that provides push/pop navigation with a navigation bar.
/// Platform implementations: iOS UINavigationController / Android Toolbar with FrameLayout
/// </summary>
public partial class NavigationView : View
{
	/// <summary>
	/// Creates a NavigationView with the specified root view.
	/// </summary>
	/// <param name="root">The root view to display in the navigation stack.</param>
	public NavigationView(View root)
	{
		ArgumentNullException.ThrowIfNull(root);
		Push(root);
	}

	/// <summary>
	/// Creates a NavigationView with a factory function for the root view.
	/// The root view is created lazily when needed.
	/// </summary>
	/// <param name="factory">A function that creates the root view.</param>
	public NavigationView(Func<View> factory)
	{
		ArgumentNullException.ThrowIfNull(factory);
		Push(factory);
	}

	/// <summary>
	/// Pushes a view onto the navigation stack.
	/// </summary>
	/// <param name="view">The view to push.</param>
	public void Push(View view)
	{
		ArgumentNullException.ThrowIfNull(view);
		view.Navigation = this;
		PushCore(view);
	}

	/// <summary>
	/// Pushes a view created by a factory function onto the navigation stack.
	/// The view is created immediately when this method is called.
	/// </summary>
	/// <param name="factory">A function that creates the view to push.</param>
	public void Push(Func<View> factory)
	{
		ArgumentNullException.ThrowIfNull(factory);
		var view = factory();
		Push(view);
	}

	/// <summary>
	/// Pushes a view of type T onto the navigation stack.
	/// The view is created lazily using its parameterless constructor.
	/// </summary>
	/// <typeparam name="T">The type of view to push. Must have a parameterless constructor.</typeparam>
	public void Push<T>() where T : View, new()
	{
		Push(new T());
	}

	/// <summary>
	/// Pops the top view from the navigation stack.
	/// </summary>
	public void Pop()
	{
		PopCore();
	}

	/// <summary>
	/// Pops all views except the root view from the navigation stack.
	/// </summary>
	public void PopToRoot()
	{
		PopToRootCore();
	}

	/// <summary>
	/// Platform-specific implementation of Push.
	/// </summary>
	partial void PushCore(View view);

	/// <summary>
	/// Platform-specific implementation of Pop.
	/// </summary>
	partial void PopCore();

	/// <summary>
	/// Platform-specific implementation of PopToRoot.
	/// </summary>
	partial void PopToRootCore();
}

/// <summary>
/// A NavigationView that creates its root view lazily using a parameterless constructor.
/// </summary>
/// <typeparam name="TRoot">The type of the root view. Must have a parameterless constructor.</typeparam>
public partial class NavigationView<TRoot> : NavigationView where TRoot : View, new()
{
	/// <summary>
	/// Creates a NavigationView that will lazily create a root view of type TRoot.
	/// </summary>
	public NavigationView() : base(() => new TRoot())
	{
	}
}
