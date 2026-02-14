namespace Spice;

public partial class ProgressBar
{
	/// <summary>
	/// Returns progressBar.NativeView
	/// </summary>
	/// <param name="progressBar">The Spice.ProgressBar</param>
	public static implicit operator UIProgressView(ProgressBar progressBar) => progressBar.NativeView;

	/// <summary>
	/// Displays a progress indicator showing the completion percentage of a task.
	/// Android -> Android.Widget.ProgressBar
	/// iOS -> UIKit.UIProgressView
	/// </summary>
	public ProgressBar() : base(_ => new UIProgressView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ProgressBar(CGRect frame) : base(_ => new UIProgressView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ProgressBar(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIProgressView
	/// </summary>
	public new UIProgressView NativeView => (UIProgressView)_nativeView.Value;

	partial void OnProgressChanged(double value) => NativeView.Progress = (float)value;
}
