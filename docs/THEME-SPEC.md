# Themes in Spice 🌶

**Status:** Design Specification  
**Created:** February 2026

## Overview

Spice views support color properties (`TextColor`, `BackgroundColor`, `Stroke`, etc.) but
there is no built-in way to define a palette of colors and auto-apply them across every view
in the tree. Developers must set colors on each control individually, and switching between
light and dark mode means touching every property by hand.

This spec proposes a simple `Theme` class — a plain C# object with well-known color
properties — and a mechanism for views to consume those colors automatically, update
live when the theme changes, and still allow per-view overrides. The entire design is
**NativeAOT safe, trimmer safe, and reflection-free**.

## What We Have Today

Colors are set per-control. There's no shared palette:

```csharp
public class App : Application
{
    public App()
    {
        Main = new StackLayout
        {
            BackgroundColor = Colors.White,
            new Label  { Text = "Hello", TextColor = Colors.Black },
            new Button { Text = "Tap me", TextColor = Colors.White, BackgroundColor = Colors.Blue },
        };
    }
}
```

Switching to dark mode means changing every single value:

```csharp
void SwitchToDark()
{
    stackLayout.BackgroundColor = Color.FromArgb("#1E1E1E");
    label.TextColor = Colors.White;
    button.TextColor = Colors.White;
    button.BackgroundColor = Color.FromArgb("#0078D4");
    // ...repeat for every view...
}
```

This doesn't scale. We need a way to define colors once and have them flow to all views.

## Proposed: `Theme` Class

### Core API

A `Theme` is just a POCO that extends `ObservableObject` — same base as every Spice view.
It defines **semantic color slots** that map to view properties:

```csharp
namespace Spice;

/// <summary>
/// Defines a set of semantic colors that are auto-applied to views.
/// Extend ObservableObject so that changing a color fires PropertyChanged
/// and updates all subscribed views live.
/// </summary>
public partial class Theme : ObservableObject
{
    /// <summary>Default text color for Label, Button, Entry, SearchBar, etc.</summary>
    [ObservableProperty]
    Color? _textColor;

    /// <summary>Default background color for all views.</summary>
    [ObservableProperty]
    Color? _backgroundColor;

    /// <summary>Accent/tint color for interactive controls (Button background, Switch tint, ActivityIndicator, etc.)</summary>
    [ObservableProperty]
    Color? _accentColor;

    /// <summary>Border/stroke color for Border views.</summary>
    [ObservableProperty]
    Color? _strokeColor;

    /// <summary>Placeholder text color for Editor, SearchBar, etc.</summary>
    [ObservableProperty]
    Color? _placeholderColor;
}
```

No reflection, no dictionaries, no string lookups. Just typed properties on a typed object.

### Built-In Light and Dark Themes

```csharp
public partial class Theme
{
    public static Theme Light => new()
    {
        TextColor = Colors.Black,
        BackgroundColor = Colors.White,
        AccentColor = Color.FromArgb("#0078D4"),
        StrokeColor = Color.FromArgb("#E0E0E0"),
        PlaceholderColor = Colors.DarkGray,
    };

    public static Theme Dark => new()
    {
        TextColor = Colors.White,
        BackgroundColor = Color.FromArgb("#1E1E1E"),
        AccentColor = Color.FromArgb("#4CC2FF"),
        StrokeColor = Color.FromArgb("#404040"),
        PlaceholderColor = Colors.LightGray,
    };
}
```

### Setting the Theme on Application

```csharp
public partial class Application : View
{
    /// <summary>
    /// The current theme. Setting this applies colors to the entire view tree
    /// and subscribes to live updates.
    /// </summary>
    [ObservableProperty]
    Theme? _theme;
}
```

## How Theming Works

### Step 1: Each View Knows How to Apply a Theme

Every view type overrides a virtual method that maps theme color slots to its own
properties. This is the **only** connection between themes and views — no reflection,
no attribute scanning, no magic:

