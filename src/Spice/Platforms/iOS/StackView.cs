namespace Spice;

public partial class StackView
{
	public static implicit operator UIStackView(StackView stackView) => stackView.NativeView;

	public StackView() : this(v => new SpiceStackView((StackView)v) { AutoresizingMask = UIViewAutoresizing.FlexibleDimensions, Axis = UILayoutConstraintAxis.Vertical }) { }

	public StackView(CGRect frame) : this(v => new SpiceStackView((StackView)v, frame) { AutoresizingMask = UIViewAutoresizing.FlexibleDimensions, Axis = UILayoutConstraintAxis.Vertical }) { }

	public StackView(Func<View, UIView> creator) : base(creator) { }

	public new UIStackView NativeView => (UIStackView)_nativeView.Value;

	partial void OnOrientationChanging(Orientation value)
	{
		switch (value)
		{
			case Orientation.Vertical:
				NativeView.Axis = UILayoutConstraintAxis.Vertical;
				break;
			case Orientation.Horizontal:
				NativeView.Axis = UILayoutConstraintAxis.Horizontal;
				break;
			default:
				throw new NotSupportedException($"{nameof(Orientation)} value '{value}' not supported!");
		}
	}

	class SpiceStackView : UIStackView
	{
		readonly StackView _parent;

		public SpiceStackView(StackView parent) => _parent = parent;

		public SpiceStackView(StackView parent, CGRect frame) : base(frame) => _parent = parent;

		public override CGSize SizeThatFits(CGSize size)
		{
			var measured = new CGSize();
			foreach (var child in Subviews)
			{
				var childSize = child.SizeThatFits(size);
				if (_parent.Orientation == Orientation.Vertical)
				{
					if (childSize.Width > measured.Width)
						measured.Width = childSize.Width;
					measured.Height += childSize.Height;
				}
				else if (_parent.Orientation == Orientation.Horizontal)
				{
					measured.Width += childSize.Width;
					if (childSize.Height > measured.Height)
						measured.Height = childSize.Height;
				}
			}
			return measured;
		}
	}
}