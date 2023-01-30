using Android.Content;

namespace Spice;

public partial class View
{
	public static implicit operator Android.Views.View(View view) => view.Android;

	public View() => Android = new Android.Views.View(Platform.Context);

	public View(Context context) => Android = new Android.Views.View(context);

#pragma warning disable CS8618
	public View(bool inherited)
#pragma warning restore CS8618
	{
		// NOTE: the purpose of this constructor is so types can prevent subclasses from creating controls
	}

	public Android.Views.View Android { get; private set; }
}