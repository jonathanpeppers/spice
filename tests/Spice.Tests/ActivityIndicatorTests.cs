namespace Spice.Tests;

/// <summary>
/// Tests for ActivityIndicator control
/// </summary>
public class ActivityIndicatorTests
{
	[Fact]
	public void ActivityIndicatorCreation()
	{
		var activityIndicator = new ActivityIndicator();
		Assert.NotNull(activityIndicator);
		Assert.False(activityIndicator.IsRunning);
		Assert.Null(activityIndicator.Color);
	}

	[Fact]
	public void IsRunningProperty()
	{
		var activityIndicator = new ActivityIndicator();
		Assert.False(activityIndicator.IsRunning);

		activityIndicator.IsRunning = true;
		Assert.True(activityIndicator.IsRunning);

		activityIndicator.IsRunning = false;
		Assert.False(activityIndicator.IsRunning);
	}

	[Fact]
	public void ColorProperty()
	{
		var activityIndicator = new ActivityIndicator();
		Assert.Null(activityIndicator.Color);

		var red = new Color(255, 0, 0);
		activityIndicator.Color = red;
		Assert.Equal(red, activityIndicator.Color);

		var blue = new Color(0, 0, 255);
		activityIndicator.Color = blue;
		Assert.Equal(blue, activityIndicator.Color);

		activityIndicator.Color = null;
		Assert.Null(activityIndicator.Color);
	}

	[Fact]
	public void PropertyChangedEventsWork()
	{
		string? propertyName = null;
		var activityIndicator = new ActivityIndicator();
		activityIndicator.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		activityIndicator.IsRunning = true;
		Assert.Equal(nameof(activityIndicator.IsRunning), propertyName);

		activityIndicator.Color = new Color(0, 128, 0);
		Assert.Equal(nameof(activityIndicator.Color), propertyName);
	}

	[Fact]
	public void ActivityIndicatorInStackLayout()
	{
		var blue = new Color(0, 0, 255);
		var stackLayout = new StackLayout();
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			Color = blue
		};

		stackLayout.Add(activityIndicator);
		Assert.Single(stackLayout.Children);
		Assert.Same(activityIndicator, stackLayout.Children[0]);
		Assert.True(activityIndicator.IsRunning);
		Assert.Equal(blue, activityIndicator.Color);
	}
}
