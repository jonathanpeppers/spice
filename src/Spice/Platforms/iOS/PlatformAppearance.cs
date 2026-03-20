namespace Spice;

public static partial class PlatformAppearance
{
	/// <summary>
	/// Gets whether the system is in dark mode on iOS.
	/// </summary>
	public static bool IsDarkMode =>
		UITraitCollection.CurrentTraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark;
}
