namespace Spice.Tests;

/// <summary>
/// Tests for the ImageButton control
/// </summary>
public class ImageButtonTests
{
	[Fact]
	public void ImageButtonCreation()
	{
		var imageButton = new ImageButton();
		Assert.NotNull(imageButton);
		Assert.Equal("", imageButton.Source);
		Assert.Null(imageButton.Clicked);
	}

	[Fact]
	public void SourcePropertyWorks()
	{
		var imageButton = new ImageButton { Source = "spice" };
		Assert.Equal("spice", imageButton.Source);
	}

	[Fact]
	public void SourcePropertyChangedEvent()
	{
		string? property = null;
		var imageButton = new ImageButton();
		imageButton.PropertyChanged += (sender, e) => property = e.PropertyName;
		imageButton.Source = "dotnet_bot";

		Assert.Equal(nameof(imageButton.Source), property);
		Assert.Equal("dotnet_bot", imageButton.Source);
	}

	[Fact]
	public void ClickedActionWorks()
	{
		var imageButton = new ImageButton();
		Assert.Null(imageButton.Clicked);

		imageButton.Clicked = _ => { };
		Assert.NotNull(imageButton.Clicked);
	}

	[Fact]
	public void ClickedActionCanBeCleared()
	{
		var imageButton = new ImageButton
		{
			Clicked = _ => { }
		};

		imageButton.Clicked = null;
		Assert.Null(imageButton.Clicked);
	}

	[Fact]
	public void ImageButtonInheritsFromView()
	{
		var imageButton = new ImageButton();
		Assert.IsAssignableFrom<View>(imageButton);
	}
}
