namespace Spice;

/// <summary>
/// A view that displays bottom tabs for navigation between different content views.
/// Platform implementations: iOS UITabBarController / Android BottomNavigationView
/// </summary>
public partial class TabView : View
{
	/// <summary>
	/// Adds a tab to the TabView. Supports collection initializer syntax.
	/// </summary>
	/// <param name="tab">The tab to add.</param>
	public void Add(Tab tab)
	{
		ArgumentNullException.ThrowIfNull(tab);
		Children.Add(tab);
	}
}

/// <summary>
/// Represents a single tab in a TabView.
/// </summary>
public partial class Tab : View
{
	/// <summary>
	/// Gets or sets the icon for the tab. Should be a resource name or path.
	/// </summary>
	[ObservableProperty]
	string _icon = "";

	View? _content;

	/// <summary>
	/// Gets the content view for this tab.
	/// </summary>
	public View? Content
	{
		get => _content;
		private set
		{
			if (_content != value)
			{
				_content = value;
				if (_content != null && Children.Count == 0)
				{
					Children.Add(_content);
				}
			}
		}
	}

	/// <summary>
	/// Creates a Tab with a title, icon, and content view.
	/// </summary>
	/// <param name="title">The title of the tab.</param>
	/// <param name="icon">The icon resource name or path for the tab.</param>
	/// <param name="content">The content view to display when this tab is selected.</param>
	public Tab(string title, string icon, View content)
	{
		ArgumentNullException.ThrowIfNull(title);
		ArgumentNullException.ThrowIfNull(icon);
		ArgumentNullException.ThrowIfNull(content);
		
		Title = title;
		Icon = icon;
		Content = content;
	}

	/// <summary>
	/// Creates a Tab with a title, icon, and a factory function for the content view.
	/// The content is created lazily when the tab is first selected.
	/// </summary>
	/// <param name="title">The title of the tab.</param>
	/// <param name="icon">The icon resource name or path for the tab.</param>
	/// <param name="factory">A function that creates the content view.</param>
	public Tab(string title, string icon, Func<View> factory)
	{
		ArgumentNullException.ThrowIfNull(title);
		ArgumentNullException.ThrowIfNull(icon);
		ArgumentNullException.ThrowIfNull(factory);
		
		Title = title;
		Icon = icon;
		_contentFactory = factory;
	}

	Func<View>? _contentFactory;

	/// <summary>
	/// Ensures content is created if it hasn't been already.
	/// Called by platform implementations when tab is first selected.
	/// </summary>
	public void EnsureContent()
	{
		if (Content == null && _contentFactory != null)
		{
			Content = _contentFactory();
			_contentFactory = null; // Release the factory after creating content
		}
	}
}

/// <summary>
/// A Tab that creates its content view lazily using a parameterless constructor.
/// </summary>
/// <typeparam name="TContent">The type of the content view. Must have a parameterless constructor.</typeparam>
public partial class Tab<TContent> : Tab where TContent : View, new()
{
	/// <summary>
	/// Creates a Tab that will lazily create content of type TContent.
	/// </summary>
	/// <param name="title">The title of the tab.</param>
	/// <param name="icon">The icon resource name or path for the tab.</param>
	public Tab(string title, string icon) : base(title, icon, () => new TContent())
	{
	}
}
