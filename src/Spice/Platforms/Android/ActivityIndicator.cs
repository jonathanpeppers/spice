using Android.Content;

namespace Spice;

public partial class ActivityIndicator
{
	/// <summary>
	/// Returns activityIndicator.NativeView
	/// </summary>
	/// <param name="activityIndicator">The Spice.ActivityIndicator</param>
	public static implicit operator ProgressBar(ActivityIndicator activityIndicator) => activityIndicator.NativeView;

	static ProgressBar Create(Context context)
	{
		var progressBar = new ProgressBar(context);
		progressBar.Indeterminate = true;
		return progressBar;
	}

	/// <summary>
	/// Represents a loading spinner on screen. Set the IsRunning property to true to show the spinner.
	/// Android -> Android.Widget.ProgressBar (indeterminate)
	/// iOS -> UIKit.UIActivityIndicatorView
	/// </summary>
	public ActivityIndicator() : base(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public ActivityIndicator(Context context) : base(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ActivityIndicator(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ActivityIndicator(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.ProgressBar
	/// </summary>
	public new ProgressBar NativeView => (ProgressBar)_nativeView.Value;

	partial void OnIsRunningChanged(bool value)
	{
		NativeView.Visibility = value ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
	}

	partial void OnColorChanged(Color? value)
	{
		if (value != null)
		{
			NativeView.IndeterminateDrawable?.SetTint(value.ToAndroidInt());
		}
		else
		{
			NativeView.IndeterminateDrawable?.ClearColorFilter();
		}
	}
}
