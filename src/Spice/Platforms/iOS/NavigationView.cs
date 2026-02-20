namespace Spice;

public partial class NavigationView
{
	/// <summary>
	/// Implicit conversion to UINavigationController.
	/// </summary>
	public static implicit operator UINavigationController(NavigationView nav) => nav.NavigationController;

	/// <summary>
	/// NavigationView ctor - creates a UINavigationController
	/// </summary>
	public NavigationView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.All })
	{
	}

	WeakReference<UINavigationController>? _navigationController;

	/// <summary>
	/// Gets the underlying UINavigationController
	/// </summary>
	UINavigationController NavigationController
	{
		get
		{
			UINavigationController? controller = null;
			if (_navigationController == null || !_navigationController.TryGetTarget(out controller))
			{
				controller = new UINavigationController();
				_navigationController = new WeakReference<UINavigationController>(controller);
				NativeView.AddSubview(controller.View!);
				controller.View!.Frame = NativeView.Bounds;
				controller.View!.AutoresizingMask = UIViewAutoresizing.All;
			}
			return controller;
		}
	}

	partial void PushCore(View view)
	{
		var viewController = new UIViewController();
		var nativeView = (UIView)view;
		viewController.View!.AddSubview(nativeView);
		
		// Set the title from the view's Title property
		viewController.Title = view.Title;
		
		// Layout the view to fill the view controller's view
		nativeView.Frame = viewController.View.Bounds;
		nativeView.AutoresizingMask = UIViewAutoresizing.All;
		
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
