using System.Diagnostics;

namespace Spice;

/// <summary>
/// Spice extension methods for iOS
/// </summary>
public static class PlatformExtensions
{
	/// <summary>
	/// Convert a Microsoft.Maui.Graphics.Color to a UIColor
	/// </summary>
	/// <param name="color">the Microsoft.Maui.Graphics.Color</param>
	/// <returns>A UIColor or null if null was passed in</returns>
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

	/// <summary>
	/// Calls [UIView recursiveDescription], an internal iOS API for debugging views
	/// </summary>
	/// <param name="view">The UIView to dump</param>
	[Conditional("DEBUG")]
	public static void DumpHierarchy(this UIView? view)
	{
		if (view == null)
			return;
		var selector = new ObjCRuntime.Selector("recursiveDescription");
		var result = view.PerformSelector(selector);
		Debug.WriteLine(result);
	}

	/// <summary>
	/// Convert a DateTime to an NSDate
	/// </summary>
	/// <param name="dateTime">The DateTime to convert</param>
	/// <returns>An NSDate</returns>
	public static NSDate ToNSDate(this DateTime dateTime)
	{
		var reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return NSDate.FromTimeIntervalSinceReferenceDate((dateTime.ToUniversalTime() - reference).TotalSeconds);
	}

	/// <summary>
	/// Convert an NSDate to a DateTime
	/// </summary>
	/// <param name="nsDate">The NSDate to convert</param>
	/// <returns>A DateTime</returns>
	public static DateTime ToDateTime(this NSDate nsDate)
	{
		var reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return reference.AddSeconds(nsDate.SecondsSinceReferenceDate).ToLocalTime();
	}
}