namespace Spice.Tests;

/// <summary>
/// Tests for Opacity property on View
/// </summary>
public class ViewOpacityTests
{
	[Fact]
	public void OpacityDefaultsToOne()
	{
		var view = new View();
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityCanBeSetToZero()
	{
		var view = new View
		{
			Opacity = 0.0
		};
		Assert.Equal(0.0, view.Opacity);
	}

	[Fact]
	public void OpacityCanBeSetToHalf()
	{
		var view = new View
		{
			Opacity = 0.5
		};
		Assert.Equal(0.5, view.Opacity);
	}

	[Fact]
	public void OpacityCanBeSetToOne()
	{
		var view = new View
		{
			Opacity = 1.0
		};
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.Opacity = 0.5;
		Assert.Equal(nameof(view.Opacity), propertyName);
		Assert.Equal(0.5, view.Opacity);
	}

	[Fact]
	public void OpacityWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			Opacity = 0.5
		};
		Assert.Equal(0.5, button.Opacity);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void OpacityWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			Opacity = 0.7
		};
		Assert.Equal(0.7, label.Opacity);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void OpacityWorksOnBoxView()
	{
		var boxView = new BoxView
		{
			BackgroundColor = Colors.Red,
			Opacity = 0.3
		};
		Assert.Equal(0.3, boxView.Opacity);
		Assert.Equal(Colors.Red, boxView.BackgroundColor);
	}

	[Fact]
	public void OpacityWorksOnActivityIndicator()
	{
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			Opacity = 0.8
		};
		Assert.Equal(0.8, activityIndicator.Opacity);
		Assert.True(activityIndicator.IsRunning);
	}

	[Fact]
	public void CanToggleOpacity()
	{
		var view = new View { Opacity = 0.0 };
		Assert.Equal(0.0, view.Opacity);

		view.Opacity = 0.5;
		Assert.Equal(0.5, view.Opacity);

		view.Opacity = 1.0;
		Assert.Equal(1.0, view.Opacity);

		view.Opacity = 0.25;
		Assert.Equal(0.25, view.Opacity);
	}

	[Fact]
	public void OpacityWorksInStackLayout()
	{
		var stackView = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			Opacity = 0.6
		};

		stackView.Add(button);
		Assert.Single(stackView.Children);
		Assert.Same(button, stackView.Children[0]);
		Assert.Equal(0.6, button.Opacity);
	}

	[Fact]
	public void OpacityWorksWithMultipleControls()
	{
		var label = new Label { Text = "Label", Opacity = 0.3 };
		var button = new Button { Text = "Button", Opacity = 0.5 };
		var boxView = new BoxView { BackgroundColor = Colors.Blue, Opacity = 0.7 };

		Assert.Equal(0.3, label.Opacity);
		Assert.Equal(0.5, button.Opacity);
		Assert.Equal(0.7, boxView.Opacity);
	}

	[Fact]
	public void OpacityCanBeSetAfterCreation()
	{
		var view = new View();
		Assert.Equal(1.0, view.Opacity);

		view.Opacity = 0.4;
		Assert.Equal(0.4, view.Opacity);
	}

	[Fact]
	public void OpacityWorksInContentView()
	{
		var contentView = new ContentView
		{
			Content = new Label { Text = "Inside", Opacity = 0.2 }
		};

		Assert.NotNull(contentView.Content);
		Assert.Equal(0.2, contentView.Content.Opacity);
	}
}
