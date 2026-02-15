namespace Spice;

/// <summary>
/// Represents the thickness of a frame around a rectangle. Four double values describe the Left, Top, Right, and Bottom sides of the rectangle.
/// Aligns with Microsoft.Maui.Thickness for compatibility.
/// </summary>
public struct Thickness : IEquatable<Thickness>
{
	/// <summary>
	/// Gets or sets the width of the left side of the bounding rectangle
	/// </summary>
	public double Left { get; set; }

	/// <summary>
	/// Gets or sets the width of the top of the bounding rectangle
	/// </summary>
	public double Top { get; set; }

	/// <summary>
	/// Gets or sets the width of the right side of the bounding rectangle
	/// </summary>
	public double Right { get; set; }

	/// <summary>
	/// Gets or sets the width of the bottom of the bounding rectangle
	/// </summary>
	public double Bottom { get; set; }

	/// <summary>
	/// Initializes a new instance of Thickness with a uniform thickness on all sides
	/// </summary>
	/// <param name="uniformSize">The uniform size applied to all sides</param>
	public Thickness(double uniformSize)
	{
		Left = Top = Right = Bottom = uniformSize;
	}

	/// <summary>
	/// Initializes a new instance of Thickness with horizontal and vertical thickness
	/// </summary>
	/// <param name="horizontalSize">The thickness on the left and right</param>
	/// <param name="verticalSize">The thickness on the top and bottom</param>
	public Thickness(double horizontalSize, double verticalSize)
	{
		Left = Right = horizontalSize;
		Top = Bottom = verticalSize;
	}

	/// <summary>
	/// Initializes a new instance of Thickness with different thickness values for each side
	/// </summary>
	/// <param name="left">The thickness on the left</param>
	/// <param name="top">The thickness on the top</param>
	/// <param name="right">The thickness on the right</param>
	/// <param name="bottom">The thickness on the bottom</param>
	public Thickness(double left, double top, double right, double bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}

	/// <summary>
	/// Gets the horizontal thickness (Left + Right)
	/// </summary>
	public double HorizontalThickness => Left + Right;

	/// <summary>
	/// Gets the vertical thickness (Top + Bottom)
	/// </summary>
	public double VerticalThickness => Top + Bottom;

	/// <summary>
	/// Returns true if all sides are zero
	/// </summary>
	public bool IsEmpty => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;

	/// <summary>
	/// Implicit conversion from double to Thickness with uniform size
	/// </summary>
	public static implicit operator Thickness(double uniformSize) => new(uniformSize);

	/// <summary>
	/// Determines whether two Thickness instances are equal
	/// </summary>
	public static bool operator ==(Thickness left, Thickness right) => left.Equals(right);

	/// <summary>
	/// Determines whether two Thickness instances are not equal
	/// </summary>
	public static bool operator !=(Thickness left, Thickness right) => !left.Equals(right);

	/// <summary>
	/// Determines whether this Thickness is equal to another Thickness
	/// </summary>
	public bool Equals(Thickness other) =>
		Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

	/// <summary>
	/// Determines whether this Thickness is equal to another object
	/// </summary>
	public override bool Equals(object? obj) => obj is Thickness other && Equals(other);

	/// <summary>
	/// Returns a hash code for this Thickness
	/// </summary>
	public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

	/// <summary>
	/// Returns a string representation of the Thickness
	/// </summary>
	public override string ToString()
	{
		if (Left == Top && Left == Right && Left == Bottom)
			return $"{Left}";
		if (Left == Right && Top == Bottom)
			return $"{Left},{Top}";
		return $"{Left},{Top},{Right},{Bottom}";
	}
}
