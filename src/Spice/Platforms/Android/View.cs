using Android.Content;

namespace Spice;

public partial class View
{
	public static implicit operator Android.Views.View(View view) => view._nativeView.Value;

	public View() : this(Platform.Context!, c => new Android.Widget.RelativeLayout(c)) { }

	public View(Context context) : this(context, c => new Android.Widget.RelativeLayout(c)) { }

	public View(Context context, Func<Context, Android.Views.View> creator) => _nativeView = new Lazy<Android.Views.View>(() =>
	{
		var view = creator(context);
		view.LayoutParameters = _layoutParameters;
		return view;
	});

	protected readonly Lazy<Android.Views.View> _nativeView;

	public Android.Views.View NativeView => _nativeView.Value;

	readonly Android.Views.ViewGroup.LayoutParams _layoutParameters =
		new(Android.Views.ViewGroup.LayoutParams.WrapContent, Android.Views.ViewGroup.LayoutParams.WrapContent);

	partial void OnHorizontalAlignChanged(Align value)
	{
		switch (value)
		{
			case Align.Center:
			case Align.Start:
			case Align.End:
				_layoutParameters.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
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
		switch (value)
		{
			case Align.Center:
			case Align.Start:
			case Align.End:
				_layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
				break;
			case Align.Stretch:
				_layoutParameters.Height = Android.Views.ViewGroup.LayoutParams.MatchParent;
				break;
			default:
				throw new NotSupportedException($"Align value '{value}' not supported!");
		}
	}
}