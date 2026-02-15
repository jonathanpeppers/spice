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
	public Border() : base(v => new SpiceBorderView((Border)v) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Border(CGRect frame) : base(v => new SpiceBorderView((Border)v, frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Border(Func<View, UIView> creator) : base(creator) { }

	partial void OnContentChanged(View? oldValue, View? newValue)
	{
		if (oldValue != null)
		{
			((UIView)oldValue).RemoveFromSuperview();
		}

		if (newValue != null)
		{
			NativeView.AddSubview(newValue);
			UpdateContentLayout();
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
		UpdateContentLayout();
	}

	void UpdateContentLayout()
	{
		if (Content == null)
			return;

		var padding = (nfloat)Padding;
		var contentView = (UIView)Content;
		var bounds = NativeView.Bounds;

		contentView.Frame = new CGRect(
			padding,
			padding,
			bounds.Width - (padding * 2),
			bounds.Height - (padding * 2)
		);
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

	class SpiceBorderView : UIView
	{
		readonly Border _parent;

		public SpiceBorderView(Border parent) => _parent = parent;

		public SpiceBorderView(Border parent, CGRect frame) : base(frame) => _parent = parent;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			_parent.UpdateContentLayout();
		}
	}
}
