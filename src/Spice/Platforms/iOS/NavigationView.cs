namespace Spice;

public partial class NavigationView
{
	/// <summary>
	/// NavigationView ctor - creates a UINavigationController
	/// </summary>
	public NavigationView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.All })
	{
	}

	UINavigationController? _navigationController;

	/// <summary>
	/// Gets the underlying UINavigationController
	/// </summary>
	UINavigationController NavigationController
	{
		get
		{
			if (_navigationController == null)
			{
				_navigationController = new UINavigationController();
				NativeView.AddSubview(_navigationController.View);
				_navigationController.View.Frame = NativeView.Bounds;
				_navigationController.View.AutoresizingMask = UIViewAutoresizing.All;
			}
			return _navigationController;
		}
	}

	partial void PushCore(View view)
	{
		var viewController = new UIViewController();
		viewController.View!.AddSubview(view);
		
		// Set the title from the view's Title property
		viewController.Title = view.Title;
		
		// Layout the view to fill the view controller's view
		((UIView)view).Frame = viewController.View.Bounds;
		((UIView)view).AutoresizingMask = UIViewAutoresizing.All;
		
		NavigationController.PushViewController(viewController, animated: true);
	}

	partial void PopCore()
	{
		NavigationController.PopViewController(animated: true);
	}

	partial void PopToRootCore()
	{
		NavigationController.PopToRootViewController(animated: true);
	}
}