```csharp
// In View (base class)
public partial class View
{
    /// <summary>
    /// Applies semantic theme colors to this view. Override in subclasses
    /// to map additional theme slots to view-specific properties.
    /// Only sets properties that the developer has not explicitly set.
    /// Sets _isApplyingTheme = true so that On{Prop}Changing hooks can
    /// distinguish theme-driven changes from developer-driven changes.
    /// </summary>
    protected virtual void ApplyTheme(Theme theme)
    {
        _isApplyingTheme = true;
        _themedBackgroundColor = theme.BackgroundColor;
        if (!_isBackgroundColorSet)
            BackgroundColor = _themedBackgroundColor;
        _isApplyingTheme = false;
    }
}
```

```csharp
// In Label
public partial class Label : View
{
    protected override void ApplyTheme(Theme theme)
    {
        base.ApplyTheme(theme);
        _themedTextColor = theme.TextColor;
        if (!_isTextColorSet)
            TextColor = _themedTextColor;
    }
}
```

```csharp
// In Button
public partial class Button : View
{
    protected override void ApplyTheme(Theme theme)
    {
        base.ApplyTheme(theme);
        _themedTextColor = theme.TextColor;
        if (!_isTextColorSet)
            TextColor = _themedTextColor;
        _themedBackgroundColor = theme.AccentColor;
        if (!_isBackgroundColorSet)
            BackgroundColor = _themedBackgroundColor;
    }
}
```

```csharp
// In Border
public partial class Border : View
{
    protected override void ApplyTheme(Theme theme)
    {
        base.ApplyTheme(theme);
        _themedStroke = theme.StrokeColor;
        if (!_isStrokeSet)
            Stroke = _themedStroke;
    }
}
```

Each mapping is a single line of C# per property — explicit, debuggable, and visible in any
IDE without decompilers. Adding a new theme slot for a new control means adding one line
in its `ApplyTheme`.

### Step 2: Tracking "Developer Set" vs "Theme Set"

When a developer explicitly sets a color on a view, that value takes priority over the
theme. We track this with a simple boolean per themeable property:

```csharp
public partial class Label : View
{
    bool _isTextColorSet;
    Color? _themedTextColor;

    // The [ObservableProperty] source generator creates OnTextColorChanged.
    // We provide an additional hook:
    partial void OnTextColorChanging(Color? value)
    {
        // ApplyTheme sets _isApplyingTheme = true while pushing theme values.
        // Only changes made outside ApplyTheme count as explicit overrides.
        if (!_isApplyingTheme)
        {
            // null means "clear explicit value, fall back to theme"
            _isTextColorSet = value is not null;
        }
    }
}
```

> **Why not a separate `ThemeTextColor` property?** That doubles the API surface for every
> color property. Tracking a bool keeps the public API clean — developers still just set
> `label.TextColor = Colors.Red` and the theme won't overwrite it.

To **clear** an explicit override and revert to the theme:

```csharp
// Future: could add a helper, but for now:
label.TextColor = null; // null means "no explicit value, use theme"
```

### Step 3: Walking the View Tree

When `Application.Theme` is set or changed, walk the entire `Main` view tree and call
`ApplyTheme` on each view. This is a simple recursive traversal of `Children`:

```csharp
// In Application
partial void OnThemeChanged(Theme? oldValue, Theme? value)
{
    // Unsubscribe from old theme
    if (oldValue is not null)
        oldValue.PropertyChanged -= OnThemePropertyChanged;

    // Subscribe to new theme for live updates
    if (value is not null)
    {
        value.PropertyChanged += OnThemePropertyChanged;
        ApplyThemeToTree(Main, value);
    }
}

static void ApplyThemeToTree(View? view, Theme theme)
{
    if (view is null) return;
    view.ApplyTheme(theme);
    foreach (var child in view.Children)
        ApplyThemeToTree(child, theme);
}

void OnThemePropertyChanged(object? sender, PropertyChangedEventArgs e)
{
    // A single color in the theme changed — re-apply the whole theme
    // (theme objects are small, this is cheap)
    if (Theme is not null)
        ApplyThemeToTree(Main, Theme);
}
```

