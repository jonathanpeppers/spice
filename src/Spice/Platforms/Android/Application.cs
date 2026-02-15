using Android.Content;

namespace Spice;

public partial class Application
{
	/// <summary>
	/// The root "view" of a Spice application. Set Main to a single view.
	/// </summary>
	public Application()
	{
		Current = this;
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Application(Context context) : base(context)
	{
		Current = this;
	}

	/// <inheritdoc />
	protected override Android.Views.ViewGroup.LayoutParams CreateLayoutParameters() =>
		new RelativeLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

	partial void OnMainChanging(View? value)
	{
		if (_main != null)
		{
			NativeView.RemoveView(_main);
		}
	}

	partial void OnMainChanged(View? value)
	{
		if (value != null)
		{
			NativeView.AddView(value);
		}
	}

	AndroidX.AppCompat.App.AlertDialog? _presentedDialog;

	partial Task PresentAsyncCore(View view)
	{
		var activity = Platform.Context as AndroidX.AppCompat.App.AppCompatActivity;
		if (activity == null)
		{
			return Task.CompletedTask;
		}

		var builder = new AndroidX.AppCompat.App.AlertDialog.Builder(activity);
		
		if (!string.IsNullOrEmpty(view.Title))
		{
			builder.SetTitle(view.Title);
		}

		// Create a container for the view
		var container = new Android.Widget.FrameLayout(activity)
		{
			LayoutParameters = new Android.Views.ViewGroup.LayoutParams(
				Android.Views.ViewGroup.LayoutParams.MatchParent,
				Android.Views.ViewGroup.LayoutParams.WrapContent)
		};
		container.AddView(view);
		builder.SetView(container);

		_presentedDialog = builder.Create();
		_presentedDialog.SetCanceledOnTouchOutside(false);
		_presentedDialog.Show();

		return Task.CompletedTask;
	}

	partial Task DismissAsyncCore()
	{
		if (_presentedDialog != null)
		{
			_presentedDialog.Dismiss();
			_presentedDialog = null;
		}

		return Task.CompletedTask;
	}
}