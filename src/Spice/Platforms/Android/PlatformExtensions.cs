namespace Spice;

/// <summary>
/// Extension methods for Android
/// </summary>
public static class PlatformExtensions
{
	static readonly Lazy<float> Density = new(() => Platform.Context.Resources!.DisplayMetrics!.Density);

	/// <summary>
	/// Convert DP to Pixels
	/// </summary>
	/// <param name="value">DP value</param>
	/// <returns>Pixel value</returns>
	public static int ToPixels(this int value) => (int)Math.Round(value * Density.Value);

	/// <summary>
	/// Convert DP to Pixels
	/// </summary>
	/// <param name="value">DP value</param>
	/// <returns>Pixel value</returns>
	public static int ToPixels(this double value) => (int)Math.Round(value * Density.Value);

	/// <summary>
	/// Converts a Maui.Graphics.Color to Android int representation of a color
	/// </summary>
	/// <param name="color">The Maui.Graphics.Color</param>
	/// <returns>Android int representation of a color</returns>
	public static int ToAndroidInt(this Color? color) => color.ToAndroidColor();

	/// <summary>
	/// Converts a Maui.Graphics.Color to Android.Graphics.Color
	/// </summary>
	/// <param name="color">The Maui.Graphics.Color</param>
	/// <returns>The Android.Graphics.Color</returns>
	public static Android.Graphics.Color ToAndroidColor(this Color? color)
	{
		if (color == null)
			return Android.Graphics.Color.Transparent;

		int r = (int)Math.Round(color.Red * byte.MaxValue);
		int g = (int)Math.Round(color.Green * byte.MaxValue);
		int b = (int)Math.Round(color.Blue * byte.MaxValue);
		int a = (int)Math.Round(color.Alpha * byte.MaxValue);
		return new Android.Graphics.Color(r, g, b, a);
	}
}