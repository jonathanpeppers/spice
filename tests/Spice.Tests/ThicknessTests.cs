namespace Spice.Tests;

/// <summary>
/// Tests for Thickness struct
/// </summary>
public class ThicknessTests
{
	[Fact]
	public void DefaultConstructor()
	{
		var thickness = new Thickness();
		Assert.Equal(0, thickness.Left);
		Assert.Equal(0, thickness.Top);
		Assert.Equal(0, thickness.Right);
		Assert.Equal(0, thickness.Bottom);
	}

	[Fact]
	public void UniformConstructor()
	{
		var thickness = new Thickness(10);
		Assert.Equal(10, thickness.Left);
		Assert.Equal(10, thickness.Top);
		Assert.Equal(10, thickness.Right);
		Assert.Equal(10, thickness.Bottom);
	}

	[Fact]
	public void HorizontalVerticalConstructor()
	{
		var thickness = new Thickness(10, 20);
		Assert.Equal(10, thickness.Left);
		Assert.Equal(20, thickness.Top);
		Assert.Equal(10, thickness.Right);
		Assert.Equal(20, thickness.Bottom);
	}

	[Fact]
	public void IndividualSidesConstructor()
	{
		var thickness = new Thickness(5, 10, 15, 20);
		Assert.Equal(5, thickness.Left);
		Assert.Equal(10, thickness.Top);
		Assert.Equal(15, thickness.Right);
		Assert.Equal(20, thickness.Bottom);
	}

	[Fact]
	public void HorizontalThickness()
	{
		var thickness = new Thickness(5, 10, 15, 20);
		Assert.Equal(20, thickness.HorizontalThickness); // 5 + 15
	}

	[Fact]
	public void VerticalThickness()
	{
		var thickness = new Thickness(5, 10, 15, 20);
		Assert.Equal(30, thickness.VerticalThickness); // 10 + 20
	}

	[Fact]
	public void IsEmptyWhenAllZero()
	{
		var thickness = new Thickness(0);
		Assert.True(thickness.IsEmpty);
	}

	[Fact]
	public void IsNotEmptyWhenAnyNonZero()
	{
		Assert.False(new Thickness(1, 0, 0, 0).IsEmpty);
		Assert.False(new Thickness(0, 1, 0, 0).IsEmpty);
		Assert.False(new Thickness(0, 0, 1, 0).IsEmpty);
		Assert.False(new Thickness(0, 0, 0, 1).IsEmpty);
	}

	[Fact]
	public void ImplicitConversionFromDouble()
	{
		Thickness thickness = 10.5;
		Assert.Equal(10.5, thickness.Left);
		Assert.Equal(10.5, thickness.Top);
		Assert.Equal(10.5, thickness.Right);
		Assert.Equal(10.5, thickness.Bottom);
	}

	[Fact]
	public void EqualityOperator()
	{
		var t1 = new Thickness(10, 20, 30, 40);
		var t2 = new Thickness(10, 20, 30, 40);
		var t3 = new Thickness(5, 20, 30, 40);

		Assert.True(t1 == t2);
		Assert.False(t1 == t3);
	}

	[Fact]
	public void InequalityOperator()
	{
		var t1 = new Thickness(10, 20, 30, 40);
		var t2 = new Thickness(10, 20, 30, 40);
		var t3 = new Thickness(5, 20, 30, 40);

		Assert.False(t1 != t2);
		Assert.True(t1 != t3);
	}

	[Fact]
	public void EqualsMethod()
	{
		var t1 = new Thickness(10, 20, 30, 40);
		var t2 = new Thickness(10, 20, 30, 40);
		var t3 = new Thickness(5, 20, 30, 40);

		Assert.True(t1.Equals(t2));
		Assert.False(t1.Equals(t3));
	}

	[Fact]
	public void EqualsObjectMethod()
	{
		var t1 = new Thickness(10, 20, 30, 40);
		object t2 = new Thickness(10, 20, 30, 40);
		object t3 = new Thickness(5, 20, 30, 40);
		object notThickness = "string";

		Assert.True(t1.Equals(t2));
		Assert.False(t1.Equals(t3));
		Assert.False(t1.Equals(notThickness));
		Assert.False(t1.Equals(null));
	}

	[Fact]
	public void GetHashCodeConsistent()
	{
		var t1 = new Thickness(10, 20, 30, 40);
		var t2 = new Thickness(10, 20, 30, 40);

		Assert.Equal(t1.GetHashCode(), t2.GetHashCode());
	}

	[Fact]
	public void ToStringUniform()
	{
		var thickness = new Thickness(10);
		Assert.Equal("10", thickness.ToString());
	}

	[Fact]
	public void ToStringHorizontalVertical()
	{
		var thickness = new Thickness(10, 20);
		Assert.Equal("10,20", thickness.ToString());
	}

	[Fact]
	public void ToStringIndividual()
	{
		var thickness = new Thickness(5, 10, 15, 20);
		Assert.Equal("5,10,15,20", thickness.ToString());
	}

	[Fact]
	public void PropertiesCanBeSet()
	{
		var thickness = new Thickness
		{
			Left = 1,
			Top = 2,
			Right = 3,
			Bottom = 4
		};

		Assert.Equal(1, thickness.Left);
		Assert.Equal(2, thickness.Top);
		Assert.Equal(3, thickness.Right);
		Assert.Equal(4, thickness.Bottom);
	}
}
