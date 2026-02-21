namespace Spice;

public partial class ContentView
{
	/// <summary>
	/// Returns contentView.NativeView
	/// </summary>
	/// <param name="contentView">The Spice.ContentView</param>
	public static implicit operator UIView(ContentView contentView) => contentView.NativeView;

	/// <summary>
	/// A container view that holds a single child view via the Content property.
	/// Android -> Android.Views.ViewGroup (FrameLayout)
	/// iOS -> UIKit.UIView
	/// </summary>
	public ContentView() : base(v => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ContentView(CGRect frame) : base(v => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ContentView(Func<View, UIView> creator) : base(creator) { }

	NSLayoutConstraint[]? _contentConstraints;

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Remove old content
		if (oldContent != null)
		{
			ConstraintHelper.RemoveConstraints(_contentConstraints);
			_contentConstraints = null;
			((UIView)oldContent).RemoveFromSuperview();
		}

		// Add new content
		if (newContent != null)
		{
			UIView contentView = newContent;
			NativeView.AddSubview(contentView);
			var padding = (nfloat)Padding;
			_contentConstraints = ConstraintHelper.PinEdges(contentView, NativeView, padding);
		}
	}

	partial void OnPaddingChanged(double value)
	{
		if (_contentConstraints != null)
		{
			var padding = (nfloat)value;
			ConstraintHelper.UpdateInsets(_contentConstraints, padding);
		}
	}
}
