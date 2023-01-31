namespace Spice;

public static class PlatformExtensions
{
	/// <summary>
	/// TODO: when reducing JNI calls, this should create int instead
	/// </summary>
	public static UIColor? ToUIColor(this Color? color)
	{
		if (color == null)
			return null;

		int r = (int)Math.Round(color.Red * byte.MaxValue);
		int g = (int)Math.Round(color.Green * byte.MaxValue);
		int b = (int)Math.Round(color.Blue * byte.MaxValue);
		int a = (int)Math.Round(color.Alpha * byte.MaxValue);
		return UIColor.FromRGBA(r, g, b, a);
	}
}