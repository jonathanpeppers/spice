namespace Spice.Tests;

/// <summary>
/// Tests for BoxView control
/// </summary>
public class BoxViewTests
{
	[Fact]
	public void BoxViewCreation()
	{
		var boxView = new BoxView();
		Assert.NotNull(boxView);
		Assert.Null(boxView.Color);
	}

	[Fact]
	public void ColorProperty()
	{
		var boxView = new BoxView();
		Assert.Null(boxView.Color);

		var red = new Color(255, 0, 0);
		boxView.Color = red;
		Assert.Equal(red, boxView.Color);

		var blue = new Color(0, 0, 255);
		boxView.Color = blue;
		Assert.Equal(blue, boxView.Color);

		boxView.Color = null;
		Assert.Null(boxView.Color);
	}

	[Fact]
	public void PropertyChangedEventsWork()
	{
		string? propertyName = null;
		var boxView = new BoxView();
		boxView.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		boxView.Color = new Color(0, 128, 0);
		Assert.Equal(nameof(boxView.Color), propertyName);
	}

	[Fact]
	public void BoxViewInStackView()
	{
		var green = new Color(0, 255, 0);
		var stackView = new StackView();
		var boxView = new BoxView
		{
			Color = green
		};

		stackView.Add(boxView);
		Assert.Single(stackView.Children);
		Assert.Same(boxView, stackView.Children[0]);
		Assert.Equal(green, boxView.Color);
	}
}
