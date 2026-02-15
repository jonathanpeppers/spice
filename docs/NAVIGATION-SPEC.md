# Navigation in Spice 🌶

Three new views and two methods on `Application`. That's it.

## Overview

| Type | What it does | iOS | Android |
|------|-------------|-----|---------|
| `NavigationView` | Push/pop stack with nav bar | `UINavigationController` | AndroidX Navigation |
| `TabView` | Bottom tabs | `UITabBarController` | `BottomNavigationView` |
| `Application.PresentAsync()` | Modal overlay | `PresentViewController()` | `DialogFragment` |

> These are **views**, not pages — they compose with existing Spice views like any other layout.
> `Application.Main` swapping still works; these are additive.

## NavigationView

```csharp
public class App : Application
{
    public App()
    {
        Main = new NavigationView(new HomeView());
    }
}

public class HomeView : StackLayout
{
    public HomeView()
    {
        Title = "Home";
        Add(new Label { Text = "Welcome!" });
        Add(new Button
        {
            Text = "Details",
            Clicked = _ => Navigation!.Push(new DetailView())
        });
    }
}

public class DetailView : StackLayout
{
    public DetailView()
    {
        Title = "Details";
        Add(new Label { Text = "Detail content" });
    }
    // Back button is automatic — no code needed
}
```

### Proposed API

```csharp
// Added to View base class
public partial class View
{
    [ObservableProperty] string _title = "";
    public NavigationView? Navigation { get; internal set; }
}

public partial class NavigationView : View
{
    public NavigationView(View root);
    public void Push(View view);
    public void Pop();
    public void PopToRoot();
}
```

`Push`/`Pop` are synchronous — matches Spice's `Action<T>` callback pattern. The native platform handles animations.

`Title` is added to `View` so any view can set a nav bar title. Ignored when not inside a `NavigationView`.

`Navigation` is set on child views by `NavigationView` when they're pushed, similar to how layouts already manage their `Children`.

## TabView

```csharp
Main = new TabView
{
    new Tab("Home", "home.png", new HomeView()),
    new Tab("Search", "search.png", new SearchView()),
    new Tab("Profile", "profile.png", new ProfileView()),
};
```

Each tab can contain a `NavigationView` for independent push/pop stacks:

```csharp
Main = new TabView
{
    new Tab("Home", "home.png", new NavigationView(new HomeView())),
    new Tab("Search", "search.png", new NavigationView(new SearchView())),
};
```

### Proposed API

```csharp
public partial class TabView : View
{
    public void Add(Tab tab);  // collection initializer support
}

public partial class Tab : View
{
    [ObservableProperty] string _icon = "";

    public Tab(string title, string icon, View content);
}
```

`Tab` reuses `View.Title` for the tab label and `View.Children` for its content.

## Modal Presentation

```csharp
// Present
await Application.Current.PresentAsync(new LoginView());

// Dismiss (from inside the modal)
await Application.Current.DismissAsync();
```

### Proposed API

```csharp
public partial class Application : View
{
    public Task PresentAsync(View view);
    public Task DismissAsync();
}
```

Modals are async because the caller often needs to know when presentation/dismissal completes (e.g., to read a result). `Push`/`Pop` don't need this — you fire and forget.

## Complete Example

```csharp
public class App : Application
{
    public App()
    {
        Main = new TabView
        {
            new Tab("Feed", "feed.png", new NavigationView(new FeedView())),
            new Tab("Profile", "profile.png", new ProfileView()),
        };
    }
}

public class FeedView : StackLayout
{
    public FeedView()
    {
        Title = "Feed";
        Add(new Button
        {
            Text = "View Post",
            Clicked = _ => Navigation!.Push(new PostView())
        });
        Add(new Button
        {
            Text = "New Post",
            Clicked = async _ => await Application.Current.PresentAsync(new NewPostView())
        });
    }
}

public class PostView : StackLayout
{
    public PostView()
    {
        Title = "Post";
        Add(new Label { Text = "Post content..." });
        Add(new Button
        {
            Text = "Comments",
            Clicked = _ => Navigation!.Push(new CommentsView())
        });
    }
}

public class NewPostView : StackLayout
{
    public NewPostView()
    {
        Title = "New Post";
        Add(new Entry { Placeholder = "What's on your mind?" });
        Add(new Button
        {
            Text = "Post",
            Clicked = async _ => await Application.Current.DismissAsync()
        });
    }
}
```

## Migration

```csharp
// Before — swap the whole view tree
new Button { Clicked = _ => Main = new DetailView() }

// After — push onto the stack, get a nav bar + back button for free
new Button { Clicked = _ => Navigation!.Push(new DetailView()) }
```

## Platform Mapping

### iOS

| Spice | iOS |
|-------|-----|
| `NavigationView` | `UINavigationController` |
| `Push()` | `pushViewController(_:animated:)` |
| `Pop()` | `popViewController(animated:)` |
| `PopToRoot()` | `popToRootViewController(animated:)` |
| `TabView` | `UITabBarController` |
| `Tab` | `UITab` / tab bar item |
| `PresentAsync()` | `present(_:animated:completion:)` |
| `DismissAsync()` | `dismiss(animated:completion:)` |

### Android

| Spice | Android |
|-------|---------|
| `NavigationView` | `NavController` + `NavHostFragment` |
| `Push()` | `navigate()` |
| `Pop()` | `navigateUp()` |
| `PopToRoot()` | `popBackStack(startDest)` |
| `TabView` | `BottomNavigationView` |
| `Tab` | Menu item |
| `PresentAsync()` | `DialogFragment.show()` |
| `DismissAsync()` | `DialogFragment.dismiss()` |

## Design Decisions

**Why "views" not "pages"?** Spice doesn't have pages. `NavigationView` and `TabView` are views — they inherit from `View`, use `[ObservableProperty]`, and compose like `StackLayout` or `Grid`.

**Why sync Push/Pop but async modals?** Push/pop match Spice's `Action<T>` event pattern — no `async void` needed in `Clicked` handlers. Modals are async because callers often await the result.

**Why `Title` on View?** It's one `[ObservableProperty]` field. Ignored unless the view is inside a `NavigationView` or `Tab`. Simpler than a separate "page" abstraction.

**What about `Application.Main` swapping?** Still works. Use `NavigationView`/`TabView` when you want native navigation chrome (back button, nav bar, tab bar). Use `Main =` when you don't.
