# Navigation Specification for Spice 🌶

This document describes how navigation could work in Spice, supporting both iOS and Android platforms with their respective native navigation patterns.

## Overview

Navigation in Spice should:
- Use native platform navigation patterns and controls
- Support hierarchical (push/pop) and modal navigation
- Support tabbed navigation with optional tab bars
- Leverage AndroidX and UIKit best practices
- Remain minimal and true to Spice's philosophy of direct native control access

## iOS Navigation

### UINavigationController (Hierarchical Navigation)

iOS uses **UINavigationController** for managing hierarchical navigation stacks. This is the primary navigation pattern for drilling down into content.

#### Key Concepts
- **Navigation Stack**: An ordered array of view controllers, managed automatically
- **Push Navigation**: Add a new view controller to the stack with `pushViewController(_:animated:)`
- **Pop Navigation**: Remove and go back with `popViewController(animated:)` or `popToRootViewController(animated:)`
- **Navigation Bar**: Automatic top bar with back button, title, and customizable buttons
- **Gestures**: Built-in swipe-to-go-back gesture

#### Implementation Pattern
```csharp
// Creating a navigation controller
var rootVC = new UIViewController();
rootVC.View!.AddSubview(new App());
var navController = new UINavigationController(rootVC);
Window.RootViewController = navController;

// Pushing to a new screen
var detailVC = new UIViewController();
detailVC.View!.AddSubview(new DetailView());
NavigationController?.PushViewController(detailVC, animated: true);

// Popping back
NavigationController?.PopViewController(animated: true);

// Going back to root
NavigationController?.PopToRootViewController(animated: true);
```

#### Customization
- Use `UIViewController.NavigationItem` to customize navigation bar per screen
- Set titles, buttons, prompts, and appearance
- Can hide/show navigation bar per screen

