namespace Spice;

public partial class ActivityIndicator
{
	/// <summary>
	/// Returns activityIndicator.NativeView
	/// </summary>
	/// <param name="activityIndicator">The Spice.ActivityIndicator</param>
	public static implicit operator UIActivityIndicatorView(ActivityIndicator activityIndicator) => activityIndicator.NativeView;

	/// <summary>
	/// Represents a loading spinner on screen. Set the IsRunning property to true to show the spinner.
	/// Android -> Android.Widget.ProgressBar (indeterminate)
	/// iOS -> UIKit.UIActivityIndicatorView
	/// </summary>
	public ActivityIndicator() : base(_ => new UIActivityIndicatorView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ActivityIndicator(CGRect frame) : base(_ => new UIActivityIndicatorView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ActivityIndicator(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIActivityIndicatorView
	/// </summary>
	public new UIActivityIndicatorView NativeView => (UIActivityIndicatorView)_nativeView.Value;

	partial void OnIsRunningChanged(bool value)
	{
		if (value)
		{
			NativeView.StartAnimating();
		}
		else
		{
			NativeView.StopAnimating();
		}
	}

	partial void OnColorChanged(Color? value) => NativeView.Color = value.ToUIColor();
}
