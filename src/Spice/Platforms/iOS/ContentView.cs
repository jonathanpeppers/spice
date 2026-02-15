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
	public ContentView() : base(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ContentView(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ContentView(Func<View, UIView> creator) : base(creator) { }

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Remove old content
		if (oldContent != null)
		{
			((UIView)oldContent).RemoveFromSuperview();
		}

		// Add new content
		if (newContent != null)
		{
			NativeView.AddSubview(newContent);
			UpdateContentLayout();
		}
	}

	partial void OnPaddingChanged(double value)
	{
		UpdateContentLayout();
	}

	void UpdateContentLayout()
	{
		if (Content == null)
			return;

		var padding = (nfloat)Padding;
		var contentView = (UIView)Content;
		var bounds = NativeView.Bounds;

		contentView.Frame = new CGRect(
			padding,
			padding,
			bounds.Width - (padding * 2),
			bounds.Height - (padding * 2)
		);
	}
}
