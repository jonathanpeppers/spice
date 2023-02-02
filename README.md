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

So for example, adding `App` to the screen on Android:

```csharp
protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    SetContentView(new App());
}
```

And on iOS:

```csharp
var vc = new UIViewController();
vc.View!.AddSubview(new App());
Window.RootViewController = vc;
```

`App` is a native view on both platforms. You just add it to an
existing app as you would any other control or view.

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

It is an achievement in itself that I was able to invent my own UI
framework and pick and choose the pieces of .NET MAUI that made sense
for my framework.

## Getting Started

Simply install the template:

```bash
dotnet new install Spice.Templates
```

Create the project and build it as you would for other .NET projects:

```bash
dotnet new spice
dotnet build
```

## Implemented Controls

* `View`: maps to `Android.Views.View` and `UIKit.View`.
* `Label`: maps to `Android.Widget.TextView` and `UIKit.UILabel`
* `Button`: maps to `Android.Widget.Button` and `UIKit.UIButton`
* `StackView`: maps to `Android.Widget.LinearLayout` and `UIKit.UIStackView`
* `Image`: maps to `Android.Widget.ImageView` and `UIKit.UIImageView`

## Custom Controls

Let's review an implementation for `Image`.

First, you can write the cross-platform part for a vanilla `net7.0`
class library:

```csharp
public partial class Image : View
{
    [ObservableProperty]
    string _source = "";
}
```

`[ObservableProperty]` comes from the [MVVM Community
Toolkit][observable] -- I made use of it for simplicity. It will
automatically generate various `partial` methods,
`INotifyPropertyChanged`, and a `public` property named `Source`.

We can implement the control on Android, such as:

```csharp
public partial class Image
{
    public static implicit operator ImageView(Image image) => image.NativeView;

    public Image() : base(c => new ImageView(c)) { }

    public new ImageView NativeView => (ImageView)_nativeView.Value;

    partial void OnSourceChanged(string value)
    {
        // NOTE: the real implementation is in Java for performance reasons
        var image = NativeView;
        var context = image.Context;
        int id = context!.Resources!.GetIdentifier(value, "drawable", context.PackageName);
        if (id != 0) 
        {
            image.SetImageResource(id);
        }
    }
}
```

This code takes the name of an image, and looks up a drawable with the
same name. This also leverages the .NET MAUI asset system, so a
`spice.svg` can simply be loaded via `new Image { Source = "spice" }`.

Lastly, the iOS implementation:

```csharp
public partial class Image
{
    public static implicit operator UIImageView(Image image) => image.NativeView;

    public Image() : base(_ => new UIImageView { AutoresizingMask = UIViewAutoresizing.None }) { }

    public new UIImageView NativeView => (UIImageView)_nativeView.Value;

    partial void OnSourceChanged(string value) => NativeView.Image = UIImage.FromFile($"{value}.png");
}
```

This implementation is a bit simpler, all we have to do is call
`UIImage.FromFile()` and make sure to append a `.png` file extension
that the MAUI asset system generates.

[observable]: https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/observableproperty
