namespace Spice;

public partial class StackView
{
	public static implicit operator UIStackView(StackView stackView) => stackView.NativeView;

	public StackView() : this(() => new UIStackView { TranslatesAutoresizingMaskIntoConstraints = false }) { }

	public StackView(CGRect frame) : this(() => new UIStackView(frame) { TranslatesAutoresizingMaskIntoConstraints = false }) { }

	public StackView(Func<UIView> creator) : base(creator) { }

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
}