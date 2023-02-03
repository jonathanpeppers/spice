# Spice 🌶, a spicy cross-platform UI framework!

A prototype (and design) of API minimalism for mobile.

If you like this idea, star for approval! Read on for details!

## Background & Motivation

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
vc.View.AddSubview(new App());
Window.RootViewController = vc;
```

`App` is a native view on both platforms. You just add it to an the
screen as you would any other control or view. This can be mix &
matched with regular iOS & Android UI because Spice views are just
native views.

[poco]: https://en.wikipedia.org/wiki/Plain_old_CLR_object
[minimal-apis]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis

## Scope

* No XAML. No DI. No MVVM. No MVC. No data-binding. No System.Reflection.
  * *Do we need these things?*
* Target iOS & Android only to start.
* Implement only the simplest controls.
* The native platforms do their own layout.
* Document how to author custom controls.
* Leverage C# Hot Reload for fast development.
* Measure startup time & app size.
* Profit?

Benefits of this approach are full support for trimming and eventually
[NativeAOT][nativeaot] if it comes to mobile one day. 😉

[nativeaot]: https://learn.microsoft.com/dotnet/core/deploying/native-aot/

## Thoughts on .NET MAUI

.NET MAUI is great. XAML is great. Think of this idea as a "mini"
MAUI.

Spice will even leverage various parts of .NET MAUI:

* The iOS and Android workloads for .NET.
* The .NET MAUI "Single Project" system.
* The .NET MAUI "Asset" system, aka Resizetizer.
* Microsoft.Maui.Graphics for primitives like `Color`.

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

Create the project and build it as you would for other .NET MAUI
projects:

```bash
dotnet new spice
dotnet build
# To run on Android
dotnet build -f net7.0-android -t:Run
# To run on iOS
dotnet build -f net7.0-ios -t:Run
```

Of course, you can also just open the project in Visual Studio and hit F5.

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

## Hot Reload

C# Hot Reload (in Visual Studio) works fine, as it does for vanilla .NET
iOS/Android apps:

![Hot Reload Demo](docs/hotreload.gif)

Note that this only works for `Button.Clicked` because the method is
invoked when you click. If the method that was changed was already
run, *something* has to force it to run again.
[`MetadataUpdateHandler`][muh] is the solution to this problem, giving
frameworks a way to "reload themselves" for Hot Reload.

Unfortunately, [`MetadataUpdateHandler`][muh] does not currently work
for non-MAUI apps in Visual Studio 2022 17.5:

```csharp
[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(HotReload))]

static class HotReload
{
    static void UpdateApplication(Type[]? updatedTypes)
    {
        if (updatedTypes == null)
            return;
        foreach (var type in updatedTypes)
        {
            // Do something with the type
            Console.WriteLine("UpdateApplication: " + type);
        }
    }
}
```

The above code works fine in a `dotnet new maui` app, but not a
`dotnet new spice` or `dotnet new android` application.

And so we can't add proper functionality for reloading `ctor`'s of
Spice views. The general idea is we could recreate the `App` class and
replace the views on screen. We could also create Android activities
or iOS view controllers if necessary.

Hopefully, we can implement this for a future release of Visual Studio.

[muh]: https://learn.microsoft.com/dotnet/api/system.reflection.metadata.metadataupdatehandlerattribute

## Startup Time & App Size

In comparison to a `dotnet new maui` project, I created a Spice
project with the same layouts and optimized settings for both project
types. (`AndroidLinkMode=r8`, etc.)

Startup time for a `Release` build on a Pixel 5:

Spice:
```log
02-02 20:09:49.583  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +261ms
02-02 20:09:51.060  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +265ms
02-02 20:09:52.482  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +262ms
02-02 20:09:53.902  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +266ms
02-02 20:09:55.345  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +246ms
02-02 20:09:56.755  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +243ms
02-02 20:09:58.190  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +264ms
02-02 20:09:59.592  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +246ms
02-02 20:10:01.030  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +249ms
02-02 20:10:02.452  2174  2505 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +248ms
Average(ms): 255
Std Err(ms): 2.94014360949333
Std Dev(ms): 9.29755045398757
```

.NET MAUI:
```log
02-02 20:07:52.357  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +541ms
02-02 20:07:54.078  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +538ms
02-02 20:07:55.799  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +538ms
02-02 20:07:57.531  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +539ms
02-02 20:07:59.262  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +537ms
02-02 20:08:00.944  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +540ms
02-02 20:08:02.666  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +544ms
02-02 20:08:04.397  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +527ms
02-02 20:08:06.111  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +532ms
02-02 20:08:07.803  2174  2505 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +535ms
Average(ms): 537.1
Std Err(ms): 1.52351931760353
Std Dev(ms): 4.8177911028926
```

App size of a single-architecture `.apk`, built for `android-arm64`:

```
 7772202 com.companyname.HeadToHeadSpice-Signed.apk
12808825 com.companyname.HeadToHeadMaui-Signed.apk
```

This gives you an idea of how much "stuff" is in .NET MAUI.

In some respects the above comparison isn't completely fair, as Spice
has like 0 features. However, Spice is [fully trimmable][trimming],
and so a `Release` build of an app without `Spice.Button` will have
the code for `Spice.Button` trimmed away. It will be quite difficult
for .NET MAUI to become [fully trimmable][trimming] -- due to the
nature of XAML, data-binding, and other System.Reflection usage in the
framework.

[trimming]: https://learn.microsoft.com/dotnet/core/deploying/trimming/prepare-libraries-for-trimming
