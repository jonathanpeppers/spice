# Spice 🌶, a spicy cross-platform UI framework!

A prototype (and design) of API minimalism for mobile.

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
            Text = "Hello, Spice 🌶",
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
    var label = (Label)app.Main.Children[0];
    var button = (Button)app.Main.Children[1];

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
protected override void OnCreate(Bundle? savedInstanceState)
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
matched with regular iOS & Android UI because Spice 🌶 views are just
native views.

[poco]: https://en.wikipedia.org/wiki/Plain_old_CLR_object
[minimal-apis]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis

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