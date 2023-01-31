namespace Spice;

public partial class Application
{
	public Application() { }

	public Application(CGRect frame) : base(frame) { }

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
			NativeView.AddSubview(value);
		}
	}
}