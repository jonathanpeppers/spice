# Themes in Spice 🌶

**Status:** Implemented  
**Created:** February 2026

## Overview

Spice provides a built-in `Theme` class — a plain C# object with well-known color
properties — and a mechanism for views to consume those colors automatically, update
live when the theme changes, and still allow per-view overrides. The entire design is
**NativeAOT safe, trimmer safe, and reflection-free**.

## `Theme` Class

### Core API

A `Theme` is just a POCO that extends `ObservableObject` — same base as every Spice view.
It defines **semantic color slots** that map to view properties:

```csharp
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

The built-in themes use float constructors instead of `Colors` static properties to avoid
pulling `Microsoft.Maui.Graphics` references into the AOT-compiled output, keeping APK size
down:

```csharp
public partial class Theme
{
    public static Theme Light => new()
    {
        TextColor = Black,                                   // #000000
        BackgroundColor = White,                             // #FFFFFF
        AccentColor = new Color(0f, 0.471f, 0.831f),        // #0078D4
        StrokeColor = new Color(0.878f, 0.878f, 0.878f),    // #E0E0E0
        PlaceholderColor = DarkGray,                         // #A9A9A9
    };

    public static Theme Dark => new()
    {
        TextColor = White,                                    // #FFFFFF
        BackgroundColor = new Color(0.118f, 0.118f, 0.118f),  // #1E1E1E
        AccentColor = new Color(0.298f, 0.761f, 1f),          // #4CC2FF
        StrokeColor = new Color(0.251f, 0.251f, 0.251f),      // #404040
        PlaceholderColor = LightGray,                          // #D3D3D3
    };
}
```

### Setting the Theme on Application

```csharp
public partial class Application : View
{
    /// <summary>
    /// The current theme. Setting this applies colors to the entire view tree
    /// and subscribes to live updates. Null means no theme — views keep their
    /// individually-set colors (backward compatible default).
    /// </summary>
    [ObservableProperty]
    Theme? _theme;
}
```

> **Why nullable?** Theming is opt-in. Existing apps that never set `Theme` continue
> working exactly as before — no colors change, no behavior changes. Apps opt in with
> a single line: `Theme = Theme.Light;`

## How Theming Works

### Step 1: Each View Knows How to Apply a Theme

Every view type overrides a `protected virtual` method that maps theme color slots to its
own properties. This is the **only** connection between themes and views — no reflection,
no attribute scanning, no magic.

The base `View` class applies `BackgroundColor`:

```csharp
// In View (base class)
protected virtual void ApplyTheme(Theme theme)
{
    if (CanApplyTheme((int)ThemeProperty.BackgroundColor))
        BackgroundColor = theme.BackgroundColor;
}
```

Subclasses override to add their own mappings:

```csharp
// In Label
protected override void ApplyTheme(Theme theme)
{
    base.ApplyTheme(theme);
    if (CanApplyTheme((int)ThemeProperty.TextColor))
        TextColor = theme.TextColor;
}
```

```csharp
// In Button — uses AccentColor for its background
protected override void ApplyTheme(Theme theme)
{
    base.ApplyTheme(theme);
    if (CanApplyTheme((int)ThemeProperty.TextColor))
        TextColor = theme.TextColor;
    if (CanApplyTheme((int)ThemeProperty.BackgroundColor))
        BackgroundColor = theme.AccentColor;
}
```

```csharp
// In Border
protected override void ApplyTheme(Theme theme)
{
    base.ApplyTheme(theme);
    if (CanApplyTheme((int)ThemeProperty.Stroke))
        Stroke = theme.StrokeColor;
}
```

```csharp
// In Editor — text and placeholder colors
protected override void ApplyTheme(Theme theme)
{
    base.ApplyTheme(theme);
    if (CanApplyTheme((int)ThemeProperty.TextColor))
        TextColor = theme.TextColor;
    if (CanApplyTheme((int)ThemeProperty.PlaceholderColor))
        PlaceholderColor = theme.PlaceholderColor;
}
```

```csharp
// In ActivityIndicator — accent color
protected override void ApplyTheme(Theme theme)
{
    base.ApplyTheme(theme);
    if (CanApplyTheme((int)ThemeProperty.Color))
        Color = theme.AccentColor;
}
```

#### Complete Theme Mapping Table

| View | Theme Slot → Property |
|---|---|
| **View** (base) | `BackgroundColor` → `BackgroundColor` |
| **Label** | `TextColor` → `TextColor` |
| **Button** | `TextColor` → `TextColor`, `AccentColor` → `BackgroundColor` |
| **Entry** | `TextColor` → `TextColor` |
| **Editor** | `TextColor` → `TextColor`, `PlaceholderColor` → `PlaceholderColor` |
| **SearchBar** | `TextColor` → `TextColor`, `PlaceholderColor` → `PlaceholderColor` |
| **DatePicker** | `TextColor` → `TextColor` |
| **Picker** | `TextColor` → `TextColor` |
| **Border** | `StrokeColor` → `Stroke` |
| **ActivityIndicator** | `AccentColor` → `Color` |

Views that only inherit the base `BackgroundColor` mapping (no override): Image, ImageButton,
Switch, Slider, ProgressBar, WebView, ScrollView, StackLayout, ContentView, Grid, BoxView,
CheckBox, RadioButton, TimePicker.

### Step 2: Tracking "Developer Set" vs "Theme Set"

When a developer explicitly sets a color on a view, that value takes priority over the
theme. This is tracked with a bitmask (`_explicitProps`) and a `ThemeProperty` flags enum:

```csharp
public partial class View
{
    /// <summary>
    /// Built-in theme property flags. An Int32 supports up to 32 properties.
    /// Custom views can define additional flags starting at 1 << 5.
    /// </summary>
    [Flags]
    public enum ThemeProperty
    {
        None            = 0,
        BackgroundColor = 1 << 0,
        TextColor       = 1 << 1,
        PlaceholderColor= 1 << 2,
        Stroke          = 1 << 3,
        Color           = 1 << 4,
    }

