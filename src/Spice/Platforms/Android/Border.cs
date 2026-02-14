using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace Spice;

public partial class Border
{
	/// <summary>
	/// Returns border.NativeView
	/// </summary>
	/// <param name="border">The Spice.Border</param>
	public static implicit operator Android.Views.View(Border border) => border.NativeView;

	static Android.Widget.FrameLayout Create(Context context) => new(context);

	/// <summary>
	/// Represents a border around a single child view. Set Content to a View, and customize with Stroke, StrokeThickness, CornerRadius, and Padding.
	/// Android -> FrameLayout with GradientDrawable background
	/// </summary>
	public Border() : base(Platform.Context, Create)
	{
		// Subscribe to PropertyChanged to intercept BackgroundColor changes
		// This is a self-reference and won't prevent garbage collection
		PropertyChanged += OnPropertyChanged;
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Border(Context context) : base(context, Create)
	{
		// Subscribe to PropertyChanged to intercept BackgroundColor changes
		// This is a self-reference and won't prevent garbage collection
		PropertyChanged += OnPropertyChanged;
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Border(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		// Subscribe to PropertyChanged to intercept BackgroundColor changes
		// This is a self-reference and won't prevent garbage collection
		PropertyChanged += OnPropertyChanged;
	}

	/// <summary>
	/// The underlying Android.Widget.FrameLayout
	/// </summary>
	public new Android.Widget.FrameLayout NativeView => (Android.Widget.FrameLayout)_nativeView.Value;

	GradientDrawable? _borderDrawable;

	GradientDrawable GetOrCreateBorderDrawable()
	{
		if (_borderDrawable == null)
		{
			_borderDrawable = new GradientDrawable();
			if (BackgroundColor != null)
			{
				_borderDrawable.SetColor(BackgroundColor.ToAndroidInt());
			}
			NativeView.Background = _borderDrawable;
		}
		return _borderDrawable;
	}

	partial void OnContentChanged(View? oldValue, View? newValue)
	{
		if (oldValue != null)
		{
			NativeView.RemoveView(oldValue.NativeView);
		}

		if (newValue != null)
		{
			NativeView.AddView(newValue.NativeView);
			UpdateContentPadding();
		}
	}

	partial void OnStrokeChanged(Color? value)
	{
		var drawable = GetOrCreateBorderDrawable();
		if (value != null)
		{
			drawable.SetStroke(StrokeThickness.ToPixels(), ColorStateList.ValueOf(value.ToAndroidColor()));
		}
		else
		{
			drawable.SetStroke(0, ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
		}
	}

	partial void OnStrokeThicknessChanged(double value)
	{
		var drawable = GetOrCreateBorderDrawable();
		if (Stroke != null)
		{
			drawable.SetStroke(value.ToPixels(), ColorStateList.ValueOf(Stroke.ToAndroidColor()));
		}
		else
		{
			drawable.SetStroke(value.ToPixels(), ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
		}
		UpdateContentPadding();
	}

	partial void OnCornerRadiusChanged(double value)
	{
		var drawable = GetOrCreateBorderDrawable();
		drawable.SetCornerRadius((float)value.ToPixels());
	}

	partial void OnPaddingChanged(double value)
	{
		UpdateContentPadding();
	}

	void UpdateContentPadding()
	{
		var paddingPx = (Padding + StrokeThickness).ToPixels();
		NativeView.SetPadding(paddingPx, paddingPx, paddingPx, paddingPx);
	}

	void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(BackgroundColor))
		{
			var drawable = GetOrCreateBorderDrawable();
			if (BackgroundColor != null)
			{
				drawable.SetColor(BackgroundColor.ToAndroidInt());
			}
			else
			{
				drawable.SetColor(Android.Graphics.Color.Transparent);
			}
		}
	}
}
