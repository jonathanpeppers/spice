using Android.Content;

namespace Spice;

public partial class ContentView
{
	/// <summary>
	/// Returns contentView.NativeView
	/// </summary>
	/// <param name="contentView">The Spice.ContentView</param>
	public static implicit operator Android.Views.ViewGroup(ContentView contentView) => (Android.Views.ViewGroup)contentView._nativeView.Value;

	static FrameLayout Create(Context context) => new(context);

	/// <summary>
	/// A container view that holds a single child view via the Content property.
	/// Android -> Android.Views.ViewGroup (FrameLayout)
	/// iOS -> UIKit.UIView
	/// </summary>
	public ContentView() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public ContentView(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ContentView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected ContentView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Views.ViewGroup (FrameLayout)
	/// </summary>
	public new Android.Views.ViewGroup NativeView => (Android.Views.ViewGroup)_nativeView.Value;

	partial void OnContentChanged(View? oldContent, View? newContent)
	{
		// Remove old content
		if (oldContent != null)
		{
			NativeView.RemoveView((Android.Views.View)oldContent);
		}

		// Add new content
		if (newContent != null)
		{
			NativeView.AddView(newContent);
		}
	}

	partial void OnPaddingChanged(double value)
	{
		var paddingPx = value.ToPixels();
		NativeView.SetPadding(paddingPx, paddingPx, paddingPx, paddingPx);
	}
}
