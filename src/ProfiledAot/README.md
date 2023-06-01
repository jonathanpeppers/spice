# Profiled AOT support for Android

This is based on the NuGet package found here:

https://github.com/jonathanpeppers/Mono.Profiler.Android#usage-of-the-aot-profiler

## Updating Profiles

Build this repo with `dotnet build -c Release` and install the `spice`
templates:

```sh
dotnet new install bin/Release/Spice.Templates.*.nupkg
```

Then run the following command:

```sh
dotnet build src/ProfiledAot/build.proj -p:App=spice
```

For `spice-blazor`:

```sh
dotnet build src/ProfiledAot/build.proj -p:App=spice-blazor
```

You can also use `-r android-x64`, if you'd prefer an x86_64 emulator.

See further details about this at:

https://github.com/xamarin/xamarin-android/edit/main/src/profiled-aot/README.md