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

	SpiceModalFragment? _presentedFragment;

	private partial Task PresentAsyncCore(View view)
	{
		var activity = Platform.Context as AndroidX.AppCompat.App.AppCompatActivity;
		if (activity == null)
		{
			return Task.CompletedTask;
		}

		_presentedFragment = new SpiceModalFragment(view);
		_presentedFragment.Show(activity.SupportFragmentManager, "spice_modal");

		return Task.CompletedTask;
	}

	private partial Task DismissAsyncCore()
	{
		if (_presentedFragment != null)
		{
			_presentedFragment.Dismiss();
			_presentedFragment = null;
		}

		return Task.CompletedTask;
	}

	class SpiceModalFragment : AndroidX.Fragment.App.DialogFragment
	{
		readonly View _spiceView;

		public SpiceModalFragment(View spiceView)
		{
			_spiceView = spiceView;
		}

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup? container, Android.OS.Bundle? savedInstanceState)
		{
			var frame = new Android.Widget.FrameLayout(RequireContext())
			{
				LayoutParameters = new Android.Views.ViewGroup.LayoutParams(
					Android.Views.ViewGroup.LayoutParams.MatchParent,
					Android.Views.ViewGroup.LayoutParams.MatchParent)
			};
			frame.AddView(_spiceView);
			return frame;
		}

		public override void OnStart()
		{
			base.OnStart();

			// Make the dialog fullscreen
			Dialog?.Window?.SetLayout(
				Android.Views.ViewGroup.LayoutParams.MatchParent,
				Android.Views.ViewGroup.LayoutParams.MatchParent);

			if (!string.IsNullOrEmpty(_spiceView.Title))
			{
				Dialog?.SetTitle(_spiceView.Title);
			}
		}
	}
}