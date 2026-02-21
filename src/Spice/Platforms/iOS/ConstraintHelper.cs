namespace Spice;

/// <summary>
/// Internal helper for managing UIKit Auto Layout constraints.
/// Provides utilities to pin edges, center views, and set sizes using constraints.
/// </summary>
static class ConstraintHelper
{
	/// <summary>
	/// Pins a child view to its parent on all four edges with the given insets.
	/// The child's TranslatesAutoresizingMaskIntoConstraints is set to false.
	/// Returns the four constraints (leading, trailing, top, bottom) for later updates.
	/// </summary>
	public static NSLayoutConstraint[] PinEdges(UIView child, UIView parent, nfloat top, nfloat leading, nfloat bottom, nfloat trailing)
	{
		child.TranslatesAutoresizingMaskIntoConstraints = false;

		var constraints = new[]
		{
			child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor, leading),
			parent.TrailingAnchor.ConstraintEqualTo(child.TrailingAnchor, trailing),
			child.TopAnchor.ConstraintEqualTo(parent.TopAnchor, top),
			parent.BottomAnchor.ConstraintEqualTo(child.BottomAnchor, bottom),
		};

		NSLayoutConstraint.ActivateConstraints(constraints);
		return constraints;
	}

	/// <summary>
	/// Pins a child view to its parent on all four edges with uniform inset.
	/// </summary>
	public static NSLayoutConstraint[] PinEdges(UIView child, UIView parent, nfloat inset = default)
	{
		return PinEdges(child, parent, inset, inset, inset, inset);
	}

	/// <summary>
	/// Updates the constants of an existing set of edge constraints (leading, trailing, top, bottom)
	/// returned by <see cref="PinEdges(UIView, UIView, nfloat, nfloat, nfloat, nfloat)"/>.
	/// </summary>
	public static void UpdateInsets(NSLayoutConstraint[] constraints, nfloat top, nfloat leading, nfloat bottom, nfloat trailing)
	{
		if (constraints.Length != 4)
			return;

		constraints[0].Constant = leading;   // leading
		constraints[1].Constant = trailing;  // trailing
		constraints[2].Constant = top;       // top
		constraints[3].Constant = bottom;    // bottom
	}

	/// <summary>
	/// Updates the constants of an existing set of edge constraints with uniform inset.
	/// </summary>
	public static void UpdateInsets(NSLayoutConstraint[] constraints, nfloat inset)
	{
		UpdateInsets(constraints, inset, inset, inset, inset);
	}

	/// <summary>
	/// Removes and deactivates a set of constraints.
	/// </summary>
	public static void RemoveConstraints(NSLayoutConstraint[]? constraints)
	{
		if (constraints == null)
			return;

		NSLayoutConstraint.DeactivateConstraints(constraints);
	}

	/// <summary>
	/// Applies alignment constraints to a child view within a parent based on Spice layout options,
	/// margin, and size requests. Returns the active constraints for later removal/update.
	/// </summary>
	public static NSLayoutConstraint[] ApplyAlignment(
		UIView child,
		UIView parent,
		LayoutAlignment horizontalAlignment,
		LayoutAlignment verticalAlignment,
		Thickness margin,
		double widthRequest,
		double heightRequest)
	{
		child.TranslatesAutoresizingMaskIntoConstraints = false;

		var constraints = new List<NSLayoutConstraint>();

		var marginLeft = (nfloat)margin.Left;
		var marginTop = (nfloat)margin.Top;
		var marginRight = (nfloat)margin.Right;
		var marginBottom = (nfloat)margin.Bottom;

		// Horizontal
		switch (horizontalAlignment)
		{
			case LayoutAlignment.Fill:
				if (widthRequest >= 0)
				{
					constraints.Add(child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor, marginLeft));
					constraints.Add(child.WidthAnchor.ConstraintEqualTo((nfloat)widthRequest));
				}
				else
				{
					constraints.Add(child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor, marginLeft));
					constraints.Add(parent.TrailingAnchor.ConstraintEqualTo(child.TrailingAnchor, marginRight));
				}
				break;
			case LayoutAlignment.Center:
				if (widthRequest >= 0)
				{
					constraints.Add(child.CenterXAnchor.ConstraintEqualTo(parent.CenterXAnchor, (marginLeft - marginRight) / 2));
					constraints.Add(child.WidthAnchor.ConstraintEqualTo((nfloat)widthRequest));
				}
				else
				{
					// Without an explicit width, anchor to both leading and trailing to avoid ambiguous width.
					constraints.Add(child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor, marginLeft));
					constraints.Add(parent.TrailingAnchor.ConstraintEqualTo(child.TrailingAnchor, marginRight));
				}
				break;
			case LayoutAlignment.Start:
				constraints.Add(child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor, marginLeft));
				if (widthRequest >= 0)
				{
					constraints.Add(child.WidthAnchor.ConstraintEqualTo((nfloat)widthRequest));
				}
				else
				{
					// Derive width from parent when no explicit width is requested.
					constraints.Add(parent.TrailingAnchor.ConstraintEqualTo(child.TrailingAnchor, marginRight));
				}
				break;
			case LayoutAlignment.End:
				constraints.Add(parent.TrailingAnchor.ConstraintEqualTo(child.TrailingAnchor, marginRight));
				if (widthRequest >= 0)
				{
					constraints.Add(child.WidthAnchor.ConstraintEqualTo((nfloat)widthRequest));
				}
				else
				{
					// Derive width from parent when no explicit width is requested.
					constraints.Add(child.LeadingAnchor.ConstraintEqualTo(parent.LeadingAnchor, marginLeft));
				}
				break;
		}

		// Vertical
		switch (verticalAlignment)
		{
			case LayoutAlignment.Fill:
				if (heightRequest >= 0)
				{
					constraints.Add(child.TopAnchor.ConstraintEqualTo(parent.TopAnchor, marginTop));
					constraints.Add(child.HeightAnchor.ConstraintEqualTo((nfloat)heightRequest));
				}
				else
				{
					constraints.Add(child.TopAnchor.ConstraintEqualTo(parent.TopAnchor, marginTop));
					constraints.Add(parent.BottomAnchor.ConstraintEqualTo(child.BottomAnchor, marginBottom));
				}
				break;
			case LayoutAlignment.Center:
				if (heightRequest >= 0)
				{
					constraints.Add(child.CenterYAnchor.ConstraintEqualTo(parent.CenterYAnchor, (marginTop - marginBottom) / 2));
					constraints.Add(child.HeightAnchor.ConstraintEqualTo((nfloat)heightRequest));
				}
				else
				{
					// Without an explicit height, anchor to both top and bottom to avoid ambiguous height.
					constraints.Add(child.TopAnchor.ConstraintEqualTo(parent.TopAnchor, marginTop));
					constraints.Add(parent.BottomAnchor.ConstraintEqualTo(child.BottomAnchor, marginBottom));
				}
				break;
			case LayoutAlignment.Start:
				constraints.Add(child.TopAnchor.ConstraintEqualTo(parent.TopAnchor, marginTop));
				if (heightRequest >= 0)
				{
					constraints.Add(child.HeightAnchor.ConstraintEqualTo((nfloat)heightRequest));
				}
				else
				{
					// Derive height from parent when no explicit height is requested.
					constraints.Add(parent.BottomAnchor.ConstraintEqualTo(child.BottomAnchor, marginBottom));
				}
				break;
			case LayoutAlignment.End:
				constraints.Add(parent.BottomAnchor.ConstraintEqualTo(child.BottomAnchor, marginBottom));
				if (heightRequest >= 0)
				{
					constraints.Add(child.HeightAnchor.ConstraintEqualTo((nfloat)heightRequest));
				}
				else
				{
					// Derive height from parent when no explicit height is requested.
					constraints.Add(child.TopAnchor.ConstraintEqualTo(parent.TopAnchor, marginTop));
				}
				break;
		}

		var result = constraints.ToArray();
		NSLayoutConstraint.ActivateConstraints(result);
		return result;
	}
}
