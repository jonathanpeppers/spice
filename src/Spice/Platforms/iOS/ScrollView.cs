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
	/// Android -> Android.Widget.ScrollView / Android.Widget.HorizontalScrollView
	/// iOS -> UIKit.UIScrollView
	/// </summary>
	public ScrollView() : base(v => new SpiceScrollView((ScrollView)v) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="orientation">The scroll orientation.</param>
	public ScrollView(Orientation orientation) : this()
	{
		Orientation = orientation;
	}

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

	View? _childView;

	partial void OnOrientationChanging(Orientation value)
	{
		if (!_nativeView.IsValueCreated)
			return;

		var scrollView = NativeView;
		if (value == Orientation.Horizontal)
		{
			scrollView.AlwaysBounceHorizontal = true;
			scrollView.AlwaysBounceVertical = false;
			scrollView.ShowsHorizontalScrollIndicator = true;
			scrollView.ShowsVerticalScrollIndicator = false;
		}
		else
		{
			scrollView.AlwaysBounceHorizontal = false;
			scrollView.AlwaysBounceVertical = true;
			scrollView.ShowsHorizontalScrollIndicator = false;
			scrollView.ShowsVerticalScrollIndicator = true;
		}

		if (_childView != null)
			UpdateContentSize(_childView);
	}

	partial void OnPaddingChanged(double value)
	{
		var padding = (nfloat)value;
		NativeView.ContentInset = new UIEdgeInsets(padding, padding, padding, padding);
	}

	/// <inheritdoc />
	protected override void AddSubview(View view)
	{
		// ScrollView should only have one child — remove existing subviews
		foreach (UIView subview in NativeView.Subviews)
		{
			subview.RemoveFromSuperview();
		}

		_childView = view;
		NativeView.AddSubview(view);
		view.UpdateAlign();
		UpdateContentSize(view);
	}

	void UpdateContentSize(View view)
	{
		var childView = (UIView)view;
		childView.SizeToFit();
		var size = childView.Frame.Size;

		if (Orientation == Orientation.Horizontal)
		{
			NativeView.ContentSize = new CGSize(size.Width, NativeView.Frame.Height);
		}
		else
		{
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

			if (_parent._childView != null)
			{
				_parent.UpdateContentSize(_parent._childView);
			}
		}
	}
}
