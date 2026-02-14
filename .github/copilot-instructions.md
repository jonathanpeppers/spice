# Spice 🌶 — Copilot Instructions

## What This Project Is

Spice is a **minimal cross-platform mobile UI framework** for .NET (iOS + Android). It intentionally avoids `Microsoft.Maui.Controls` — no XAML, no DI, no MVVM, no data-binding, no `System.Reflection`. Views are POCOs that map directly to native platform controls. It uses only `Microsoft.Maui.Graphics` (for `Color`) and the MAUI Single Project + Resizetizer asset system.

## Architecture: The Partial Class Pattern

Every view is split across three files — this is the core pattern:

| Location | Role |
|---|---|
| `src/Spice/Core/{Type}.cs` | Cross-platform partial class. Defines properties via `[ObservableProperty]` (CommunityToolkit.Mvvm). Compiles on all TFMs including bare `net10.0`. |
| `src/Spice/Platforms/iOS/{Type}.cs` | iOS partial — constructor with `UIView` factory, `NativeView` property, `partial void On{Prop}Changed` pushing to UIKit. |
| `src/Spice/Platforms/Android/{Type}.cs` | Android partial — constructor with `Context`-based factory, `NativeView` property, `partial void On{Prop}Changed` pushing to Android widgets. |

Platform files are excluded from non-matching TFMs by SingleProject logic inlined in `src/Spice/MSBuild/Spice.targets`.

When adding a new control, follow the `Image` or `Label` pattern: core file with `[ObservableProperty]`, plus platform partials implementing `On{Prop}Changed`.

## Key Conventions

- **Namespace**: Everything lives in the `Spice` namespace — no sub-namespaces.
- **Properties**: Use private `_camelCase` backing fields with `[ObservableProperty]`. The toolkit generates public `PascalCase` properties and `On{Prop}Changed`/`On{Prop}Changing` partial methods.
- **Implicit operators**: Each platform partial declares `public static implicit operator NativeType(SpiceType)` for seamless native interop.
- **Lazy native views**: `protected readonly Lazy<T> _nativeView` — native views are created on first access via a factory function.
- **XML doc comments**: Required on all public types and members (`GenerateDocumentationFile=true`). Include platform mappings, e.g., `/// <summary>Maps to Android.Widget.TextView / UIKit.UILabel</summary>`.
- **GlobalUsings** (`src/Spice/GlobalUsings.cs`): `CommunityToolkit.Mvvm.ComponentModel` and `Color = Microsoft.Maui.Graphics.Color`.
- **`VANILLA` define**: Set when building for base `net10.0` (no platform). Use `#if !VANILLA` sparingly; prefer partial classes.
- **Blazor**: Separate `Blazor/` folders at library and platform levels. `BlazorWebView` extends `WebView`.

## Project Structure

```
src/Spice/              → Main library (net10.0;net10.0-android;net10.0-ios)
  Core/                 → Cross-platform view definitions
  Platforms/iOS/        → iOS partials + SpiceAppDelegate/SpiceSceneDelegate
  Platforms/Android/    → Android partials + SpiceActivity
  Blazor/               → Cross-platform Blazor support
  MSBuild/              → Spice.props (capabilities), Spice.targets (SingleProject logic)
samples/                → Spice.Scenarios, Spice.BlazorSample, HeadToHead comparisons
tests/Spice.Tests/      → xUnit tests targeting net10.0 (POCO-based, no platform needed)
sizes/                  → APK size baseline files (.apkdesc) for CI regression checks
src/Spice.Templates/    → dotnet new templates (spice, spice-blazor)
```

## Build & Test

```sh
dotnet build                                    # Build all (requires maui-android + maui-ios workloads)
dotnet build -f net10.0-android -t:Run          # Run on Android
dotnet build -f net10.0-ios -t:Run              # Run on iOS
dotnet test tests/Spice.Tests/Spice.Tests.csproj  # Run unit tests (net10.0 only, no device needed)
dotnet pack                                     # Create NuGet packages
```

Tests exercise views as plain C# objects — instantiate `Application`, access `Children`, invoke `Clicked` delegates, assert property changes. No mocking or platform emulation needed.

## CI (GitHub Actions)

Runs on `macos-latest`. Key steps: build → apkdiff size regression check → test → pack. APK baselines in `sizes/*.apkdesc` are compared with `apkdiff` tool (3% APK / 5% content regression thresholds). Update baselines by building Release Android APKs and running `apkdiff -s --save-description-2=sizes/{name}.apkdesc`.

## iOS Entry Point

Uses **scene-based lifecycle**: `SpiceAppDelegate<TApp>` + `SpiceSceneDelegate<TApp>`. Consumer AppDelegates are one-liners: `public class AppDelegate : SpiceAppDelegate<App> { }`. All `Info.plist` files must include `UIApplicationSceneManifest`. Access the window via `Platform.Window`.

## Android Entry Point

`SpiceActivity` extends `AppCompatActivity`. Sets `Platform.Context`. Consumers subclass and call `SetContentView(new App())` in `OnCreate`.

## MSBuild: How Spice.props/targets Work

These ship in the NuGet and are imported by consumer projects. **Spice.targets** inlines Microsoft.Maui's SingleProject MSBuild logic (since Spice doesn't reference MAUI Controls) — it manages `Platforms/` folder conventions, excludes non-current-platform files from compilation, and adds IDE capabilities. Do not reference `Microsoft.Maui.Controls` — this is intentional.