    bool _isApplyingTheme;
    int _explicitProps;

    /// <summary>
    /// Tracks whether a theme property was explicitly set by the developer.
    /// When value is non-null the flag is set; when null it is cleared.
    /// </summary>
    public void TrackExplicit(int property, object? value)
    {
        if (!_isApplyingTheme)
        {
            if (value is not null)
                _explicitProps |= property;
            else
                _explicitProps &= ~property;
        }
    }

    /// <summary>
    /// Returns true when the theme property has not been explicitly set.
    /// </summary>
    public bool CanApplyTheme(int property) => (_explicitProps & property) == 0;
}
```

Each view hooks its `On{Prop}Changing` partial to call `TrackExplicit`:

```csharp
// In View
partial void OnBackgroundColorChanging(Color? value) =>
    TrackExplicit((int)ThemeProperty.BackgroundColor, value);

// In Label
partial void OnTextColorChanging(Color? value) =>
    TrackExplicit((int)ThemeProperty.TextColor, value);

// In Border
partial void OnStrokeChanging(Color? value) =>
    TrackExplicit((int)ThemeProperty.Stroke, value);
```

> **Why a bitmask instead of per-property booleans?** A single `int` tracks up to 32
> properties with no per-field memory overhead. The `ThemeProperty` enum provides
> compile-time safety for the flag values.

To **clear** an explicit override and revert to the theme:

```csharp
label.TextColor = null; // clears the flag, theme value applies on next theme application
```

### Step 3: Walking the View Tree

When `Application.Theme` is set or changed, the entire `Main` view tree is walked via
`ApplyThemeToTree` (an `internal static` method on `View`). Each view is themed through
`ApplyThemeInternal`, which manages the `_isApplyingTheme` flag, stores the applied theme
for dynamic children, and delegates to the virtual `ApplyTheme`:

```csharp
// In View
internal void ApplyThemeInternal(Theme theme)
{
    _isApplyingTheme = true;
    _appliedTheme = theme;

    if (!_themeChildrenSubscribed)
    {
        _themeChildrenSubscribed = true;
        Children.CollectionChanged += OnThemeChildrenChanged;
    }

    ApplyTheme(theme);
    _isApplyingTheme = false;
}

internal static void ApplyThemeToTree(View? view, Theme theme)
{
    if (view is null) return;
    view.ApplyThemeInternal(theme);
    foreach (var child in view.Children)
        ApplyThemeToTree(child, theme);
}
```

In `Application`, theme changes subscribe to `PropertyChanged` for live updates. Setting
`Main` after a theme also applies the theme to the new tree. Explicitly setting `Theme`
disables `UseSystemTheme`:

```csharp
// In Application
partial void OnThemeChanging(Theme? value)
{
    if (!_isSettingSystemTheme)
        UseSystemTheme = false;
}

partial void OnThemeChanged(Theme? oldValue, Theme? newValue)
{
    if (oldValue is not null)
        oldValue.PropertyChanged -= OnThemePropertyChanged;

    if (newValue is not null)
    {
        newValue.PropertyChanged += OnThemePropertyChanged;
        ApplyThemeToTree(Main, newValue);
    }
}

