using Android.Content;

namespace Spice;

public partial class ScrollView
{
	/// <summary>
	/// Returns scrollView.NativeView
	/// </summary>
	/// <param name="scrollView">The Spice.ScrollView</param>
	public static implicit operator Android.Views.ViewGroup(ScrollView scrollView) => (Android.Views.ViewGroup)scrollView._nativeView.Value;

	static Android.Views.View CreateVertical(Context context) => new Android.Widget.ScrollView(context);

	static Android.Views.View CreateHorizontal(Context context) => new HorizontalScrollView(context);

	static Func<Context, Android.Views.View> GetCreator(Orientation orientation) =>
		orientation == Orientation.Horizontal ? CreateHorizontal : CreateVertical;

	/// <summary>
	/// A scrollable container view that can hold a single child view.
	/// Android -> Android.Widget.ScrollView (vertical) / Android.Widget.HorizontalScrollView (horizontal)
	/// iOS -> UIKit.UIScrollView
	/// </summary>
	public ScrollView() : this(Platform.Context, Orientation.Vertical) { }

	/// <inheritdoc />
	/// <param name="orientation">The scroll orientation. Must be set at construction time on Android.</param>
	public ScrollView(Orientation orientation) : this(Platform.Context, orientation) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public ScrollView(Context context) : this(context, Orientation.Vertical) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="orientation">The scroll orientation. Must be set at construction time on Android.</param>
	protected ScrollView(Context context, Orientation orientation) : base(context, GetCreator(orientation))
	{
		_orientation = orientation;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ScrollView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ScrollView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.ScrollView (or HorizontalScrollView)
	/// </summary>
	public new Android.Views.ViewGroup NativeView => (Android.Views.ViewGroup)_nativeView.Value;

	partial void OnOrientationChanging(Orientation value)
	{
		// Android has separate ScrollView (vertical) and HorizontalScrollView classes.
		// Use the ScrollView(Orientation) constructor to set orientation at creation time.
		throw new NotSupportedException(
			"Changing ScrollView orientation is not supported on Android. " +
			"Use the ScrollView(Orientation) constructor instead.");
	}
}
