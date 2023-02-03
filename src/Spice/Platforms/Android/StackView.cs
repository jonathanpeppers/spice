using Android.Content;

namespace Spice;

public partial class StackView
{
	/// <summary>
	/// Returns stackView.NativeView
	/// </summary>
	/// <param name="stackView">The Spice.StackView</param>
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

	/// <summary>
	/// StackView ctor
	/// </summary>
	public StackView() : this(Platform.Context, Create) { }

	/// <summary>
	/// StackView ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public StackView(Context context) : this(context, Create) { }

	/// <summary>
	/// StackView ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected StackView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <summary>
	/// StackView ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected StackView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.LinearLayout
	/// </summary>
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