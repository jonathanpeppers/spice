namespace Spice;

public partial class Application
{
	/// <summary>
	/// The root "view" of a Spice application. Set Main to a single view.
	/// </summary>
	public Application() : base(_ => new UIView(Platform.Window!.Frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Application(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	NSLayoutConstraint[]? _mainConstraints;

	partial void OnMainChanging(View? value)	
	{
		if (_main != null)
		{
			ConstraintHelper.RemoveConstraints(_mainConstraints);
			_mainConstraints = null;
			((UIView)_main).RemoveFromSuperview();
		}
	}

	partial void OnMainChanged(View? value)
	{
		if (value != null)
		{
			UIView native = value;
			NativeView.AddSubview(native);
			_mainConstraints = ConstraintHelper.PinEdges(native, NativeView);
		}
	}
}