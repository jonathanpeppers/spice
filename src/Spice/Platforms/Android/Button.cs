using Android.Content;

namespace Spice;

public partial class Button
{
	public static implicit operator Android.Views.View(Button button) => button.Android;

	public static implicit operator Android.Widget.Button(Button button) => button.Android;

	public Button() : this(inherited: true) => Android = new Android.Widget.Button(Platform.Context);

	public Button(Context context) : this(inherited: true) => Android = new Android.Widget.Button(context);

#pragma warning disable CS8618
	public Button(bool inherited)
#pragma warning restore CS8618
	{
		// NOTE: the purpose of this constructor is so types can prevent subclasses from creating controls
	}

	public new Android.Widget.Button Android { get; private set; }
}