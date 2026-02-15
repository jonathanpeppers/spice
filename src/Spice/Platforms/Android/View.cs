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

	partial void OnHorizontalAlignChanged(Align value)
	{
		// TODO: reduce JNI calls
		if (_layoutParameters.Value is RelativeLayout.LayoutParams layoutParameters)
		{
			// TODO: support value changing
			switch (value)
			{
				case Align.Center:
					layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.AddRule(LayoutRules.CenterHorizontal);
					break;
				case Align.Start:
					layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterHorizontal);
					layoutParameters.AddRule(LayoutRules.AlignParentLeft);
					break;
				case Align.End:
					layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterHorizontal);
					layoutParameters.AddRule(LayoutRules.AlignParentRight);
					break;
				case Align.Stretch:
					layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.MatchParent;
					break;
				default:
					throw new NotSupportedException($"{nameof(HorizontalAlign)} value '{value}' not supported!");
			}
		}
		else
		{
			throw new NotSupportedException($"LayoutParameters of type {_layoutParameters.Value.GetType()} not supported!");
		}
	}

	partial void OnVerticalAlignChanged(Align value)
	{
		// TODO: reduce JNI calls

		if (_layoutParameters.Value is RelativeLayout.LayoutParams layoutParameters)
		{
			// TODO: support value changing
			switch (value)
			{
				case Align.Center:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterVertical);
					layoutParameters.AddRule(LayoutRules.CenterVertical);
					break;
				case Align.Start:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterVertical);
					layoutParameters.AddRule(LayoutRules.AlignParentTop);
					break;
				case Align.End:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.RemoveRule(LayoutRules.CenterVertical);
					layoutParameters.AddRule(LayoutRules.AlignParentBottom);
					break;
				case Align.Stretch:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.MatchParent;
					break;
				default:
					throw new NotSupportedException($"{nameof(VerticalAlign)} value '{value}' not supported!");
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

	partial void OnWidthRequestChanged(double value)
	{
		if (_layoutParameters.IsValueCreated)
		{
			var layoutParameters = _layoutParameters.Value;
			
			// Convert value to pixels
			// -1 means unset, use WrapContent if alignment is not Stretch
			if (value < 0)
			{
				layoutParameters.Width = HorizontalAlign == Align.Stretch 
					? Android.Views.ViewGroup.LayoutParams.MatchParent 
					: Android.Views.ViewGroup.LayoutParams.WrapContent;
			}
			else
			{
				// Convert from device-independent units to pixels
				var displayMetrics = _nativeView.Value.Resources?.DisplayMetrics;
				if (displayMetrics != null)
				{
					layoutParameters.Width = (int)(value * displayMetrics.Density);
				}
				else
				{
					layoutParameters.Width = (int)value;
				}
			}
			
			_nativeView.Value.RequestLayout();
		}
	}

	partial void OnHeightRequestChanged(double value)
	{
		if (_layoutParameters.IsValueCreated)
		{
			var layoutParameters = _layoutParameters.Value;
			
			// Convert value to pixels
			// -1 means unset, use WrapContent if alignment is not Stretch
			if (value < 0)
			{
				layoutParameters.Height = VerticalAlign == Align.Stretch 
					? Android.Views.ViewGroup.LayoutParams.MatchParent 
					: Android.Views.ViewGroup.LayoutParams.WrapContent;
			}
			else
			{
				// Convert from device-independent units to pixels
				var displayMetrics = _nativeView.Value.Resources?.DisplayMetrics;
				if (displayMetrics != null)
				{
					layoutParameters.Height = (int)(value * displayMetrics.Density);
				}
				else
				{
					layoutParameters.Height = (int)value;
				}
			}
			
			_nativeView.Value.RequestLayout();
		}
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
}