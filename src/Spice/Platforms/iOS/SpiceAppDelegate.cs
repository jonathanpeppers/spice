using Foundation;
using ObjCRuntime;

namespace Spice;

/// <summary>
/// A <see cref="IUIWindowSceneDelegate"/> for Spice applications that sets up the <see cref="UIWindow"/>
/// and root <see cref="Application"/> view using the modern scene-based lifecycle.
/// </summary>
/// <typeparam name="TApp">The <see cref="Application"/> subclass to display as the root view.</typeparam>
public class SpiceSceneDelegate<TApp> : UIResponder, IUIWindowSceneDelegate where TApp : Application, new()
{
	/// <summary>
	/// The main window for the scene.
	/// </summary>
	[Export("window")]
	public UIWindow? Window { get; set; }

	/// <summary>
	/// The app view that is displayed in the window.
	/// </summary>
	private TApp? _appView;

	/// <summary>
	/// Called when a new scene session is being created and the UIWindow needs to be configured.
	/// </summary>
	[Export("scene:willConnectToSession:options:")]
	public virtual void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
	{
		if (scene is UIWindowScene windowScene)
		{
			Window = Platform.Window = new UIWindow(windowScene);

			var vc = new UIViewController();
			_appView = new TApp();
			vc.View!.AddSubview(_appView);
			Window.RootViewController = vc;
			Window.MakeKeyAndVisible();
		}
	}

	/// <summary>
	/// Called when the scene is about to disconnect from the session.
	/// </summary>
	[Export("sceneDidDisconnect:")]
	public virtual void DidDisconnect(UIScene scene)
	{
		// Dispose the app view and all its children
		if (_appView != null)
		{
			View.DisposeRecursive(_appView);
			_appView = null;
		}
	}
}

/// <summary>
/// The <see cref="UIApplicationDelegate"/> to use in Spice applications.
/// Configures the app to use <see cref="SpiceSceneDelegate{TApp}"/> for the modern scene-based lifecycle.
/// Requires <c>UIApplicationSceneManifest</c> in Info.plist.
/// </summary>
/// <typeparam name="TApp">The <see cref="Application"/> subclass to display as the root view.</typeparam>
public class SpiceAppDelegate<TApp> : UIApplicationDelegate where TApp : Application, new()
{
	/// <inheritdoc />
	public override UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
	{
		var config = new UISceneConfiguration("Default Configuration", connectingSceneSession.Role);
		config.DelegateType = typeof(SpiceSceneDelegate<TApp>);
		return config;
	}
}
