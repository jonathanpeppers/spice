namespace Spice;

public static class PlatformExtensions
{
	static readonly Lazy<float> Density = new Lazy<float>(() => Platform.Context!.Resources!.DisplayMetrics!.Density);

	public static int ToPixels(this int value) => (int)Math.Round(value * Density.Value);

	/// <summary>
	/// TODO: when reducing JNI calls, this should create int instead
	/// </summary>
	public static Android.Graphics.Color ToAndroidColor(this Color? color)
	{
		if (color == null)
			return default;

		int r = (int)Math.Round(color.Red * byte.MaxValue);
		int g = (int)Math.Round(color.Green * byte.MaxValue);
		int b = (int)Math.Round(color.Blue * byte.MaxValue);
		int a = (int)Math.Round(color.Alpha * byte.MaxValue);
		return new Android.Graphics.Color(r, g, b, a);
	}

	/// <summary>
	/// TODO: reduce JNI calls, use int
	/// </summary>
	public static Android.Graphics.Drawables.ColorDrawable? ToAndroidDrawable(this Color? color)
	{
		if (color == null)
			return null;

		return new Android.Graphics.Drawables.ColorDrawable(color.ToAndroidColor());
	}
}