namespace Spice;

public partial class StackView
{
	/// <summary>
	/// Returns stackView.NativeView
	/// </summary>
	/// <param name="stackView">The Spice.StackView</param>
	public static implicit operator UIStackView(StackView stackView) => stackView.NativeView;

	/// <summary>
	/// StackView ctor
	/// </summary>
	public StackView() : this(v => new SpiceStackView((StackView)v) { AutoresizingMask = UIViewAutoresizing.None, Alignment = UIStackViewAlignment.Center, Axis = UILayoutConstraintAxis.Vertical }) { }

	/// <summary>
	/// StackView ctor
	/// </summary>
	/// <param name="frame">Pass the underlying view a frame</param>
	public StackView(CGRect frame) : this(v => new SpiceStackView((StackView)v, frame) { AutoresizingMask = UIViewAutoresizing.None, Alignment = UIStackViewAlignment.Center, Axis = UILayoutConstraintAxis.Vertical }) { }

	/// <summary>
	/// StackView ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected StackView(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIStackView
	/// </summary>
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

	/// <inheritdoc cref="F:Spice.View.AddSubview" />
	protected override void AddSubview(View view)
	{
		NativeView.AddArrangedSubview(view);
		view.UpdateAlign();
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