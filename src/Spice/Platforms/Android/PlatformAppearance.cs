using Android.Content.Res;

namespace Spice;

public static partial class PlatformAppearance
{
	/// <summary>
	/// Gets whether the system is in dark mode on Android.
	/// </summary>
	public static bool IsDarkMode
	{
		get
		{
			var nightMode = Platform.Context.Resources?.Configuration?.UiMode & UiMode.NightMask;
			return nightMode == UiMode.NightYes;
		}
	}
}
