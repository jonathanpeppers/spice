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
	public ContentView() : base(v => new SpiceContentView((ContentView)v) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public ContentView(CGRect frame) : base(v => new SpiceContentView((ContentView)v, frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected ContentView(Func<View, UIView> creator) : base(creator) { }

	readonly UIView _paddingWrapper = new UIView { AutoresizingMask = UIViewAutoresizing.None };

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
			if (_paddingWrapper.Superview == null)
				NativeView.AddSubview(_paddingWrapper);
			_paddingWrapper.AddSubview(newContent);
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
		var bounds = NativeView.Bounds;
		var width = (nfloat)Math.Max(0, bounds.Width - padding * 2);
		var height = (nfloat)Math.Max(0, bounds.Height - padding * 2);

		_paddingWrapper.Frame = new CGRect(padding, padding, width, height);
		Content.UpdateAlign();
	}

	class SpiceContentView : UIView
	{
		readonly ContentView _parent;

		public SpiceContentView(ContentView parent) => _parent = parent;

		public SpiceContentView(ContentView parent, CGRect frame) : base(frame) => _parent = parent;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			_parent.UpdateContentLayout();
		}
	}
}
