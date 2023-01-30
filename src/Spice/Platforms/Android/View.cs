using Android.Content;

namespace Spice;

public partial class View
{
	public static implicit operator Android.Views.View(View view) => view._nativeView.Value;

	public View() : this(Platform.Context!, c => new RelativeLayout(c)) { }

	public View(Context context) : this(context, c => new RelativeLayout(c)) { }

	public View(Context context, Func<Context, Android.Views.View> creator) => _nativeView = new Lazy<Android.Views.View>(() =>
	{
		var view = creator(context);
		view.LayoutParameters = _layoutParameters;
		return view;
	});

	protected readonly Lazy<Android.Views.View> _nativeView;

	public Android.Views.ViewGroup NativeView => (Android.Views.ViewGroup)_nativeView.Value;

	readonly RelativeLayout.LayoutParams _layoutParameters =
		new(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

	public partial void Add(View item)
	{
		Children.Add(item);
		NativeView.AddView(item);
	}

	partial void OnHorizontalAlignChanged(Align value)
	{
		// TODO: support value changing
		// TODO: reduce JNI calls

		switch (value)
		{
			case Align.Center:
				_layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
				_layoutParameters.AddRule(LayoutRules.CenterHorizontal);
				break;
			case Align.Start:
				_layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
				_layoutParameters.AddRule(LayoutRules.AlignLeft);
				break;
			case Align.End:
				_layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
				_layoutParameters.AddRule(LayoutRules.AlignRight);
				break;
			case Align.Stretch:
				_layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.MatchParent;
				break;
			default:
				throw new NotSupportedException($"Align value '{value}' not supported!");
		}
	}

	partial void OnVerticalAlignChanged(Align value)
	{
		// TODO: support value changing
		// TODO: reduce JNI calls

		switch (value)
		{
			case Align.Center:
				_layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
				_layoutParameters.AddRule(LayoutRules.CenterVertical);
				break;
			case Align.Start:
				_layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
				_layoutParameters.AddRule(LayoutRules.AlignTop);
				break;
			case Align.End:
				_layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
				_layoutParameters.AddRule(LayoutRules.AlignBottom);
				break;
			case Align.Stretch:
				_layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.MatchParent;
				break;
			default:
				throw new NotSupportedException($"Align value '{value}' not supported!");
		}
	}
}