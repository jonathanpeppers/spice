using Android.Content;

namespace Spice;

public partial class Label
{
	public static implicit operator Android.Views.View(Label label) => label.Android;

	public static implicit operator Android.Widget.TextView(Label label) => label.Android;

	public Label() : base(inherited: true) => Android = new Android.Widget.Button(Platform.Context);

	public Label(Context context) : base(inherited: true) => Android = new Android.Widget.Button(context);

#pragma warning disable CS8618
	public Label(bool inherited) : base(inherited)
#pragma warning restore CS8618
	{
		// NOTE: the purpose of this constructor is so types can prevent subclasses from creating controls
	}

	public new Android.Widget.TextView Android { get; private set; }
}
