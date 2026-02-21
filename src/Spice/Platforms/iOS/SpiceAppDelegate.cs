using System.Diagnostics;
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
			var vc = new SpiceViewController();
			var view = vc.View;
			Debug.Assert(view != null, "UIViewController should have a view");
			view.BackgroundColor = UIColor.SystemBackground;
			view.AddSubview(appDelegate.CreateApplication());
			Window.RootViewController = vc;
			Window.MakeKeyAndVisible();
		}
	}
}

/// <summary>
/// A UIViewController that detects system appearance (dark/light mode) changes
/// and notifies <see cref="PlatformAppearance"/>.
/// </summary>
class SpiceViewController : UIViewController
{
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		if (OperatingSystem.IsIOSVersionAtLeast(17))
		{
			RegisterForTraitChanges([typeof(UITraitUserInterfaceStyle)], (IUITraitEnvironment _, UITraitCollection _) =>
			{
				PlatformAppearance.OnChanged(PlatformAppearance.IsDarkMode);
			});
		}
	}

#pragma warning disable CA1422 // Validate platform compatibility (fallback for iOS 16)
	public override void TraitCollectionDidChange(UITraitCollection? previousTraitCollection)
	{
		base.TraitCollectionDidChange(previousTraitCollection);

		if (!OperatingSystem.IsIOSVersionAtLeast(17) &&
			previousTraitCollection?.UserInterfaceStyle != TraitCollection.UserInterfaceStyle)
		{
			PlatformAppearance.OnChanged(PlatformAppearance.IsDarkMode);
		}
	}
#pragma warning restore CA1422
}

/// <summary>
/// The <see cref="UIApplicationDelegate"/> to use in Spice applications.
/// Override <see cref="CreateApplication"/> to provide the root <see cref="Application"/> view.
/// Requires <c>UIApplicationSceneManifest</c> in Info.plist with <c>UISceneDelegateClassName</c> set to <c>SpiceSceneDelegate</c>.
/// </summary>
public abstract class SpiceAppDelegate : UIApplicationDelegate
{
	/// <summary>
	/// Creates the root <see cref="Application"/> view for the app.
	/// </summary>
	public abstract Application CreateApplication();
}
