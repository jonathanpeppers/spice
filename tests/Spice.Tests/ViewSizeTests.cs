namespace Spice.Tests;

/// <summary>
/// Tests for Width, Height, WidthRequest, and HeightRequest properties on View
/// </summary>
public class ViewSizeTests
{
	[Fact]
	public void WidthRequestDefaultsToNegativeOne()
	{
		var view = new View();
		Assert.Equal(-1, view.WidthRequest);
	}

	[Fact]
	public void HeightRequestDefaultsToNegativeOne()
	{
		var view = new View();
		Assert.Equal(-1, view.HeightRequest);
	}

	[Fact]
	public void WidthRequestCanBeSet()
	{
		var view = new View
		{
			WidthRequest = 100
		};
		Assert.Equal(100, view.WidthRequest);
	}

	[Fact]
	public void HeightRequestCanBeSet()
	{
		var view = new View
		{
			HeightRequest = 200
		};
		Assert.Equal(200, view.HeightRequest);
	}

	[Fact]
	public void WidthRequestCanBeSetToZero()
	{
		var view = new View
		{
			WidthRequest = 0
		};
		Assert.Equal(0, view.WidthRequest);
	}

	[Fact]
	public void HeightRequestCanBeSetToZero()
	{
		var view = new View
		{
			HeightRequest = 0
		};
		Assert.Equal(0, view.HeightRequest);
	}

	[Fact]
	public void WidthRequestPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.WidthRequest = 150;
		Assert.Equal(nameof(view.WidthRequest), propertyName);
		Assert.Equal(150, view.WidthRequest);
	}

	[Fact]
	public void HeightRequestPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.HeightRequest = 250;
		Assert.Equal(nameof(view.HeightRequest), propertyName);
		Assert.Equal(250, view.HeightRequest);
	}

	[Fact]
	public void WidthRequestWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			WidthRequest = 120
		};
		Assert.Equal(120, button.WidthRequest);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void HeightRequestWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			HeightRequest = 50
		};
		Assert.Equal(50, button.HeightRequest);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void WidthRequestWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			WidthRequest = 80
		};
		Assert.Equal(80, label.WidthRequest);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void HeightRequestWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			HeightRequest = 30
		};
		Assert.Equal(30, label.HeightRequest);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void WidthRequestWorksOnBoxView()
	{
		var boxView = new BoxView
		{
			Color = new Color(255, 0, 0),
			WidthRequest = 100
		};
		Assert.Equal(100, boxView.WidthRequest);
	}

	[Fact]
	public void HeightRequestWorksOnBoxView()
	{
		var boxView = new BoxView
		{
			Color = new Color(255, 0, 0),
			HeightRequest = 100
		};
		Assert.Equal(100, boxView.HeightRequest);
	}

	[Fact]
	public void CanToggleWidthRequest()
	{
		var view = new View { WidthRequest = 100 };
		Assert.Equal(100, view.WidthRequest);

		view.WidthRequest = 200;
		Assert.Equal(200, view.WidthRequest);

		view.WidthRequest = -1;
		Assert.Equal(-1, view.WidthRequest);
	}

	[Fact]
	public void CanToggleHeightRequest()
	{
		var view = new View { HeightRequest = 100 };
		Assert.Equal(100, view.HeightRequest);

		view.HeightRequest = 200;
		Assert.Equal(200, view.HeightRequest);

		view.HeightRequest = -1;
		Assert.Equal(-1, view.HeightRequest);
	}

	[Fact]
	public void WidthRequestWorksInStackLayout()
	{
		var stackLayout = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			WidthRequest = 150
		};

		stackLayout.Add(button);
		Assert.Single(stackLayout.Children);
		Assert.Same(button, stackLayout.Children[0]);
		Assert.Equal(150, button.WidthRequest);
	}

	[Fact]
	public void HeightRequestWorksInStackLayout()
	{
		var stackLayout = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			HeightRequest = 60
		};

		stackLayout.Add(button);
		Assert.Single(stackLayout.Children);
		Assert.Same(button, stackLayout.Children[0]);
		Assert.Equal(60, button.HeightRequest);
	}

	[Fact]
	public void WidthAndHeightRequestWorkTogether()
	{
		var view = new View
		{
			WidthRequest = 100,
			HeightRequest = 200
		};
		Assert.Equal(100, view.WidthRequest);
		Assert.Equal(200, view.HeightRequest);
	}

	[Fact]
	public void SizeRequestsWorkWithVisibility()
	{
		var view = new View
		{
			WidthRequest = 100,
			HeightRequest = 200,
			IsVisible = false
		};
		Assert.Equal(100, view.WidthRequest);
		Assert.Equal(200, view.HeightRequest);
		Assert.False(view.IsVisible);
	}

	[Fact]
	public void SizeRequestsWorkWithAlignment()
	{
		var view = new View
		{
			WidthRequest = 100,
			HeightRequest = 200,
			HorizontalAlign = Align.Start,
			VerticalAlign = Align.End
		};
		Assert.Equal(100, view.WidthRequest);
		Assert.Equal(200, view.HeightRequest);
		Assert.Equal(Align.Start, view.HorizontalAlign);
		Assert.Equal(Align.End, view.VerticalAlign);
	}

	[Fact]
	public void WidthAndHeightPropertiesExist()
	{
		var view = new View();
		// Width and Height are read-only properties that return the actual rendered size
		// Without a layout pass, they should return 0 or a default value
		var width = view.Width;
		var height = view.Height;
		
		// Just verify the properties can be accessed
		Assert.True(width >= 0);
		Assert.True(height >= 0);
	}

	[Fact]
	public void WidthAndHeightWorkOnButton()
	{
		var button = new Button
		{
			Text = "Click me"
		};
		
		// Width and Height should be accessible
		var width = button.Width;
		var height = button.Height;
		
		Assert.True(width >= 0);
		Assert.True(height >= 0);
	}

	[Fact]
	public void WidthAndHeightWorkOnLabel()
	{
		var label = new Label
		{
			Text = "Hello"
		};
		
		// Width and Height should be accessible
		var width = label.Width;
		var height = label.Height;
		
		Assert.True(width >= 0);
		Assert.True(height >= 0);
	}

	[Fact]
	public void SettingWidthRequestWithHorizontalAlignStretch()
	{
		var view = new View
		{
			HorizontalAlign = Align.Stretch
		};

		view.WidthRequest = 200;

		Assert.Equal(200, view.WidthRequest);
		Assert.Equal(Align.Stretch, view.HorizontalAlign);
	}

	[Fact]
	public void SettingHeightRequestWithVerticalAlignStretch()
	{
		var view = new View
		{
			VerticalAlign = Align.Stretch
		};

		view.HeightRequest = 150;

		Assert.Equal(150, view.HeightRequest);
		Assert.Equal(Align.Stretch, view.VerticalAlign);
	}

	[Fact]
	public void ChangingAlignmentAfterSizeRequestsAreSetKeepsRequests()
	{
		var view = new View
		{
			HorizontalAlign = Align.Start,
			VerticalAlign = Align.Start
		};

		view.WidthRequest = 120;
		view.HeightRequest = 80;

		view.HorizontalAlign = Align.Stretch;
		view.VerticalAlign = Align.Stretch;

		Assert.Equal(120, view.WidthRequest);
		Assert.Equal(80, view.HeightRequest);
		Assert.Equal(Align.Stretch, view.HorizontalAlign);
		Assert.Equal(Align.Stretch, view.VerticalAlign);
	}
}
