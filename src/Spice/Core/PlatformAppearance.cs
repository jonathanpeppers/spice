namespace Spice;

/// <summary>
/// Provides cross-platform access to the system's appearance (light/dark mode).
/// Platform implementations provide the actual detection logic.
/// </summary>
public static partial class PlatformAppearance
{
	/// <summary>
	/// Event raised when the system appearance changes.
	/// The bool parameter is true when the system switched to dark mode.
	/// </summary>
	public static event Action<bool>? Changed;

	internal static void OnChanged(bool isDarkMode) => Changed?.Invoke(isDarkMode);

#if VANILLA
	/// <summary>
	/// Gets whether the system is in dark mode. Always false on net10.0 (no device).
	/// </summary>
	public static bool IsDarkMode => false;
#endif
}
