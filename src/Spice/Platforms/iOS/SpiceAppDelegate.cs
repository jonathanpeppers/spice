using Foundation;
using ObjCRuntime;

namespace Spice;

/// <summary>
/// A <see cref="IUIWindowSceneDelegate"/> for Spice applications that sets up the <see cref="UIWindow"/>
/// and root <see cref="Application"/> view using the modern scene-based lifecycle.
/// Declared as <c>UISceneDelegateClassName</c> in Info.plist.
/// </summary>
[Register("SpiceSceneDelegate")]
public class SpiceSceneDelegate : UIResponder, IUIWindowSceneDelegate
{
	/// <summary>
	/// The main window for the scene.
	/// </summary>
	[Export("window")]
	public UIWindow? Window { get; set; }

	/// <summary>
	/// Called when a new scene session is being created and the UIWindow needs to be configured.
	/// </summary>
	[Export("scene:willConnectToSession:options:")]
	public virtual void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
	{
		if (scene is UIWindowScene windowScene)
		{
			Window = Platform.Window = new UIWindow(windowScene);

			var appDelegate = (SpiceAppDelegate)UIApplication.SharedApplication.Delegate;
			var vc = new UIViewController();
			vc.View!.AddSubview(appDelegate.CreateApplication());
			Window.RootViewController = vc;
			Window.MakeKeyAndVisible();
		}
	}
}

/// <summary>
/// The <see cref="UIApplicationDelegate"/> to use in Spice applications.
/// Override <see cref="CreateApplication"/> to provide the root <see cref="Application"/> view.
/// Requires <c>UIApplicationSceneManifest</c> in Info.plist with <c>UISceneDelegateClassName</c> set to <c>SpiceSceneDelegate</c>.
/// </summary>
public class SpiceAppDelegate : UIApplicationDelegate
{
	/// <summary>
	/// Creates the root <see cref="Application"/> view for the app.
	/// Override this method to return your <see cref="Application"/> subclass.
	/// </summary>
	public virtual Application CreateApplication() =>
		throw new NotImplementedException($"Override {nameof(CreateApplication)} in your AppDelegate to return your Application subclass.");
}
