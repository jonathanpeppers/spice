﻿using Android.Content;

namespace Spice;

public partial class StackView
{
	/// <summary>
	/// Returns stackView.NativeView
	/// </summary>
	/// <param name="stackView">The Spice.StackView</param>
	public static implicit operator LinearLayout(StackView stackView) => stackView.NativeView;

	static LinearLayout Create(Context context)
	{
		var layout = new LinearLayout(context)
		{
			Orientation = Android.Widget.Orientation.Vertical,
		};
		layout.SetGravity(Android.Views.GravityFlags.Center);
		return layout;
	}

	/// <summary>
	/// A parent view for laying out child controls in a row. Defaults to Orientation=Vertical.
	/// Android -> Android.Widget.LinearLayout
	/// iOS -> UIKit.UIStackView
	/// </summary>
	public StackView() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public StackView(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected StackView(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected StackView(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.LinearLayout
	/// </summary>
	public new LinearLayout NativeView => (LinearLayout)_nativeView.Value;

	partial void OnOrientationChanging(Orientation value)
	{
		switch (value)
		{
			case Orientation.Vertical:
				NativeView.Orientation = Android.Widget.Orientation.Vertical;
				break;
			case Orientation.Horizontal:
				NativeView.Orientation = Android.Widget.Orientation.Horizontal;
				break;
			default:
				throw new NotSupportedException($"{nameof(Orientation)} value '{value}' not supported!");
		}
	}

	partial void OnSpacingChanged(int value) => NativeView.DividerPadding = value;
}