### Step 4: New Views Get the Theme Too

When a view is added to the tree (e.g., pushed onto a NavigationView, added to a
StackLayout at runtime), it should pick up the current theme. This hooks into the
existing `Children.CollectionChanged` event:

```csharp
// In View base — when children change, ask the owning Application to theme new additions
void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
{
    // Application is a reference to the owning app, set when the view is attached
    if (e.Action == NotifyCollectionChangedAction.Add && Application?.Theme is not null)
    {
        foreach (View child in e.NewItems ?? [])
            Application.ApplyThemeToTree(child, Application.Theme);
    }
    // existing platform child management code...
}
```

## Dark Mode / Light Mode

### Simple Toggle

```csharp
public class App : Application
{
    public App()
    {
        Theme = Theme.Light;

        var toggle = new Switch
        {
            Toggled = sw => Theme = sw.IsOn ? Theme.Dark : Theme.Light
        };

        Main = new StackLayout
        {
            new Label { Text = "Hello, Spice 🌶" },
            new Label { Text = "Dark Mode:" },
            toggle,
        };
    }
}
```

Flipping the `Switch` swaps the entire theme — every view in the tree updates immediately.

### Responding to System Dark Mode

Platform-specific code can detect the system appearance and set the theme accordingly:

```csharp
// iOS — in SpiceSceneDelegate or app startup
if (UITraitCollection.CurrentTraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
    app.Theme = Theme.Dark;
else
    app.Theme = Theme.Light;

// Android — in SpiceActivity
var nightMode = Resources?.Configuration?.UiMode & UiMode.NightMask;
app.Theme = nightMode == UiMode.NightYes ? Theme.Dark : Theme.Light;
```

A future enhancement could add `Application.AppearanceChanged` callback to make this
cross-platform, but the platform hooks above work today with zero new API.

## Custom Themes

### Extend the Built-In Slots

Developers can create their own themes by simply constructing `Theme` with different colors:

```csharp
var corporate = new Theme
{
    TextColor = Color.FromArgb("#333333"),
    BackgroundColor = Color.FromArgb("#F5F5F5"),
    AccentColor = Color.FromArgb("#FF6600"),    // brand orange
    StrokeColor = Color.FromArgb("#CCCCCC"),
    PlaceholderColor = Color.FromArgb("#999999"),
};
app.Theme = corporate;
```

### Add New Color Slots (Subclass)

For app-specific color slots that built-in views don't know about:

```csharp
public partial class BrandTheme : Theme
{
    [ObservableProperty]
    Color? _headerColor;

    [ObservableProperty]
    Color? _cardBackgroundColor;
}
```

Custom views consume these in their own `ApplyTheme`:

```csharp
public partial class HeaderView : Label
{
    protected override void ApplyTheme(Theme theme)
    {
        base.ApplyTheme(theme);
        if (theme is BrandTheme brand && brand.HeaderColor is not null)
            TextColor = brand.HeaderColor;
    }
}
```

## Per-View Overrides

Setting a color explicitly on a view always wins over the theme:

```csharp
app.Theme = Theme.Dark; // TextColor = White

var label = new Label { Text = "Always red", TextColor = Colors.Red };
// TextColor stays Red even though the theme says White
```

To reset a view back to the theme's color:

```csharp
label.TextColor = null; // Reverts to theme's TextColor on next theme application
```

## Design Decisions

### Why Not a Dictionary / Resource System?

Frameworks like WPF and MAUI use dictionaries (`ResourceDictionary`) where theme values are
looked up by string key at runtime. This is flexible but:

| | Dictionary (WPF/MAUI) | Typed Theme (Spice) |
|---|---|---|
| **NativeAOT safe** | ❌ Often uses reflection for type conversion | ✅ Plain properties, zero reflection |
| **Trimmer safe** | ⚠️ String keys can't be statically analyzed | ✅ Direct property access, fully trimmable |
| **Compile-time safety** | ❌ Typo in key = runtime error | ✅ Typo in property name = compile error |
| **Discoverability** | ❌ Keys are strings, need documentation | ✅ IntelliSense shows all available slots |
| **Debuggability** | ❌ Opaque dictionary lookups | ✅ Step through `ApplyTheme` line by line |

