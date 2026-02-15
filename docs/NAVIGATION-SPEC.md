# Navigation in Spice 🌶

A simple, succinct, cross-platform navigation API for Spice applications.

## Overview

Spice navigation provides three cross-platform page types:
- **`NavigationPage`** - Hierarchical navigation with push/pop and a navigation bar
- **`TabbedPage`** - Tab-based navigation between multiple top-level pages
- **Modal presentation** - Temporary overlay screens via `PresentAsync()` / `DismissAsync()`

## NavigationPage

Manages a stack of pages with automatic back navigation and a navigation bar.

### Basic Usage

```csharp
public class App : Application
{
    public App()
    {
        Main = new NavigationPage(new HomePage());
    }
}

public class HomePage : ContentView
{
    public HomePage()
    {
        Title = "Home";
        
        Content = new StackLayout
        {
            new Label { Text = "Welcome!" },
            new Button
            {
                Text = "Go to Details",
                Clicked = async _ => await Navigation.PushAsync(new DetailPage())
            }
        };
    }
}

public class DetailPage : ContentView
{
    public DetailPage()
    {
        Title = "Details";
        
        Content = new StackLayout
        {
            new Label { Text = "Detail Page" },
            new Button
            {
                Text = "Go Back",
                Clicked = async _ => await Navigation.PopAsync()
            }
        };
    }
}
```

### API

#### Properties
- **`Title`** (string) - Page title shown in the navigation bar
- **`Navigation`** (INavigation) - Access to navigation methods

#### Methods
- **`PushAsync(View page)`** - Navigate forward to a new page
- **`PopAsync()`** - Navigate back to the previous page
- **`PopToRootAsync()`** - Navigate back to the first page in the stack

#### Platform Mapping
- **iOS**: Uses `UINavigationController` with `UIViewController` per page
- **Android**: Uses multiple Activities with Intent-based navigation, or Fragment-based navigation with AndroidX Navigation Component

## TabbedPage

Switches between multiple top-level pages via tabs.

### Basic Usage

```csharp
public class App : Application
{
    public App()
    {
        Main = new TabbedPage
        {
            Children =
            {
                new NavigationPage(new HomePage()) { Title = "Home", Icon = "home.png" },
                new NavigationPage(new SearchPage()) { Title = "Search", Icon = "search.png" },
                new NavigationPage(new ProfilePage()) { Title = "Profile", Icon = "profile.png" }
            }
        };
    }
}
```

### API

#### Properties
- **`Children`** (ObservableCollection<View>) - Collection of tab pages
- **`Title`** (string) - Tab label
- **`Icon`** (string) - Tab icon (asset name)

#### Platform Mapping
- **iOS**: Uses `UITabBarController` with tab bar at bottom
- **Android**: Uses `BottomNavigationView` with Material Design styling

### Combining Navigation and Tabs

Typically, each tab contains a `NavigationPage` to enable hierarchical navigation within that tab:

```csharp
Main = new TabbedPage
{
    Children =
    {
        new NavigationPage(new HomePage()) { Title = "Home" },
        new NavigationPage(new SearchPage()) { Title = "Search" }
    }
};
```

## Modal Presentation

Display temporary pages that overlay the current context.

### Basic Usage

```csharp
// Present a modal
var modal = new LoginPage();
await Navigation.PresentAsync(modal);

// Dismiss the modal (from within the modal page)
await Navigation.DismissAsync();
```

### API

#### Methods
- **`PresentAsync(View page)`** - Present a page modally
- **`DismissAsync()`** - Dismiss the current modal page

#### Platform Mapping
- **iOS**: Uses `PresentViewController()` with configurable presentation styles
- **Android**: Uses dialog-themed Activity or DialogFragment

## Complete Example

```csharp
public class App : Application
{
    public App()
    {
        Main = new TabbedPage
        {
            Children =
            {
                new NavigationPage(new FeedPage()) { Title = "Feed", Icon = "feed.png" },
                new NavigationPage(new SearchPage()) { Title = "Search", Icon = "search.png" },
                new NavigationPage(new ProfilePage()) { Title = "Profile", Icon = "profile.png" }
            }
        };
    }
}

public class FeedPage : ContentView
{
    public FeedPage()
    {
        Title = "Feed";
        
        Content = new StackLayout
        {
            new Button
            {
                Text = "View Post",
                Clicked = async _ => await Navigation.PushAsync(new PostDetailPage())
            },
            new Button
            {
                Text = "Create Post",
                Clicked = async _ => await Navigation.PresentAsync(new CreatePostPage())
            }
        };
    }
}

public class PostDetailPage : ContentView
{
    public PostDetailPage()
    {
        Title = "Post Details";
        
        Content = new StackLayout
        {
            new Label { Text = "Post content here..." },
            new Button
            {
                Text = "View Comments",
                Clicked = async _ => await Navigation.PushAsync(new CommentsPage())
            }
        };
    }
}

public class CreatePostPage : ContentView
{
    public CreatePostPage()
    {
        Title = "Create Post";
        
        Content = new StackLayout
        {
            new Entry { Placeholder = "What's on your mind?" },
            new Button
            {
                Text = "Post",
                Clicked = async _ =>
                {
                    // Save post logic
                    await Navigation.DismissAsync();
                }
            },
            new Button
            {
                Text = "Cancel",
                Clicked = async _ => await Navigation.DismissAsync()
            }
        };
    }
}
```

