namespace Spice.Tests;

/// <summary>
/// Tests for LayoutOptions struct
/// </summary>
public class LayoutOptionsTests
{
	[Fact]
	public void LayoutOptionsStartHasCorrectAlignment()
	{
		Assert.Equal(LayoutAlignment.Start, LayoutOptions.Start.Alignment);
		Assert.False(LayoutOptions.Start.Expands);
	}

	[Fact]
	public void LayoutOptionsCenterHasCorrectAlignment()
	{
		Assert.Equal(LayoutAlignment.Center, LayoutOptions.Center.Alignment);
		Assert.False(LayoutOptions.Center.Expands);
	}

	[Fact]
	public void LayoutOptionsEndHasCorrectAlignment()
	{
		Assert.Equal(LayoutAlignment.End, LayoutOptions.End.Alignment);
		Assert.False(LayoutOptions.End.Expands);
	}

	[Fact]
	public void LayoutOptionsFillHasCorrectAlignment()
	{
		Assert.Equal(LayoutAlignment.Fill, LayoutOptions.Fill.Alignment);
		Assert.False(LayoutOptions.Fill.Expands);
	}

	[Fact]
	public void LayoutOptionsCanBeConstructed()
	{
		var options = new LayoutOptions(LayoutAlignment.Center, false);
		Assert.Equal(LayoutAlignment.Center, options.Alignment);
		Assert.False(options.Expands);
	}

	[Fact]
	public void LayoutOptionsCanBeConstructedWithExpands()
	{
		var options = new LayoutOptions(LayoutAlignment.Start, true);
		Assert.Equal(LayoutAlignment.Start, options.Alignment);
		Assert.True(options.Expands);
	}

	[Fact]
	public void LayoutOptionsEqualityWorks()
	{
		var options1 = LayoutOptions.Center;
		var options2 = LayoutOptions.Center;
		var options3 = LayoutOptions.Start;

		Assert.Equal(options1, options2);
		Assert.NotEqual(options1, options3);
		Assert.True(options1 == options2);
		Assert.False(options1 == options3);
		Assert.False(options1 != options2);
		Assert.True(options1 != options3);
	}

	[Fact]
	public void LayoutOptionsAlignmentCanBeSet()
	{
		var options = LayoutOptions.Center;
		options.Alignment = LayoutAlignment.End;
		Assert.Equal(LayoutAlignment.End, options.Alignment);
	}

	[Fact]
	public void LayoutOptionsExpandsCanBeSet()
	{
		var options = LayoutOptions.Center;
		options.Expands = true;
		Assert.True(options.Expands);
	}

	[Fact]
	public void LayoutOptionsGetHashCodeWorks()
	{
		var options1 = LayoutOptions.Center;
		var options2 = LayoutOptions.Center;
		var options3 = LayoutOptions.Start;

		Assert.Equal(options1.GetHashCode(), options2.GetHashCode());
		Assert.NotEqual(options1.GetHashCode(), options3.GetHashCode());
	}

	[Fact]
	public void LayoutOptionsEqualsWorksWithObject()
	{
		var options1 = LayoutOptions.Center;
		object options2 = LayoutOptions.Center;
		object options3 = LayoutOptions.Start;

		Assert.True(options1.Equals(options2));
		Assert.False(options1.Equals(options3));
		Assert.False(options1.Equals(null));
		Assert.False(options1.Equals("string"));
	}
}
