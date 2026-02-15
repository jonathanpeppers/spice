using System.Collections.Specialized;

namespace Spice;

public partial class TabView
{
	/// <summary>
	/// TabView ctor - creates a UITabBarController
	/// </summary>
	public TabView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.All })
	{
		Children.CollectionChanged += OnTabsChanged;
	}

	WeakReference<UITabBarController>? _tabBarController;

	/// <summary>
	/// Gets the underlying UITabBarController
	/// </summary>
	UITabBarController TabBarController
	{
		get
		{
			UITabBarController? controller = null;
			if (_tabBarController == null || !_tabBarController.TryGetTarget(out controller))
			{
				controller = new UITabBarController();
				_tabBarController = new WeakReference<UITabBarController>(controller);
				NativeView.AddSubview(controller.View);
				controller.View.Frame = NativeView.Bounds;
				controller.View.AutoresizingMask = UIViewAutoresizing.All;
			}
			return controller;
		}
	}

	void OnTabsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateTabs();
	}

	void UpdateTabs()
	{
		var viewControllers = new List<UIViewController>();

		foreach (var child in Children)
		{
			if (child is Tab tab)
			{
				tab.EnsureContent();
				
				var viewController = new UIViewController();
				if (tab.Content != null)
				{
					viewController.View!.AddSubview(tab.Content);
					((UIView)tab.Content).Frame = viewController.View.Bounds;
					((UIView)tab.Content).AutoresizingMask = UIViewAutoresizing.All;
				}
				
				viewController.Title = tab.Title;
				
				// Set tab bar item
				if (!string.IsNullOrEmpty(tab.Icon))
				{
					viewController.TabBarItem = new UITabBarItem(tab.Title, UIImage.FromBundle(tab.Icon), 0);
				}
				else
				{
					viewController.TabBarItem = new UITabBarItem(tab.Title, null, 0);
				}
				
				viewControllers.Add(viewController);
			}
		}

		TabBarController.ViewControllers = viewControllers.ToArray();
	}
}

public partial class Tab
{
	/// <summary>
	/// Tab ctor - tabs are logical containers, not views themselves
	/// </summary>
	public Tab() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None })
	{
	}
}
