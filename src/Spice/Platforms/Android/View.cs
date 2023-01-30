using Android.Content;

namespace Spice;

public partial class View
{
	public static implicit operator Android.Views.View(View view) => view._nativeView;

	public View() => _nativeView = new Android.Views.View(Platform.Context);

	public View(Context context) => _nativeView = new Android.Widget.RelativeLayout(context);

#pragma warning disable CS8618
	public View(bool inherited)
#pragma warning restore CS8618
	{
		// NOTE: the purpose of this constructor is so types can prevent subclasses from creating controls
	}

	protected virtual Android.Views.View _nativeView { get; private set; }

	public Android.Views.View NativeView => _nativeView;

	readonly Android.Views.ViewGroup.LayoutParams _layoutParams =
		new(Android.Views.ViewGroup.LayoutParams.WrapContent, Android.Views.ViewGroup.LayoutParams.WrapContent);

	partial void OnHorizontalAlignChanged(Align value)
	{
		switch (value)
		{
			case Align.Center:
			case Align.Start:
			case Align.End:
				_layoutParams.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
				break;
			case Align.Stretch:
				_layoutParams.Width = Android.Views.ViewGroup.LayoutParams.MatchParent;
				break;
			default:
				throw new NotSupportedException($"Align value '{value}' not supported!");
		}
		_nativeView.LayoutParameters = _layoutParams;
	}

	partial void OnVerticalAlignChanged(Align value)
	{
		switch (value)
		{
			case Align.Center:
			case Align.Start:
			case Align.End:
				_layoutParams.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
				break;
			case Align.Stretch:
				_layoutParams.Height = Android.Views.ViewGroup.LayoutParams.MatchParent;
				break;
			default:
				throw new NotSupportedException($"Align value '{value}' not supported!");
		}
		_nativeView.LayoutParameters = _layoutParams;
	}
}