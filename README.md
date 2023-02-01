# Spice 🌶, a spicy cross-platform UI framework!

In reviewing, many of the *cool* UI frameworks for mobile:

* [Flutter](https://flutter.dev)
* [SwiftUI](https://developer.apple.com/xcode/swiftui/)
* [Jetpack Compose](https://developer.android.com/jetpack/compose)
* [Fabulous](https://fabulous.dev/)
* [Comet](https://github.com/dotnet/Comet)
* An, of course, [.NET MAUI](https://dotnet.microsoft.com/apps/maui)!

Looking at what apps look like today -- it seems like bunch of
rigamarole to me. Can we build mobile applications *without* design
patterns?

The idea is we could build apps in a simple way, in a similar vein as
[minimal APIs in ASP.NET Core][minimal-apis] but for mobile & maybe
one day desktop:

```csharp
public class App : Application
{
    public App()
    {
        int count = 0;
    
        var label = new Label
        {
            Text = "Hello, Spice! 🌶",
        };
    
        var button = new Button
        {
            Text = "Click Me",
            Clicked = _ => label.Text = $"Times: {++count}"
        };
    
        Main = new StackView { label, button };
    }
}
```

These "view" types are mostly just [POCOs][poco].

Thus you can easily write unit tests in a vanilla `net7.0` Xunit
project, such as:

```csharp
[Fact]
public void Application()
{
    var app = new App();
    Assert.Equal(2, app.Main.Children.Count);

    var label = app.Main.Children[0] as Label;
    Assert.NotNull(label);
    var button = app.Main.Children[1] as Button;
    Assert.NotNull(button);

    button.Clicked(button);
    Assert.Equal("Times: 1", label.Text);

    button.Clicked(button);
    Assert.Equal("Times: 2", label.Text);
}
```

The above views in a `net7.0` project are not real UI, while
`net7.0-android` and `net7.0-ios` projects get the full
implementations that actually *do* something on screen.

[poco]: https://en.wikipedia.org/wiki/Plain_old_CLR_object
[minimal-apis]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis

## Scope

* No XAML. No DI. No MVVM. No MVC. No data-binding. No System.Reflection.
* Target iOS & Android only to start.
* Implement only the simplest controls.
* The native platforms do their own layout.
* Document how to author custom controls.
* Leverage C# Hot Reload for fast development.
* Measure startup time & app size.
* Profit?

Some benefits of this approach are full support for trimming and
eventually [NativeAOT][nativeaot] if it comes to mobile one day. 😉

[nativeaot]: https://learn.microsoft.com/dotnet/core/deploying/native-aot/

## Thoughts on .NET MAUI

.NET MAUI is great. XAML is great. Think of this idea as a "mini"
MAUI.

Spice will even leverage various parts of .NET MAUI:

* The iOS and Android workloads for .NET.
* The .NET MAUI "Single Project" system.
* The .NET MAUI "Asset" system, aka Resizetizer.
* Microsoft.Maui.Graphics.

And, of course, you should be able to use Microsoft.Maui.Essentials by
opting in with `UseMauiEssentials=true`.
