using Android.Util;

namespace Spice;

public static class PlatformExtensions
{
	static readonly Lazy<float> Density = new Lazy<float>(() => Platform.Context!.Resources!.DisplayMetrics!.Density);

	public static int ToPixels(this int value) => (int)Math.Round(value * Density.Value);
}
