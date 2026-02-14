namespace Spice;

/// <summary>
/// Represents the length of elements that support GridUnitType-based units (like rows and columns in a Grid).
/// </summary>
public struct GridLength : IEquatable<GridLength>
{
	/// <summary>
	/// Initializes a new instance of GridLength using the specified absolute value in pixels.
	/// </summary>
	public GridLength(double value) : this(value, GridUnitType.Absolute)
	{
	}

	/// <summary>
	/// Initializes a new instance of GridLength using the specified absolute value and unit type.
	/// </summary>
	public GridLength(double value, GridUnitType type)
	{
		if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
			throw new ArgumentException("Invalid value", nameof(value));

		if (type == GridUnitType.Auto)
			value = 0;

		Value = value;
		GridUnitType = type;
	}

	/// <summary>
	/// Gets a GridLength value that is set to hold a value whose size is determined by the size properties of the content object.
	/// </summary>
	public static GridLength Auto { get; } = new GridLength(0, GridUnitType.Auto);

	/// <summary>
	/// Gets a GridLength value that represents the default size.
	/// </summary>
	public static GridLength Star { get; } = new GridLength(1, GridUnitType.Star);

	/// <summary>
	/// Gets the GridUnitType of the GridLength.
	/// </summary>
	public GridUnitType GridUnitType { get; }

	/// <summary>
	/// Gets a value that indicates whether the GridLength has a GridUnitType of Auto.
	/// </summary>
	public bool IsAuto => GridUnitType == GridUnitType.Auto;

	/// <summary>
	/// Gets a value that indicates whether the GridLength has a GridUnitType of Absolute.
	/// </summary>
	public bool IsAbsolute => GridUnitType == GridUnitType.Absolute;

	/// <summary>
	/// Gets a value that indicates whether the GridLength has a GridUnitType of Star.
	/// </summary>
	public bool IsStar => GridUnitType == GridUnitType.Star;

	/// <summary>
	/// Gets the value associated with the GridLength.
	/// </summary>
	public double Value { get; }

	/// <summary>
	/// Determines whether the specified GridLength is equal to the current GridLength.
	/// </summary>
	public bool Equals(GridLength other) =>
		GridUnitType == other.GridUnitType && Math.Abs(Value - other.Value) < double.Epsilon;

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is GridLength other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode() => HashCode.Combine(GridUnitType, Value);

	/// <summary>
	/// Compares two GridLength structures for equality.
	/// </summary>
	public static bool operator ==(GridLength left, GridLength right) => left.Equals(right);

	/// <summary>
	/// Compares two GridLength structures for inequality.
	/// </summary>
	public static bool operator !=(GridLength left, GridLength right) => !left.Equals(right);

	/// <inheritdoc/>
	public override string ToString()
	{
		return GridUnitType switch
		{
			GridUnitType.Auto => "Auto",
			GridUnitType.Star => Value == 1 ? "*" : $"{Value}*",
			GridUnitType.Absolute => Value.ToString(),
			_ => Value.ToString()
		};
	}
}
