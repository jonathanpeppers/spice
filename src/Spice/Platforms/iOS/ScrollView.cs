namespace Spice;

public partial class ScrollView
{
	/// <summary>
	/// Returns scrollView.NativeView
	/// </summary>
	/// <param name="scrollView">The Spice.ScrollView</param>
	public static implicit operator UIScrollView(ScrollView scrollView) => scrollView.NativeView;

	/// <summary>
	/// A scrollable container view that can hold a single child view.
	/// Android -> Android.Widget.ScrollView
	/// iOS -> UIKit.UIScrollView
	/// </summary>
	public ScrollView() : base(v => new SpiceScrollView((ScrollView)v) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ScrollView(CGRect frame) : base(v => new SpiceScrollView((ScrollView)v, frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ScrollView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIScrollView
	/// </summary>
	public new UIScrollView NativeView => (UIScrollView)_nativeView.Value;

	partial void OnOrientationChanging(Orientation value)
	{
		// iOS UIScrollView supports both directions simultaneously by default
		// We can optionally restrict scrolling direction if needed
	}

	/// <inheritdoc />
	protected override void AddSubview(View view)
	{
		// ScrollView should only have one child
		// Remove all existing children first
		foreach (UIView subview in NativeView.Subviews)
		{
			subview.RemoveFromSuperview();
		}

		NativeView.AddSubview(view);
		view.UpdateAlign();

		// Update content size based on child
		UpdateContentSize(view);
	}

	void UpdateContentSize(View view)
	{
		var childView = (UIView)view;
		childView.SizeToFit();
		var size = childView.Frame.Size;

		if (Orientation == Orientation.Horizontal)
		{
			// Allow horizontal scrolling, constrain vertical
			NativeView.ContentSize = new CGSize(size.Width, NativeView.Frame.Height);
		}
		else // Vertical
		{
			// Allow vertical scrolling, constrain horizontal
			NativeView.ContentSize = new CGSize(NativeView.Frame.Width, size.Height);
		}
	}

	class SpiceScrollView : UIScrollView
	{
		readonly ScrollView _parent;

		public SpiceScrollView(ScrollView parent) => _parent = parent;

		public SpiceScrollView(ScrollView parent, CGRect frame) : base(frame) => _parent = parent;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			// Update content size when layout changes
			if (Subviews.Length > 0)
			{
				_parent.UpdateContentSize((View)Subviews[0]);
			}
		}
	}
}
