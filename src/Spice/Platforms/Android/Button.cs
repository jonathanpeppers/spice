using Android.Content;

namespace Spice;

public partial class Button
{
	public static implicit operator Android.Views.View(Button button) => button.NativeView;

	public static implicit operator Android.Widget.Button(Button button) => button.NativeView;

	public Button() : base(inherited: true) => NativeView = new Android.Widget.Button(Platform.Context);

	public Button(Context context) : base(inherited: true) => NativeView = new Android.Widget.Button(context);

#pragma warning disable CS8618
	public Button(bool inherited) : base(inherited)
#pragma warning restore CS8618
	{
		// NOTE: the purpose of this constructor is so types can prevent subclasses from creating controls
	}

	public new Android.Widget.Button NativeView { get; private set; }

	protected override Android.Views.View _nativeView => NativeView;
}