A typed `Theme` class trades some flexibility (you can't add arbitrary keys at runtime) for
compile-time safety, IntelliSense, and zero runtime overhead. This matches Spice's philosophy.

### Why Not Expression Trees / Compiled Lambdas?

Expression trees would let us infer property names automatically, but they require
`System.Linq.Expressions` which is not fully NativeAOT safe and increases binary size.
Explicit virtual methods are zero-overhead and always trimmable.

### Why `protected virtual` Instead of `public virtual`?

`ApplyTheme` is a framework implementation detail — developers don't call it directly,
they set `Application.Theme` and the framework handles the rest. Making it `protected`
keeps it out of the public API surface while still allowing external libraries and custom
controls to override it and participate in theming.

### Why Re-Apply the Entire Theme on Single-Property Changes?

When `Theme.TextColor` changes, we re-apply the full theme to the view tree instead of
only updating text colors. This keeps the logic simple — `ApplyTheme` is the single source
of truth for all theme→view mappings. Theme objects are small (a handful of color
properties), and the view tree traversal is fast (layout trees are typically shallow).

## Full Example

```csharp
public class App : Application
{
    public App()
    {
        Theme = Theme.Light;

        int count = 0;
        var label = new Label { Text = "Hello, Spice 🌶" };
        var counter = new Label { Text = "Times: 0" };
        var button = new Button
        {
            Text = "Tap me",
            Clicked = _ => counter.Text = $"Times: {++count}",
        };
        var darkModeSwitch = new Switch
        {
            Toggled = sw => Theme = sw.IsOn ? Theme.Dark : Theme.Light,
        };
        var overrideLabel = new Label
        {
            Text = "I'm always red",
            TextColor = Colors.Red, // explicit override — theme won't touch this
        };

        Main = new StackLayout
        {
            label,
            counter,
            button,
            new StackLayout
            {
                Orientation = Orientation.Horizontal,
                new Label { Text = "Dark Mode:" },
                darkModeSwitch,
            },
            overrideLabel,
        };
    }
}
```

Toggle the switch → every view updates to dark colors instantly, except the red label which
keeps its explicit color.

## Implementation Roadmap

### Phase 1: Core Theme Class
- Add `Theme` class with semantic color properties
- Add `Application.Theme` property
- Implement `ApplyTheme` virtual method on `View` and all built-in views
- Implement tree walking in `Application.OnThemeChanged`
- Unit tests for theme application (POCOs on `net10.0`, no device needed)

### Phase 2: Developer Override Tracking
- Add `_is{Prop}Set` booleans per themeable property
- Hook `On{Prop}Changing` partials to detect explicit sets
- Ensure `null` assignment resets to theme value

### Phase 3: Dynamic View Addition
- Hook `Children.CollectionChanged` to apply theme to newly added views
- Cover NavigationView push, TabView tab switching, CollectionView cell creation

### Phase 4: System Appearance Detection
- Optional `Application.AppearanceChanged` callback
- Platform implementations to detect iOS `traitCollectionDidChange` and Android `uiMode`
- Auto-switch between `Theme.Light` and `Theme.Dark` when system appearance changes

## Summary

- **`Theme`** is a POCO extending `ObservableObject` — just typed color properties, no reflection
- **`Application.Theme`** sets the active theme and walks the view tree
- **Live updates** — change a theme property or swap the entire theme, views update immediately
- **Explicit overrides win** — set `TextColor = Colors.Red` and the theme won't touch it
- **Dark/Light mode** — swap `Theme.Light` ↔ `Theme.Dark` with one assignment
- **Custom themes** — subclass `Theme` or just construct one with your own colors
- **NativeAOT safe** — zero reflection, fully trimmable, compile-time type safety
