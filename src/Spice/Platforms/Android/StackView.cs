using Android.Content;

namespace Spice;

public partial class StackView
{
	public static implicit operator LinearLayout(StackView stackView) => stackView.NativeView;

	static LinearLayout Create(Context context)
	{
		var layout = new LinearLayout(context)
		{
			Orientation = Android.Widget.Orientation.Vertical,
		};
		layout.SetGravity(Android.Views.GravityFlags.Center);
		return layout;
	}

	public StackView() : this(Platform.Context!, Create) { }

	public StackView(Context context) : this(context, Create) { }

	public StackView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	public new LinearLayout NativeView => (LinearLayout)_nativeView.Value;

	partial void OnOrientationChanging(Orientation value)
	{
		switch (value)
		{
			case Orientation.Vertical:
				NativeView.Orientation = Android.Widget.Orientation.Vertical;
				break;
			case Orientation.Horizontal:
				NativeView.Orientation = Android.Widget.Orientation.Horizontal;
				break;
			default:
				throw new NotSupportedException($"{nameof(Orientation)} value '{value}' not supported!");
		}
	}
}