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

		activityIndicator.Color = Colors.Red;
		Assert.Equal(Colors.Red, activityIndicator.Color);

		activityIndicator.Color = Colors.Blue;
		Assert.Equal(Colors.Blue, activityIndicator.Color);

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

		activityIndicator.Color = Colors.Green;
		Assert.Equal(nameof(activityIndicator.Color), propertyName);
	}

	[Fact]
	public void ActivityIndicatorInStackView()
	{
		var stackView = new StackView();
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			Color = Colors.Blue
		};

		stackView.Add(activityIndicator);
		Assert.Equal(1, stackView.Children.Count);
		Assert.Same(activityIndicator, stackView.Children[0]);
		Assert.True(activityIndicator.IsRunning);
		Assert.Equal(Colors.Blue, activityIndicator.Color);
	}
}
