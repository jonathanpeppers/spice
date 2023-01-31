using System.Collections.Specialized;
using Android.Content;

namespace Spice;

public partial class View
{
	public static implicit operator Android.Views.View(View view) => view._nativeView.Value;

	public View() : this(Platform.Context!, c => new RelativeLayout(c)) { }

	public View(Context context) : this(context, c => new RelativeLayout(c)) { }

	public View(Context context, Func<Context, Android.Views.View> creator)
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

	protected readonly Lazy<Android.Views.ViewGroup.LayoutParams> _layoutParameters;
	protected readonly Lazy<Android.Views.View> _nativeView;

	protected virtual Android.Views.ViewGroup.LayoutParams CreateLayoutParameters() =>
		new(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

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

		if (_layoutParameters.Value is RelativeLayout.LayoutParams relativeParameters)
		{
			// TODO: support value changing
			switch (value)
			{
				case Align.Center:
					relativeParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
					relativeParameters.AddRule(LayoutRules.CenterHorizontal);
					break;
				case Align.Start:
					relativeParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
					relativeParameters.AddRule(LayoutRules.AlignLeft);
					break;
				case Align.End:
					relativeParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
					relativeParameters.AddRule(LayoutRules.AlignRight);
					break;
				case Align.Stretch:
					relativeParameters.Width = Android.Views.ViewGroup.LayoutParams.MatchParent;
					break;
				default:
					throw new NotSupportedException($"{nameof(HorizontalAlign)} value '{value}' not supported!");
			}
		}

		if (_layoutParameters.Value is LinearLayout.LayoutParams linearParameters)
		{
			UpdateGravity(linearParameters);
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
					layoutParameters.AddRule(LayoutRules.CenterVertical);
					break;
				case Align.Start:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.AddRule(LayoutRules.AlignTop);
					break;
				case Align.End:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
					layoutParameters.AddRule(LayoutRules.AlignBottom);
					break;
				case Align.Stretch:
					layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.MatchParent;
					break;
				default:
					throw new NotSupportedException($"{nameof(VerticalAlign)} value '{value}' not supported!");
			}
		}

		if (_layoutParameters.Value is LinearLayout.LayoutParams linearParameters)
		{
			UpdateGravity(linearParameters);
		}
	}

	void UpdateGravity(LinearLayout.LayoutParams linearParameters)
	{
		var gravity = Android.Views.GravityFlags.NoGravity;
		int height = Android.Views.ViewGroup.LayoutParams.WrapContent, width = Android.Views.ViewGroup.LayoutParams.WrapContent;

		switch (_horizontalAlign)
		{
			case Align.Center:
				gravity |= Android.Views.GravityFlags.CenterHorizontal;
				break;
			case Align.Start:
				gravity |= Android.Views.GravityFlags.Left;
				break;
			case Align.End:
				gravity |= Android.Views.GravityFlags.Right;
				break;
			case Align.Stretch:
				height = Android.Views.ViewGroup.LayoutParams.MatchParent;
				gravity |= Android.Views.GravityFlags.FillHorizontal;
				break;
			default:
				throw new NotSupportedException($"{nameof(HorizontalAlign)} value '{_horizontalAlign}' not supported!");
		}

		switch (_verticalAlign)
		{
			case Align.Center:
				gravity |= Android.Views.GravityFlags.CenterVertical;
				break;
			case Align.Start:
				gravity |= Android.Views.GravityFlags.Top;
				break;
			case Align.End:
				gravity |= Android.Views.GravityFlags.Bottom;
				break;
			case Align.Stretch:
				height = Android.Views.ViewGroup.LayoutParams.MatchParent;
				gravity |= Android.Views.GravityFlags.FillVertical;
				break;
			default:
				throw new NotSupportedException($"{nameof(VerticalAlign)} value '{_verticalAlign}' not supported!");
		}

		// TODO: reduce JNI calls
		linearParameters.Width = width;
		linearParameters.Height = height;
		linearParameters.Gravity = gravity;
	}

	partial void OnBackgroundColorChanged(Color? value) => _nativeView.Value.Background = value.ToAndroidDrawable();
}