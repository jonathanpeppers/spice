# Spice 🌶 — Copilot Instructions

Minimal cross-platform mobile UI framework for .NET (iOS + Android). **No** `Microsoft.Maui.Controls`, XAML, DI, MVVM, data-binding, or `System.Reflection`. Views are POCOs mapping to native controls. Uses only `Microsoft.Maui.Graphics` (`Color`) and MAUI's SingleProject + Resizetizer.

## Partial Class Pattern (Core Architecture)

Each view = 3 files: `Core/{Type}.cs` (cross-platform, `[ObservableProperty]`), `Platforms/iOS/{Type}.cs` (UIKit), `Platforms/Android/{Type}.cs` (Android widgets). Platform partials implement `partial void On{Prop}Changed`. Platform files excluded from other TFMs via `Spice.targets`. Follow `Image`/`Label` as templates for new controls.

## Conventions

- **Namespace**: `Spice` only — no sub-namespaces
- **Properties**: `[ObservableProperty]` on `_camelCase` fields → generates `PascalCase` + `On{Prop}Changed`
- **Implicit operators**: `public static implicit operator NativeType(SpiceType)` in platform partials
- **Lazy views**: `Lazy<T> _nativeView` with factory function
- **XML docs**: Required on all public API; include platform mappings (e.g. `Android.Widget.TextView / UIKit.UILabel`)
- **GlobalUsings**: `CommunityToolkit.Mvvm.ComponentModel` + `Color = Microsoft.Maui.Graphics.Color`
- **`VANILLA` define**: Active on `net10.0` only; prefer partials over `#if`
- **Blazor**: `Blazor/` folders at library+platform levels; `BlazorWebView` extends `WebView`
- **iOS memory**: NSObject subclasses must not hold strong references to other NSObjects (causes cycles the garbage collector cannot break). Use `WeakReference<T>` for any NSObject-typed field/property in an NSObject subclass. Enforced by [`MemoryAnalyzers`](https://github.com/jonathanpeppers/memory-analyzers) (MEM0001–MEM0003).

## Layout

```
src/Spice/Core/          cross-platform views
src/Spice/Platforms/     iOS + Android partials, entry points
src/Spice/Blazor/        cross-platform Blazor; platform Blazor in Platforms/*/Blazor/
src/Spice/MSBuild/       Spice.props (capabilities), Spice.targets (inlined SingleProject)
samples/                 Spice.Scenarios, Spice.BlazorSample, HeadToHead*
tests/Spice.Tests/       xUnit on net10.0 — views tested as POCOs, no device needed
sizes/                   .apkdesc baselines for CI apkdiff (10% APK / 15% content thresholds)
src/Spice.Templates/     dotnet new spice / spice-blazor
```

## Build & Test

```sh
dotnet build                                      # all TFMs (needs maui-android + maui-ios workloads)
dotnet build -f net10.0-android -t:Run             # run Android
dotnet build -f net10.0-ios -t:Run                 # run iOS
dotnet test tests/Spice.Tests/Spice.Tests.csproj   # unit tests (net10.0, no device)
```

## Entry Points

- **iOS**: Scene-based. `SpiceAppDelegate<TApp>` + `SpiceSceneDelegate<TApp>`. Consumers: `class AppDelegate : SpiceAppDelegate<App> { }`. Requires `UIApplicationSceneManifest` in Info.plist. Window via `Platform.Window`.
- **Android**: `SpiceActivity : AppCompatActivity`. Sets `Platform.Context`. Consumers call `SetContentView(new App())`.

## MSBuild

`Spice.targets` inlines MAUI's SingleProject logic (platform folder filtering, IDE capabilities) because Spice doesn't reference MAUI Controls. Do not add a `Microsoft.Maui.Controls` dependency.

## CI

`macos-latest`: build → apkdiff size check → test → pack. Update baselines: `apkdiff -s --save-description-2=sizes/{name}.apkdesc`.
