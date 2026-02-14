namespace Spice;

public partial class BoxView
{
	/// <summary>
	/// Returns boxView.NativeView
	/// </summary>
	/// <param name="boxView">The Spice.BoxView</param>
	public static implicit operator UIView(BoxView boxView) => boxView.NativeView;

	/// <summary>
	/// Represents a colored rectangle used for decoration, backgrounds, or dividing lines.
	/// Android -> Android.Views.View
	/// iOS -> UIKit.UIView
	/// </summary>
	public BoxView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public BoxView(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected BoxView(Func<View, UIView> creator) : base(creator) { }

	partial void OnColorChanged(Color? value) => NativeView.BackgroundColor = value.ToUIColor();
}
