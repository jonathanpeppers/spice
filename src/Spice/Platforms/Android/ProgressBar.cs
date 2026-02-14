using Android.Content;
using AndroidProgressBar = Android.Widget.ProgressBar;

namespace Spice;

public partial class ProgressBar
{
	/// <summary>
	/// Returns progressBar.NativeView
	/// </summary>
	/// <param name="progressBar">The Spice.ProgressBar</param>
	public static implicit operator AndroidProgressBar(ProgressBar progressBar) => progressBar.NativeView;

	static AndroidProgressBar Create(Context context)
	{
		return new AndroidProgressBar(context, null, Android.Resource.Attribute.ProgressBarStyleHorizontal)
		{
			Max = 10000, // Use a large max for better precision with percentage
			Indeterminate = false
		};
	}

	/// <summary>
	/// Displays a progress indicator showing the completion percentage of a task.
	/// Android -> Android.Widget.ProgressBar
	/// iOS -> UIKit.UIProgressView
	/// </summary>
	public ProgressBar() : base(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public ProgressBar(Context context) : base(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ProgressBar(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ProgressBar(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.ProgressBar
	/// </summary>
	public new AndroidProgressBar NativeView => (AndroidProgressBar)_nativeView.Value;

	partial void OnProgressChanged(double value)
	{
		var clampedValue = Math.Clamp(value, 0.0, 1.0);
		var progress = (int)(clampedValue * NativeView.Max);
		NativeView.Progress = progress;
	}
}
