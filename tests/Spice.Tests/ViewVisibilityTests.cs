namespace Spice.Tests;

/// <summary>
/// Tests for IsVisible and IsEnabled properties on View
/// </summary>
public class ViewVisibilityTests
{
	[Fact]
	public void IsVisibleDefaultsToTrue()
	{
		var view = new View();
		Assert.True(view.IsVisible);
	}

	[Fact]
	public void IsEnabledDefaultsToTrue()
	{
		var view = new View();
		Assert.True(view.IsEnabled);
	}

	[Fact]
	public void IsVisibleCanBeSetToFalse()
	{
		var view = new View
		{
			IsVisible = false
		};
		Assert.False(view.IsVisible);
	}

	[Fact]
	public void IsEnabledCanBeSetToFalse()
	{
		var view = new View
		{
			IsEnabled = false
		};
		Assert.False(view.IsEnabled);
	}

	[Fact]
	public void IsVisiblePropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.IsVisible = false;
		Assert.Equal(nameof(view.IsVisible), propertyName);
		Assert.False(view.IsVisible);
	}

	[Fact]
	public void IsEnabledPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.IsEnabled = false;
		Assert.Equal(nameof(view.IsEnabled), propertyName);
		Assert.False(view.IsEnabled);
	}

	[Fact]
	public void IsVisibleWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			IsVisible = false
		};
		Assert.False(button.IsVisible);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void IsEnabledWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			IsEnabled = false
		};
		Assert.False(button.IsEnabled);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void IsVisibleWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			IsVisible = false
		};
		Assert.False(label.IsVisible);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void IsEnabledWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			IsEnabled = false
		};
		Assert.False(label.IsEnabled);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void IsVisibleWorksOnActivityIndicator()
	{
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			IsVisible = false
		};
		Assert.False(activityIndicator.IsVisible);
		Assert.True(activityIndicator.IsRunning);
	}

	[Fact]
	public void IsEnabledWorksOnActivityIndicator()
	{
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			IsEnabled = false
		};
		Assert.False(activityIndicator.IsEnabled);
		Assert.True(activityIndicator.IsRunning);
	}

	[Fact]
	public void CanToggleIsVisible()
	{
		var view = new View { IsVisible = false };
		Assert.False(view.IsVisible);

		view.IsVisible = true;
		Assert.True(view.IsVisible);

		view.IsVisible = false;
		Assert.False(view.IsVisible);
	}

	[Fact]
	public void CanToggleIsEnabled()
	{
		var view = new View { IsEnabled = false };
		Assert.False(view.IsEnabled);

		view.IsEnabled = true;
		Assert.True(view.IsEnabled);

		view.IsEnabled = false;
		Assert.False(view.IsEnabled);
	}

	[Fact]
	public void IsVisibleWorksInStackLayout()
	{
		var stackView = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			IsVisible = false
		};

		stackView.Add(button);
		Assert.Single(stackView.Children);
		Assert.Same(button, stackView.Children[0]);
		Assert.False(button.IsVisible);
	}

	[Fact]
	public void IsEnabledWorksInStackLayout()
	{
		var stackView = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			IsEnabled = false
		};

		stackView.Add(button);
		Assert.Single(stackView.Children);
		Assert.Same(button, stackView.Children[0]);
		Assert.False(button.IsEnabled);
	}
}
