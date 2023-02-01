namespace Spice;

public partial class Application
{
	public Application() : base(_ => new UIView(Platform.Window!.Frame) { AutoresizingMask = UIViewAutoresizing.All }) { }

	public Application(CGRect frame) : base(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.All }) { }

	partial void OnMainChanging(View? value)
	{
		if (_main != null)
		{
			((UIView)_main).RemoveFromSuperview();
		}
	}

	partial void OnMainChanged(View? value)
	{
		if (value != null)
		{
			AddSubview(value);
		}
	}
}