**Official Documentation:**
- [UINavigationController - Apple Developer](https://developer.apple.com/documentation/uikit/uinavigationcontroller)
- [Customizing your app's navigation bar - Apple Developer](https://developer.apple.com/documentation/uikit/customizing-your-app-s-navigation-bar)

### UITabBarController (Tab Navigation)

iOS uses **UITabBarController** for switching between top-level sections of an app, typically displayed at the bottom.

#### Key Concepts
- Configure tabs by setting the `viewControllers` property with an array of UIViewController instances
- iOS 18+ introduces `UITab` objects via the `tabs` property for enhanced customization
- Each tab typically contains a UINavigationController for hierarchical navigation within that section
- Supports badges for notifications
- Automatically handles "More" tab for overflow (5+ tabs)
- On iPad, may switch to sidebar layout based on available space

#### Implementation Pattern
```csharp
// Creating a tab bar controller
var homeVC = new UINavigationController(new UIViewController());
homeVC.TabBarItem = new UITabBarItem("Home", UIImage.FromFile("home.png"), 0);

var searchVC = new UINavigationController(new UIViewController());
searchVC.TabBarItem = new UITabBarItem("Search", UIImage.FromFile("search.png"), 1);

var profileVC = new UINavigationController(new UIViewController());
profileVC.TabBarItem = new UITabBarItem("Profile", UIImage.FromFile("profile.png"), 2);

var tabBarController = new UITabBarController();
tabBarController.ViewControllers = new[] { homeVC, searchVC, profileVC };

Window.RootViewController = tabBarController;
```

#### iOS 18+ Modern Approach
```csharp
// Using UITab objects (iOS 18+)
var homeTab = new UITab("Home", UIImage.FromFile("home.png"), "home", _ => new HomeViewController());
var searchTab = new UITab("Search", UIImage.FromFile("search.png"), "search", _ => new SearchViewController());

var tabBarController = new UITabBarController();
tabBarController.Tabs = new[] { homeTab, searchTab };
```

#### Best Practices
- Use 3-5 tabs for optimal user experience
- Each tab should represent a distinct top-level section
- Don't use tab bars for in-context actions (use toolbars instead)
- Combine with UINavigationController for hierarchical navigation within tabs

**Official Documentation:**
- [UITabBarController - Apple Developer](https://developer.apple.com/documentation/uikit/uitabbarcontroller)
- [Tab bars - Human Interface Guidelines](https://developer.apple.com/design/human-interface-guidelines/tab-bars/)
- [tabs property - Apple Developer](https://developer.apple.com/documentation/uikit/uitabbarcontroller/tabs) (iOS 18+)

### Modal Presentation

iOS supports **modal presentation** for displaying content that requires immediate attention or is temporarily apart from the main navigation flow.

#### Key Concepts
- **Present**: Show a view controller modally with `present(_:animated:completion:)`
- **Dismiss**: Close the modal with `dismiss(animated:completion:)`
- **Presentation Styles**: Multiple styles including `.fullScreen`, `.pageSheet`, `.formSheet`, `.popover`
- **Interactive Dismissal**: iOS 13+ enables swipe-to-dismiss by default for sheet styles
- **Blocking Dismissal**: Set `isModalInPresentation = true` to require programmatic dismissal

#### Presentation Styles

| Style | Behavior | Notes |
|-------|----------|-------|
| `.fullScreen` | Covers entire screen | No swipe-to-dismiss; must dismiss programmatically |
| `.pageSheet` | Sheet from bottom (iPhone) | Default iOS 13+; swipe-to-dismiss enabled |
| `.formSheet` | Centered sheet (iPad) | Smaller than full screen |
| `.popover` | Popover (iPad) | Tap outside to dismiss |
| `.automatic` | System chooses | Usually `.pageSheet` on iPhone |

#### Implementation Pattern
```csharp
// Presenting a modal (full screen)
var modalVC = new UIViewController();
modalVC.View!.AddSubview(new ModalView());
modalVC.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
PresentViewController(modalVC, animated: true, null);

// Presenting a modal (sheet with swipe-to-dismiss)
var sheetVC = new UIViewController();
sheetVC.View!.AddSubview(new SheetView());
sheetVC.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
PresentViewController(sheetVC, animated: true, null);

// Preventing interactive dismissal
var modalVC = new UIViewController();
modalVC.IsModalInPresentation = true; // Requires programmatic dismissal
modalVC.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
PresentViewController(modalVC, animated: true, null);

// Dismissing a modal
DismissViewController(animated: true, null);
```

#### Navigation vs Modal
- **Push (UINavigationController)**: For hierarchical navigation within the same context; shows back button
- **Modal (present)**: For temporary tasks or different contexts; requires explicit dismiss button

**Official Documentation:**
- [present(_:animated:completion:) - Apple Developer](https://developer.apple.com/documentation/uikit/uiviewcontroller/present(_:animated:completion:))
- [dismiss(animated:completion:) - Apple Developer](https://developer.apple.com/documentation/uikit/uiviewcontroller/dismiss(animated:completion:))
- [modalPresentationStyle - Apple Developer](https://developer.apple.com/documentation/uikit/uiviewcontroller/modalpresentationstyle)

### Scene-Based Architecture

iOS 13+ introduced **scene support** for multi-window apps, particularly useful for iPad.

#### Current Spice Implementation
Spice already supports scenes via `SpiceSceneDelegate<TApp>` and `SpiceAppDelegate<TApp>`:
- Scene configuration in Info.plist with `UIApplicationSceneManifest`
- `SpiceSceneDelegate` creates `UIWindow` and sets up root view controller
- Access window via `Platform.Window`

**Official Documentation:**
- [Scenes - Apple Developer](https://developer.apple.com/documentation/uikit/app_and_environment/scenes)

## Android Navigation

### Multiple Activities

Android's fundamental navigation unit is the **Activity**. Apps can have multiple activities, each representing a screen.

#### Key Concepts
- Each activity is registered in AndroidManifest.xml
- Activities are launched using **Intents**
- Activities maintain their own lifecycle
- Back button automatically pops the activity stack
- Activities can return results to the caller

#### Implementation Pattern
```csharp
// Launching a new activity
var intent = new Intent(this, typeof(DetailActivity));
intent.PutExtra("itemId", 123);
StartActivity(intent);

// With animation
StartActivity(intent);
OverridePendingTransition(Android.Resource.Animation.SlideInLeft, 
                          Android.Resource.Animation.SlideOutRight);

// Starting for result
var intent = new Intent(this, typeof(PickerActivity));
StartActivityForResult(intent, REQUEST_CODE);

// In the called activity, return result
var resultIntent = new Intent();
resultIntent.PutExtra("selectedItem", itemName);
SetResult(Result.Ok, resultIntent);
Finish();
```

#### AndroidManifest.xml Registration
```xml
<activity 
    android:name=".MainActivity"
    android:exported="true">
    <intent-filter>
        <action android:name="android.intent.action.MAIN" />
        <category android:name="android.intent.category.LAUNCHER" />
    </intent-filter>
</activity>
<activity 
    android:name=".DetailActivity"
    android:parentActivityName=".MainActivity" />
```

**Official Documentation:**
- [Activities - Android Developers](https://developer.android.com/guide/components/activities/intro-activities)
- [Intents - Android Developers](https://developer.android.com/guide/components/intents-filters)

### AndroidX Navigation Component

The modern approach for fragment-based navigation is the **AndroidX Navigation Component**, part of Android Jetpack.

#### Key Concepts
- **NavHostFragment**: Container for displaying navigation destinations
- **NavController**: Manages navigation within a NavHost
- **NavGraph**: XML declaration of destinations and navigation paths
- **Safe Args**: Type-safe argument passing between destinations
- **NavigationUI**: Helpers for integrating with app bar, drawer, and bottom nav

#### Benefits
- Declarative navigation with XML
- Automatic back stack management
- Type-safe argument passing
- Built-in support for animations and transitions
- Deep linking support
- ViewModel scoping to navigation graphs

#### Setup
```gradle
dependencies {
    def nav_version = "2.9.7"
    implementation "androidx.navigation:navigation-fragment:$nav_version"
    implementation "androidx.navigation:navigation-ui:$nav_version"
}
```

#### Navigation Graph (res/navigation/nav_graph.xml)
```xml
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    app:startDestination="@id/homeFragment">
    
    <fragment
        android:id="@+id/homeFragment"
        android:name="com.example.HomeFragment">
        <action
            android:id="@+id/action_home_to_detail"
            app:destination="@id/detailFragment"
            app:enterAnim="@anim/slide_in_right"
            app:exitAnim="@anim/slide_out_left" />
    </fragment>
    
    <fragment
        android:id="@+id/detailFragment"
        android:name="com.example.DetailFragment">
        <argument
            android:name="itemId"
            app:argType="integer" />
    </fragment>
</navigation>
```

#### Activity Layout
```xml
<androidx.fragment.app.FragmentContainerView
    android:id="@+id/nav_host_fragment"
    android:name="androidx.navigation.fragment.NavHostFragment"
    app:navGraph="@navigation/nav_graph"
    app:defaultNavHost="true"
    android:layout_width="match_parent"
    android:layout_height="match_parent" />
```

#### Navigation in Code (C#)
```csharp
// In a Fragment
var navController = NavHostFragment.FindNavController(this);
navController.Navigate(Resource.Id.action_home_to_detail);

// With arguments
var bundle = new Bundle();
bundle.PutInt("itemId", 123);
navController.Navigate(Resource.Id.detailFragment, bundle);

// Navigate back
navController.NavigateUp();
navController.PopBackStack();
```

**Official Documentation:**
- [Navigation Component - Android Developers](https://developer.android.com/guide/navigation/)
- [Navigation releases - AndroidX](https://developer.android.com/jetpack/androidx/releases/navigation)
- [Single-Activity Architecture Guide](https://codezup.com/android-navigation-component-single-activity-architecture/)

### Bottom Navigation / Tab Bar

Android uses **BottomNavigationView** (Material Design 3: "Navigation Bar") for switching between top-level destinations.

#### Key Concepts
- Part of Material Components for Android
- Material 3 introduces updated pill-shaped active indicators
- Supports 3-5 items (recommended)
- Can show badges for notifications
- Integrates with Navigation Component
- Adaptive for different screen sizes

#### Setup
```gradle
dependencies {
    implementation 'com.google.android.material:material:1.12.0'
}
```

#### Layout (activity_main.xml)
```xml
<androidx.constraintlayout.widget.ConstraintLayout ...>
    
    <!-- Fragment container -->
    <androidx.fragment.app.FragmentContainerView
        android:id="@+id/nav_host_fragment"
        android:name="androidx.navigation.fragment.NavHostFragment"
        app:navGraph="@navigation/nav_graph"
        android:layout_width="match_parent"
        android:layout_height="0dp"
        app:layout_constraintBottom_toTopOf="@id/bottom_navigation"
        app:layout_constraintTop_toTopOf="parent" />
    
    <!-- Bottom Navigation -->
    <com.google.android.material.bottomnavigation.BottomNavigationView
        android:id="@+id/bottom_navigation"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        app:menu="@menu/bottom_nav_menu"
        app:labelVisibilityMode="labeled"
        app:layout_constraintBottom_toBottomOf="parent" />
        
</androidx.constraintlayout.widget.ConstraintLayout>
```

#### Menu Definition (res/menu/bottom_nav_menu.xml)
```xml
<menu xmlns:android="http://schemas.android.com/apk/res/android">
    <item
        android:id="@+id/home"
        android:title="Home"
        android:icon="@drawable/ic_home" />
    <item
        android:id="@+id/search"
        android:title="Search"
        android:icon="@drawable/ic_search" />
    <item
        android:id="@+id/profile"
        android:title="Profile"
        android:icon="@drawable/ic_profile" />
</menu>
```

#### Wiring Up in Activity (C#)
```csharp
var bottomNav = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
var navController = Navigation.FindNavController(this, Resource.Id.nav_host_fragment);

// Connect bottom nav to Navigation Component
NavigationUI.SetupWithNavController(bottomNav, navController);

// Or handle manually
bottomNav.ItemSelected += (sender, e) =>
{
    switch (e.Item.ItemId)
    {
        case Resource.Id.home:
            navController.Navigate(Resource.Id.homeFragment);
            return true;
        case Resource.Id.search:
            navController.Navigate(Resource.Id.searchFragment);
            return true;
        case Resource.Id.profile:
            navController.Navigate(Resource.Id.profileFragment);
            return true;
    }
    return false;
};
```

#### Best Practices
- Use 3-5 top-level destinations
- For wider screens (>600dp), consider NavigationRail instead
- Always set `android:title` for accessibility
- Use Material 3 styles for modern appearance
- Don't combine with bottom app bar

**Official Documentation:**
- [Navigation Bar - Material Design 3](https://m3.material.io/components/navigation-bar/overview)
- [BottomNavigationView - Android API](https://developer.android.com/reference/com/google/android/material/bottomnavigation/BottomNavigationView)
- [Bottom Navigation Implementation Guide](https://www.geeksforgeeks.org/android/bottom-navigation-bar-in-android/)

### Modal vs Regular Navigation (Android)

Android doesn't have a direct "modal" concept like iOS, but similar patterns can be achieved:

#### Dialog Activities
```csharp
// Activity with dialog theme in AndroidManifest.xml
[Activity(Theme = "@style/Theme.AppCompat.Dialog")]
public class ModalActivity : AppCompatActivity
{
    // Activity appears as a dialog over the previous activity
}
```

#### DialogFragment
```csharp
// For modal dialogs that don't cover the entire screen
var dialog = new MyDialogFragment();
dialog.Show(SupportFragmentManager, "modalDialog");
```

#### Bottom Sheet
```csharp
// Material Design bottom sheets for modal-like behavior
var bottomSheet = new MyBottomSheetDialogFragment();
bottomSheet.Show(SupportFragmentManager, "bottomSheet");
```

#### Full-Screen Activity (Modal Equivalent)
```csharp
// Launch activity with specific animation to feel modal
var intent = new Intent(this, typeof(ModalActivity));
StartActivity(intent);
OverridePendingTransition(
    Resource.Animation.abc_slide_in_bottom,
    Resource.Animation.abc_fade_out
);
```

**Official Documentation:**
- [Dialogs - Android Developers](https://developer.android.com/develop/ui/views/components/dialogs)
- [Bottom Sheets - Material Design](https://m3.material.io/components/bottom-sheets/overview)

## Proposed Spice Navigation API

### Design Principles

1. **Stay Minimal**: Don't abstract away platform differences; expose native controls
2. **Use Platform Patterns**: Let developers use UINavigationController and Activities directly
3. **Provide Entry Points**: Offer convenient helpers but don't force an abstraction layer
4. **Type Safety**: Use C# types, not strings or reflection
5. **Testable**: Navigation logic should be testable without device dependencies

### Potential Spice Classes

#### NavigationPage (iOS and Android)
A cross-platform container that wraps platform navigation patterns:

```csharp
// Core/NavigationPage.cs
public partial class NavigationPage : View
{
    [ObservableProperty]
    private View _rootPage;
    
    public void Push(View page) { }
    public void Pop() { }
    public void PopToRoot() { }
}
```

```csharp
// Platforms/iOS/NavigationPage.cs
public partial class NavigationPage
{
    public UINavigationController NativeNavigationController => 
        (UINavigationController)_nativeView.Value;
    
    partial void OnRootPageChanged(View value)
    {
        var vc = new UIViewController();
        vc.View!.AddSubview(value);
        // Initialize navigation controller
    }
    
    public void Push(View page)
    {
        var vc = new UIViewController();
        vc.View!.AddSubview(page);
        NativeNavigationController.PushViewController(vc, animated: true);
    }
}
```

```csharp
// Platforms/Android/NavigationPage.cs
public partial class NavigationPage
{
    // On Android, might need to work with Intents and Activities
    // Or integrate with Navigation Component
}
```

#### TabbedPage (iOS and Android)
A cross-platform tabbed container:

```csharp
// Core/TabbedPage.cs
public partial class TabbedPage : View
{
    public List<(string Title, View Page)> Pages { get; set; } = new();
}
```

This would map to UITabBarController on iOS and BottomNavigationView on Android.

### Alternative: Direct Platform Access

Given Spice's philosophy, it might be better to **not** create navigation abstractions and instead:

1. **Provide documentation** on using native navigation (like this spec)
2. **Provide sample apps** demonstrating iOS and Android navigation patterns
3. **Expose platform APIs** directly through Spice views
4. **Let developers choose** their navigation approach based on platform

This keeps Spice minimal and doesn't impose navigation patterns that might not fit all use cases.

## Sample Application Structure

### iOS Sample
```csharp
// AppDelegate
public class AppDelegate : SpiceAppDelegate<App> { }

// SceneDelegate
public class SceneDelegate : SpiceSceneDelegate<App> { }

// App
public class App : Application
{
    public App()
    {
        // Option 1: Single view
        Main = new MainView();
        
        // Option 2: Tab bar with navigation
        // Create tab bar programmatically in SceneDelegate
    }
}

// SceneDelegate customization for tabs
public class CustomSceneDelegate : SpiceSceneDelegate<App>
{
    public override void WillConnect(UIScene scene, UISceneSession session, 
                                     UISceneConnectionOptions options)
    {
        if (scene is UIWindowScene windowScene)
        {
            Window = Platform.Window = new UIWindow(windowScene);
            
            var tabBarController = new UITabBarController();
            // Set up tabs with navigation controllers
            
            Window.RootViewController = tabBarController;
            Window.MakeKeyAndVisible();
        }
    }
}
```

### Android Sample
```csharp
// MainActivity
[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : SpiceActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // Option 1: Single view
        SetContentView(new App());
        
        // Option 2: Bottom nav with Navigation Component
        SetContentView(Resource.Layout.activity_main);
        var bottomNav = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav);
        // Set up navigation
    }
}

// DetailActivity
[Activity(Label = "Detail")]
public class DetailActivity : SpiceActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(new DetailView());
    }
}
```

## Recommendations

### For Simple Apps
- iOS: Use a single UIViewController with Spice views
- Android: Use a single Activity with Spice views

### For Apps with Hierarchical Navigation
- iOS: Use UINavigationController with Spice views in view controllers
- Android: Use multiple Activities or Navigation Component with Spice views

### For Apps with Tab Navigation
- iOS: Use UITabBarController with UINavigationController per tab
- Android: Use BottomNavigationView with Navigation Component

### For Modal Dialogs
- iOS: Use `present(_:animated:completion:)` with appropriate presentation style
- Android: Use DialogFragment, BottomSheetDialogFragment, or dialog-themed Activity

## Future Considerations

1. **Deep Linking**: Both platforms support deep linking; document integration
2. **State Restoration**: iOS and Android have state restoration APIs
3. **Transitions/Animations**: Custom transitions between screens
4. **Navigation Guards**: Preventing navigation based on conditions
5. **History Management**: Advanced back stack manipulation
6. **Multi-Window**: iPad multi-window, Android split-screen

## Conclusion

This spec outlines navigation patterns for Spice using native platform APIs. The recommendation is to:

1. **Document best practices** for using native navigation (this document)
2. **Provide sample apps** showing different navigation patterns
3. **Avoid over-abstraction** that hides platform-specific capabilities
4. **Let developers use** UINavigationController, UITabBarController, Activities, and Navigation Component directly
5. **Keep Spice minimal** by not adding navigation abstractions unless there's clear value

This approach maintains Spice's philosophy of API minimalism while enabling developers to build sophisticated navigation experiences using platform conventions.

## References

### iOS Documentation
- [UINavigationController](https://developer.apple.com/documentation/uikit/uinavigationcontroller)
- [UITabBarController](https://developer.apple.com/documentation/uikit/uitabbarcontroller)
- [Modal Presentation](https://developer.apple.com/documentation/uikit/uiviewcontroller/present(_:animated:completion:))
- [Customizing Navigation Bar](https://developer.apple.com/documentation/uikit/customizing-your-app-s-navigation-bar)
- [Tab Bars HIG](https://developer.apple.com/design/human-interface-guidelines/tab-bars/)

### Android Documentation
- [Activities](https://developer.android.com/guide/components/activities/intro-activities)
- [Navigation Component](https://developer.android.com/guide/navigation/)
- [Bottom Navigation](https://m3.material.io/components/navigation-bar/overview)
- [BottomNavigationView API](https://developer.android.com/reference/com/google/android/material/bottomnavigation/BottomNavigationView)
- [Dialogs](https://developer.android.com/develop/ui/views/components/dialogs)
- [Material 3 Design](https://m3.material.io/)
