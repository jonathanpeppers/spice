namespace Spice;

public partial class Application
{
	/// <summary>
	/// The root "view" of a Spice application. Set Main to a single view.
	/// </summary>
	public Application() : base(_ => new UIView(Platform.Window!.Frame) { AutoresizingMask = UIViewAutoresizing.All })
	{
		Current = this;
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Application(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.All })
	{
		Current = this;
	}

	partial void OnMainChanging(View? value)	
	{
		if (_main != null)
		{
			((UIView)_main).RemoveFromSuperview();
		}
	}

	partial void OnMainChanged(View? value)
	{
		if (value != null)
		{
			AddSubview(value);
		}
	}

	UIViewController? _presentedViewController;

	async partial Task PresentAsyncCore(View view)
	{
		var tcs = new TaskCompletionSource<bool>();

		var viewController = new UIViewController();
		viewController.View!.AddSubview(view);
		((UIView)view).Frame = viewController.View.Bounds;
		((UIView)view).AutoresizingMask = UIViewAutoresizing.All;
		
		// Set the title from the view's Title property
		if (!string.IsNullOrEmpty(view.Title))
		{
			viewController.Title = view.Title;
		}

		_presentedViewController = viewController;

		// Get the root view controller
		var rootViewController = Platform.Window?.RootViewController;
		if (rootViewController != null)
		{
			await rootViewController.PresentViewControllerAsync(viewController, animated: true);
		}

		tcs.SetResult(true);
		await tcs.Task;
	}

	async partial Task DismissAsyncCore()
	{
		if (_presentedViewController != null)
		{
			await _presentedViewController.DismissViewControllerAsync(animated: true);
			_presentedViewController = null;
		}
	}
}