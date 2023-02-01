namespace Spice;

public static class PlatformExtensions
{
	static readonly Lazy<float> Density = new(() => Platform.Context.Resources!.DisplayMetrics!.Density);

	public static int ToPixels(this int value) => (int)Math.Round(value * Density.Value);

	public static int ToAndroidInt(this Color? color) => color.ToAndroidColor();

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