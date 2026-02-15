namespace Spice.Tests;

/// <summary>
/// Tests for GridLength functionality
/// </summary>
public class GridLengthTests
{
	[Fact]
	public void AutoPropertyIsAuto()
	{
		var length = GridLength.Auto;
		Assert.True(length.IsAuto);
		Assert.False(length.IsAbsolute);
		Assert.False(length.IsStar);
		Assert.Equal(0, length.Value);
		Assert.Equal(GridUnitType.Auto, length.GridUnitType);
	}

	[Fact]
	public void StarPropertyIsStar()
	{
		var length = GridLength.Star;
		Assert.True(length.IsStar);
		Assert.False(length.IsAuto);
		Assert.False(length.IsAbsolute);
		Assert.Equal(1, length.Value);
		Assert.Equal(GridUnitType.Star, length.GridUnitType);
	}

	[Fact]
	public void CanCreateAbsoluteLength()
	{
		var length = new GridLength(100);
		Assert.True(length.IsAbsolute);
		Assert.False(length.IsAuto);
		Assert.False(length.IsStar);
		Assert.Equal(100, length.Value);
		Assert.Equal(GridUnitType.Absolute, length.GridUnitType);
	}

	[Fact]
	public void CanCreateStarLength()
	{
		var length = new GridLength(2, GridUnitType.Star);
		Assert.True(length.IsStar);
		Assert.False(length.IsAuto);
		Assert.False(length.IsAbsolute);
		Assert.Equal(2, length.Value);
		Assert.Equal(GridUnitType.Star, length.GridUnitType);
	}

	[Fact]
	public void CanCreateAutoLength()
	{
		var length = new GridLength(50, GridUnitType.Auto);
		Assert.True(length.IsAuto);
		Assert.False(length.IsAbsolute);
		Assert.False(length.IsStar);
		Assert.Equal(0, length.Value); // Auto always has value 0
		Assert.Equal(GridUnitType.Auto, length.GridUnitType);
	}

	[Fact]
	public void NegativeValueThrows()
	{
		Assert.Throws<ArgumentException>(() => new GridLength(-1));
	}

	[Fact]
	public void NaNValueThrows()
	{
		Assert.Throws<ArgumentException>(() => new GridLength(double.NaN));
	}

	[Fact]
	public void InfinityValueThrows()
	{
		Assert.Throws<ArgumentException>(() => new GridLength(double.PositiveInfinity));
	}

	[Fact]
	public void EqualityWorks()
	{
		var length1 = new GridLength(100);
		var length2 = new GridLength(100);
		var length3 = new GridLength(200);

		Assert.Equal(length1, length2);
		Assert.NotEqual(length1, length3);
		Assert.True(length1 == length2);
		Assert.True(length1 != length3);
		Assert.True(length1.Equals(length2));
		Assert.False(length1.Equals(length3));
	}

	[Fact]
	public void AutoToStringIsAuto()
	{
		var length = GridLength.Auto;
		Assert.Equal("Auto", length.ToString());
	}

	[Fact]
	public void SingleStarToStringIsStar()
	{
		var length = GridLength.Star;
		Assert.Equal("*", length.ToString());
	}

	[Fact]
	public void MultipleStarToStringHasMultiplier()
	{
		var length = new GridLength(2, GridUnitType.Star);
		Assert.Equal("2*", length.ToString());
	}

	[Fact]
	public void AbsoluteToStringIsValue()
	{
		var length = new GridLength(150);
		Assert.Equal("150", length.ToString());
	}

	[Fact]
	public void GetHashCodeWorks()
	{
		var length1 = new GridLength(100);
		var length2 = new GridLength(100);
		var length3 = GridLength.Auto;

		Assert.Equal(length1.GetHashCode(), length2.GetHashCode());
		Assert.NotEqual(length1.GetHashCode(), length3.GetHashCode());
	}

	[Fact]
	public void EqualsWithObjectWorks()
	{
		var length = new GridLength(100);
		object obj = new GridLength(100);
		object differentObj = new GridLength(200);
		object nullObj = null!;
		object wrongType = "string";

		Assert.True(length.Equals(obj));
		Assert.False(length.Equals(differentObj));
		Assert.False(length.Equals(nullObj));
		Assert.False(length.Equals(wrongType));
	}
}
