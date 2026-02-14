using Android.Content;

namespace Spice;

public partial class ScrollView
{
	/// <summary>
	/// Returns scrollView.NativeView
	/// </summary>
	/// <param name="scrollView">The Spice.ScrollView</param>
	public static implicit operator Android.Widget.ScrollView(ScrollView scrollView) => (Android.Widget.ScrollView)scrollView._nativeView.Value;

	static Android.Widget.ScrollView CreateVertical(Context context) => new(context);

	static HorizontalScrollView CreateHorizontal(Context context) => new(context);

	/// <summary>
	/// A scrollable container view that can hold a single child view.
	/// Android -> Android.Widget.ScrollView
	/// iOS -> UIKit.UIScrollView
	/// </summary>
	public ScrollView() : this(Platform.Context, CreateVertical) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public ScrollView(Context context) : this(context, CreateVertical) { }

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
		// Note: Changing orientation after creation requires recreating the native view
		// Android has separate ScrollView (vertical) and HorizontalScrollView classes
		// This is a limitation of Android's API design
		// For now, we'll throw an exception if orientation changes after creation
		if (_nativeView.IsValueCreated)
		{
			throw new NotSupportedException("Changing ScrollView orientation after creation is not supported on Android.");
		}
	}
}
