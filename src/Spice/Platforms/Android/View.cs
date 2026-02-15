using System.Collections.Specialized;
using Android.Content;

namespace Spice;

public partial class View
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view"></param>
	public static implicit operator Android.Views.View(View view) => view._nativeView.Value;

	static RelativeLayout Create(Context context) => new(context);

	/// <summary>
	/// View ctor
	/// </summary>
	public View() : this(Platform.Context, Create) { }

	/// <summary>
	/// View ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public View(Context context) : this(context, Create) { }

	/// <summary>
	/// View ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected View(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <summary>
	/// View ctor
	/// </summary>
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected View(Context context, Func<Context, Android.Views.View> creator)
	{
		_layoutParameters = new Lazy<Android.Views.ViewGroup.LayoutParams>(CreateLayoutParameters);
		_nativeView = new Lazy<Android.Views.View>(() =>
		{
			var view = creator(context);
			view.LayoutParameters = _layoutParameters.Value;
			return view;
		});

		Children.CollectionChanged += OnChildrenChanged;
	}

	/// <summary>
	/// Subclasses can access the lazy LayoutParams
	/// </summary>
	protected readonly Lazy<Android.Views.ViewGroup.LayoutParams> _layoutParameters;
	/// <summary>
	/// Subclasses can access the lazy View
	/// </summary>
	protected readonly Lazy<Android.Views.View> _nativeView;

	/// <summary>
	/// Creates the LayoutParams for this view
	/// </summary>
	/// <returns>Android.Views.ViewGroup.LayoutParams</returns>
	protected virtual Android.Views.ViewGroup.LayoutParams CreateLayoutParameters()
	{
		var layoutParameters = new RelativeLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.WrapContent, Android.Views.ViewGroup.LayoutParams.WrapContent);
		layoutParameters.AddRule(LayoutRules.CenterHorizontal);
		layoutParameters.AddRule(LayoutRules.CenterVertical);
		return layoutParameters;
	}

	/// <summary>
	/// The underlying Android.Views.ViewGroup
	/// </summary>
	public Android.Views.ViewGroup NativeView => (Android.Views.ViewGroup)_nativeView.Value;

	void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.OldItems != null)
		{
			foreach (View item in e.OldItems)
			{
				NativeView.RemoveView(item.NativeView);
			}
		}

		if (e.NewItems != null)
		{
			foreach (View item in e.NewItems)
			{
				NativeView.AddView(item);
			}
		}
	}

	partial void OnHorizontalOptionsChanged(LayoutOptions value)
	{
		// TODO: reduce JNI calls
		if (_layoutParameters.Value is RelativeLayout.LayoutParams layoutParameters)
		{
			// TODO: support value changing
			switch (value.Alignment)
			{
				case LayoutAlignment.Center:
					layoutParameters.Width = WidthRequest >= 0 ? GetWidthInPixels() : Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.AddRule(LayoutRules.CenterHorizontal);
					break;
				case LayoutAlignment.Start:
					layoutParameters.Width = WidthRequest >= 0 ? GetWidthInPixels() : Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterHorizontal);
					layoutParameters.AddRule(LayoutRules.AlignParentLeft);
					break;
				case LayoutAlignment.End:
					layoutParameters.Width = WidthRequest >= 0 ? GetWidthInPixels() : Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterHorizontal);
					layoutParameters.AddRule(LayoutRules.AlignParentRight);
					break;
				case LayoutAlignment.Fill:
					layoutParameters.Width = WidthRequest >= 0 ? GetWidthInPixels() : Android.Views.ViewGroup.LayoutParams.MatchParent;
					break;
				default:
					throw new NotSupportedException($"{nameof(HorizontalOptions)} value '{value.Alignment}' not supported!");
			}
		}
		else
		{
			throw new NotSupportedException($"LayoutParameters of type {_layoutParameters.Value.GetType()} not supported!");
		}
	}

	partial void OnVerticalOptionsChanged(LayoutOptions value)
	{
		// TODO: reduce JNI calls

		if (_layoutParameters.Value is RelativeLayout.LayoutParams layoutParameters)
		{
			// TODO: support value changing
			switch (value.Alignment)
			{
				case LayoutAlignment.Center:
					layoutParameters.Height = HeightRequest >= 0 ? GetHeightInPixels() : Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterVertical);
					layoutParameters.AddRule(LayoutRules.CenterVertical);
					break;
				case LayoutAlignment.Start:
					layoutParameters.Height = HeightRequest >= 0 ? GetHeightInPixels() : Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterVertical);
					layoutParameters.AddRule(LayoutRules.AlignParentTop);
					break;
				case LayoutAlignment.End:
					layoutParameters.Height = HeightRequest >= 0 ? GetHeightInPixels() : Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterVertical);
					layoutParameters.AddRule(LayoutRules.AlignParentBottom);
					break;
				case LayoutAlignment.Fill:
					layoutParameters.Height = HeightRequest >= 0 ? GetHeightInPixels() : Android.Views.ViewGroup.LayoutParams.MatchParent;
					break;
				default:
					throw new NotSupportedException($"{nameof(VerticalOptions)} value '{value.Alignment}' not supported!");
			}
		}
		else
		{
			throw new NotSupportedException($"LayoutParameters of type {_layoutParameters.Value.GetType()} not supported!");
		}
	}

	partial void OnBackgroundColorChanged(Color? value) => _nativeView.Value.SetBackgroundColor(value.ToAndroidColor());

	partial void OnIsVisibleChanged(bool value) => _nativeView.Value.Visibility = value ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;

	partial void OnIsEnabledChanged(bool value) => _nativeView.Value.Enabled = value;

	partial void OnOpacityChanged(double value) => _nativeView.Value.Alpha = (float)value;

	partial void OnAutomationIdChanged(string? value) => _nativeView.Value.ContentDescription = value;

	partial void OnMarginChanged(Thickness value)
	{
		if (_layoutParameters.Value is RelativeLayout.LayoutParams layoutParams)
		{
			layoutParams.SetMargins(
				(int)value.Left.ToPixels(),
				(int)value.Top.ToPixels(),
				(int)value.Right.ToPixels(),
				(int)value.Bottom.ToPixels()
			);
		}
		else
		{
			throw new NotSupportedException($"LayoutParameters of type {_layoutParameters.Value.GetType()} not supported!");
		}
	}

	partial void OnWidthRequestChanged(double value)
	{
		var layoutParameters = _layoutParameters.Value;

		if (value < 0)
		{
			layoutParameters.Width = HorizontalOptions.Alignment == LayoutAlignment.Fill
				? Android.Views.ViewGroup.LayoutParams.MatchParent
				: Android.Views.ViewGroup.LayoutParams.WrapContent;
		}
		else
		{
			layoutParameters.Width = GetWidthInPixels();
		}

		_nativeView.Value.RequestLayout();
	}

	partial void OnHeightRequestChanged(double value)
	{
		var layoutParameters = _layoutParameters.Value;

		if (value < 0)
		{
			layoutParameters.Height = VerticalOptions.Alignment == LayoutAlignment.Fill
				? Android.Views.ViewGroup.LayoutParams.MatchParent
				: Android.Views.ViewGroup.LayoutParams.WrapContent;
		}
		else
		{
			layoutParameters.Height = GetHeightInPixels();
		}

		_nativeView.Value.RequestLayout();
	}

	int GetWidthInPixels()
	{
		var displayMetrics = _nativeView.Value.Resources?.DisplayMetrics;
		return displayMetrics != null ? (int)(WidthRequest * displayMetrics.Density) : (int)WidthRequest;
	}

	int GetHeightInPixels()
	{
		var displayMetrics = _nativeView.Value.Resources?.DisplayMetrics;
		return displayMetrics != null ? (int)(HeightRequest * displayMetrics.Density) : (int)HeightRequest;
	}

	private partial double GetWidth()
	{
		var view = _nativeView.Value;
		// Convert from pixels to device-independent units
		var displayMetrics = view.Resources?.DisplayMetrics;
		if (displayMetrics != null)
		{
			return view.Width / displayMetrics.Density;
		}
		return view.Width;
	}

	private partial double GetHeight()
	{
		var view = _nativeView.Value;
		// Convert from pixels to device-independent units
		var displayMetrics = view.Resources?.DisplayMetrics;
		if (displayMetrics != null)
		{
			return view.Height / displayMetrics.Density;
		}
		return view.Height;
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