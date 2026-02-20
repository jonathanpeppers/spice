namespace Spice;

public partial class Border
{
	/// <summary>
	/// Returns border.NativeView
	/// </summary>
	/// <param name="border">The Spice.Border</param>
	public static implicit operator UIView(Border border) => border.NativeView;

	/// <summary>
	/// Represents a border around a single child view. Set Content to a View, and customize with Stroke, StrokeThickness, CornerRadius, and Padding.
	/// iOS -> UIView with CALayer border
	/// </summary>
	public Border() : base(v => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Border(CGRect frame) : base(v => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Border(Func<View, UIView> creator) : base(creator) { }

	NSLayoutConstraint[]? _contentConstraints;

	partial void OnContentChanged(View? oldValue, View? newValue)
	{
		if (oldValue != null)
		{
			ConstraintHelper.RemoveConstraints(_contentConstraints);
			_contentConstraints = null;
			((UIView)oldValue).RemoveFromSuperview();
		}

		if (newValue != null)
		{
			UIView contentView = newValue;
			NativeView.AddSubview(contentView);
			var padding = (nfloat)Padding;
			_contentConstraints = ConstraintHelper.PinEdges(contentView, NativeView, padding);
		}
	}

	partial void OnStrokeChanged(Color? value)
	{
		if (value != null)
		{
			NativeView.Layer.BorderColor = value.ToCGColor();
			// Ensure default StrokeThickness is applied to native layer
			NativeView.Layer.BorderWidth = (nfloat)StrokeThickness;
		}
		else
		{
			NativeView.Layer.BorderColor = null;
		}
	}

	partial void OnStrokeThicknessChanged(double value)
	{
		NativeView.Layer.BorderWidth = (nfloat)value;
	}

	partial void OnCornerRadiusChanged(double value)
	{
		NativeView.Layer.CornerRadius = (nfloat)value;
		NativeView.Layer.MasksToBounds = value > 0;
	}

	partial void OnPaddingChanged(double value)
	{
		if (_contentConstraints != null)
		{
			var padding = (nfloat)value;
			ConstraintHelper.UpdateInsets(_contentConstraints, padding);
		}
	}

	/// <summary>
	/// Override to handle single child only.
	/// Border only supports the Content property for adding a single child view.
	/// Use the Content property instead of Children.Add() to set the border's content.
	/// </summary>
	protected override void AddSubview(View view)
	{
		// Do nothing - Border doesn't support Children collection
		// Only Content property should be used to add a view
	}
}