## Design Principles

### Simplicity
Navigation is expressed with just three concepts:
- Push/pop for hierarchical navigation
- Tabs for top-level sections
- Modal for temporary overlays

### Cross-Platform
All navigation APIs work identically on iOS and Android, mapping to native patterns:
- iOS: UINavigationController, UITabBarController, modal presentation
- Android: Activities/Fragments, BottomNavigationView, dialogs

### Type-Safe
Navigation uses strongly-typed View instances, not strings or routes. No reflection required.

### Testable
Navigation can be tested in vanilla `net10.0` unit tests without device dependencies.

## Implementation Notes

### Navigation Property

The `Navigation` property is available on all `View` instances and provides access to the navigation stack:

```csharp
public interface INavigation
{
    Task PushAsync(View page);
    Task PopAsync();
    Task PopToRootAsync();
    Task PresentAsync(View page);
    Task DismissAsync();
}
```

### Page Lifecycle

Pages can implement lifecycle methods for navigation events:

```csharp
public class MyPage : ContentView
{
    protected override void OnAppearing()
    {
        // Called when page becomes visible
    }
    
    protected override void OnDisappearing()
    {
        // Called when page is no longer visible
    }
}
```

### Platform-Specific Customization

Access native navigation controllers when needed:

```csharp
#if IOS
// Access UINavigationController
var navController = Platform.Window?.RootViewController as UINavigationController;
navController?.NavigationBar.PrefersLargeTitles = true;
#elif ANDROID
// Access Activity or BottomNavigationView
var activity = Platform.Context as Activity;
activity?.SetTitle("Custom Title");
#endif
```

## Comparison with Native APIs

### iOS

| Spice API | Native iOS API |
|-----------|----------------|
| `NavigationPage` | `UINavigationController` |
| `PushAsync()` | `PushViewController()` |
| `PopAsync()` | `PopViewController()` |
| `TabbedPage` | `UITabBarController` |
| `PresentAsync()` | `PresentViewController()` |
| `DismissAsync()` | `DismissViewController()` |

### Android

| Spice API | Native Android API |
|-----------|-------------------|
| `NavigationPage` | Activity stack or Navigation Component |
| `PushAsync()` | `StartActivity()` or `navigate()` |
| `PopAsync()` | `Finish()` or `navigateUp()` |
| `TabbedPage` | `BottomNavigationView` |
| `PresentAsync()` | Dialog-themed Activity or `DialogFragment` |
| `DismissAsync()` | `Dismiss()` or `Finish()` |

## Migration from Current Approach

Currently, Spice apps navigate by setting `Application.Main` to a different view:

```csharp
// Current approach
new Button
{
    Text = "Go to Details",
    Clicked = _ => Main = new DetailView()
}
```

With navigation pages:

```csharp
// New approach
new Button
{
    Text = "Go to Details",
    Clicked = async _ => await Navigation.PushAsync(new DetailView())
}
```

The new approach provides:
- Automatic back navigation
- Navigation bar with title
- Proper page lifecycle
- Modal presentation support
- Platform-native animations

## Official Documentation

### iOS
- [UINavigationController](https://developer.apple.com/documentation/uikit/uinavigationcontroller) - Hierarchical navigation
- [UITabBarController](https://developer.apple.com/documentation/uikit/uitabbarcontroller) - Tab-based navigation
- [Modal Presentation](https://developer.apple.com/documentation/uikit/uiviewcontroller/present(_:animated:completion:)) - Presenting view controllers

### Android
- [Navigation Component](https://developer.android.com/guide/navigation/) - Modern Android navigation
- [Bottom Navigation](https://m3.material.io/components/navigation-bar/overview) - Material Design tab bar
- [Activities](https://developer.android.com/guide/components/activities/intro-activities) - Android screens
- [Dialogs](https://developer.android.com/develop/ui/views/components/dialogs) - Modal dialogs
