namespace Spice.Tests;

/// <summary>
/// Tests for Opacity property on View
/// </summary>
public class OpacityTests
{
	[Fact]
	public void OpacityDefaultsToOne()
	{
		var view = new View();
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityCanBeSet()
	{
		var view = new View
		{
			Opacity = 0.5
		};
		Assert.Equal(0.5, view.Opacity);
	}

	[Fact]
	public void OpacityPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.Opacity = 0.75;
		Assert.Equal(nameof(view.Opacity), propertyName);
		Assert.Equal(0.75, view.Opacity);
	}

	[Fact]
	public void OpacityWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			Opacity = 0.8
		};
		Assert.Equal(0.8, button.Opacity);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void OpacityWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			Opacity = 0.6
		};
		Assert.Equal(0.6, label.Opacity);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void OpacityWorksOnImage()
	{
		var image = new Image
		{
			Opacity = 0.4
		};
		Assert.Equal(0.4, image.Opacity);
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
	public void OpacityCanBeSetToOne()
	{
		var view = new View
		{
			Opacity = 1.0
		};
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityValueAboveOneIsClampedToOne()
	{
		var view = new View
		{
			Opacity = 1.5
		};
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityValueBelowZeroIsClampedToZero()
	{
		var view = new View
		{
			Opacity = -0.5
		};
		Assert.Equal(0.0, view.Opacity);
	}

	[Fact]
	public void OpacityCanBeChanged()
	{
		var view = new View { Opacity = 0.5 };
		Assert.Equal(0.5, view.Opacity);

		view.Opacity = 0.25;
		Assert.Equal(0.25, view.Opacity);

		view.Opacity = 1.0;
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityWorksInStackLayout()
	{
		var stackView = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			Opacity = 0.3
		};

		stackView.Add(button);
		Assert.Single(stackView.Children);
		Assert.Same(button, stackView.Children[0]);
		Assert.Equal(0.3, button.Opacity);
	}

	[Fact]
	public void OpacityWorksWithMultipleViews()
	{
		var view1 = new View { Opacity = 0.1 };
		var view2 = new View { Opacity = 0.5 };
		var view3 = new View { Opacity = 0.9 };

		Assert.Equal(0.1, view1.Opacity);
		Assert.Equal(0.5, view2.Opacity);
		Assert.Equal(0.9, view3.Opacity);
	}

	[Fact]
	public void OpacityWorksOnActivityIndicator()
	{
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			Opacity = 0.7
		};
		Assert.Equal(0.7, activityIndicator.Opacity);
		Assert.True(activityIndicator.IsRunning);
	}

	[Fact]
	public void OpacityWorksOnBoxView()
	{
		var boxView = new BoxView
		{
			BackgroundColor = Colors.Blue,
			Opacity = 0.5
		};
		Assert.Equal(0.5, boxView.Opacity);
		Assert.Equal(Colors.Blue, boxView.BackgroundColor);
	}

	[Fact]
	public void OpacityCanBeSetAfterCreation()
	{
		var view = new View();
		Assert.Equal(1.0, view.Opacity);

		view.Opacity = 0.2;
		Assert.Equal(0.2, view.Opacity);
	}

	[Fact]
	public void OpacityExtremelyLargeValueIsClamped()
	{
		var view = new View
		{
			Opacity = 1000.0
		};
		Assert.Equal(1.0, view.Opacity);
	}

	[Fact]
	public void OpacityExtremelyNegativeValueIsClamped()
	{
		var view = new View
		{
			Opacity = -1000.0
		};
		Assert.Equal(0.0, view.Opacity);
	}

	[Fact]
	public void OpacityWorksWithIsVisible()
	{
		var view = new View
		{
			Opacity = 0.5,
			IsVisible = false
		};
		Assert.Equal(0.5, view.Opacity);
		Assert.False(view.IsVisible);
	}

	[Fact]
	public void OpacityWorksWithIsEnabled()
	{
		var button = new Button
		{
			Text = "Test",
			Opacity = 0.4,
			IsEnabled = false
		};
		Assert.Equal(0.4, button.Opacity);
		Assert.False(button.IsEnabled);
	}
}
