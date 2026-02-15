namespace Spice.Tests;

/// <summary>
/// Unit tests for the cross-platform Image view behavior and properties.
/// </summary>

public class ImageTests
{
	[Fact]
	public void ImageCanBeCreated()
	{
		var image = new Image();
		Assert.NotNull(image);
	}

	[Fact]
	public void ImageInheritsFromView()
	{
		var image = new Image();
		Assert.IsAssignableFrom<View>(image);
	}

	[Fact]
	public void SourcePropertyDefaultsToEmptyString()
	{
		var image = new Image();
		Assert.Equal("", image.Source);
	}

	[Fact]
	public void SourcePropertyCanBeSet()
	{
		var image = new Image { Source = "spice" };
		Assert.Equal("spice", image.Source);
	}

	[Fact]
	public void PropertyChangedFiresOnSourceChange()
	{
		var image = new Image();
		var propertyChangedFired = false;
		image.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Image.Source))
				propertyChangedFired = true;
		};

		image.Source = "icon";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void SourceCanBeSetMultipleTimes()
	{
		var image = new Image { Source = "first" };
		Assert.Equal("first", image.Source);
		
		image.Source = "second";
		Assert.Equal("second", image.Source);
		
		image.Source = "";
		Assert.Equal("", image.Source);
	}

	[Fact]
	public void MultipleImageInstancesAreIndependent()
	{
		var image1 = new Image { Source = "icon1" };
		var image2 = new Image { Source = "icon2" };

		Assert.Equal("icon1", image1.Source);
		Assert.Equal("icon2", image2.Source);
	}
}