partial void OnMainChanged(View? oldValue, View? newValue)
{
    if (newValue is not null && Theme is not null)
        ApplyThemeToTree(newValue, Theme);
}

void OnThemePropertyChanged(object? sender, PropertyChangedEventArgs e)
{
    if (Theme is not null)
        ApplyThemeToTree(Main, Theme);
}
```

### Step 4: New Views Get the Theme Too

When a view is added to the tree at runtime, it picks up the current theme. Each
view lazily subscribes to its own `Children.CollectionChanged` the first time
`ApplyThemeInternal` is called, and themes any newly added children:

```csharp
// In View — registered lazily inside ApplyThemeInternal
void OnThemeChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
{
    if (e.NewItems is not null && _appliedTheme is not null)
    {
        foreach (View child in e.NewItems)
            ApplyThemeToTree(child, _appliedTheme);
    }
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

### Automatic System Appearance Detection

Spice provides `PlatformAppearance` — a cross-platform static class that exposes the
system's current appearance and a change notification:

```csharp
public static partial class PlatformAppearance
{
    /// <summary>
    /// Event raised when the system appearance changes.
    /// The bool parameter is true when the system switched to dark mode.
    /// </summary>
    public static event Action<bool>? Changed;

    /// <summary>
    /// Gets whether the system is in dark mode.
    /// </summary>
    public static bool IsDarkMode { get; }
}
```

Set `Application.UseSystemTheme = true` and Spice handles the rest:

```csharp
public class App : Application
{
    public App()
    {
        UseSystemTheme = true; // auto-selects Theme.Light or Theme.Dark based on OS

        Main = new StackLayout
        {
            new Label { Text = "Hello, Spice 🌶" },
        };
    }
}
```

When `UseSystemTheme` is enabled:
- On startup, Spice queries `PlatformAppearance.IsDarkMode` and sets `Theme` accordingly
- It subscribes to `PlatformAppearance.Changed` so that `Theme` is swapped automatically when the OS appearance changes
- Setting `Theme` explicitly disables `UseSystemTheme` (explicit wins)
- When `UseSystemTheme` is disabled, the `Changed` subscription is removed

```csharp
partial void OnUseSystemThemeChanged(bool value)
{
    if (value)
    {
        PlatformAppearance.Changed += OnPlatformAppearanceChanged;
        _isSettingSystemTheme = true;
        Theme = PlatformAppearance.IsDarkMode ? Theme.Dark : Theme.Light;
        _isSettingSystemTheme = false;
    }
    else
    {
        PlatformAppearance.Changed -= OnPlatformAppearanceChanged;
    }
}

void OnPlatformAppearanceChanged(bool isDarkMode)
{
    if (_useSystemTheme)
    {
        _isSettingSystemTheme = true;
        Theme = isDarkMode ? Theme.Dark : Theme.Light;
        _isSettingSystemTheme = false;
    }
    AppearanceChanged?.Invoke(isDarkMode);
}
```

Platform implementations behind `PlatformAppearance.IsDarkMode`:

- **iOS** — reads `UITraitCollection.CurrentTraitCollection.UserInterfaceStyle`; listens for trait changes via `RegisterForTraitChanges` (iOS 17+) or `TraitCollectionDidChange`.
- **Android** — reads `UiMode.NightMask` from `Resources.Configuration`; listens for configuration changes in `SpiceActivity.OnConfigurationChanged`.

For fully custom themes that still track the OS mode, use the `AppearanceChanged` callback:

```csharp
AppearanceChanged = isDark => Theme = isDark ? myDarkTheme : myLightTheme;
```

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

### Why a Bitmask for Override Tracking?

A single `int _explicitProps` field tracks up to 32 themeable properties with zero
per-field memory overhead. The `ThemeProperty` flags enum provides compile-time safety
for the bit positions. Custom views can define additional flags starting at `1 << 5`.

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

## Summary

- **`Theme`** is a POCO extending `ObservableObject` — just typed color properties, no reflection
- **`Application.Theme`** sets the active theme and walks the view tree
- **Live updates** — change a theme property or swap the entire theme, views update immediately
- **Explicit overrides win** — set `TextColor = Colors.Red` and the theme won't touch it; set to `null` to revert
- **Bitmask tracking** — `ThemeProperty` flags + `_explicitProps` track developer-set vs theme-set properties
- **Dynamic children** — views added to the tree at runtime automatically receive the current theme
- **Dark/Light mode** — `UseSystemTheme = true` auto-detects OS appearance; or swap manually with one assignment
- **Custom themes** — subclass `Theme` or just construct one with your own colors
- **NativeAOT safe** — zero reflection, fully trimmable, compile-time type safety
