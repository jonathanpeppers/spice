namespace Spice.Tests;

/// <summary>
/// Tests for AutomationId property on View
/// </summary>
public class AutomationIdTests
{
	[Fact]
	public void AutomationIdDefaultsToNull()
	{
		var view = new View();
		Assert.Null(view.AutomationId);
	}

	[Fact]
	public void AutomationIdCanBeSet()
	{
		var view = new View
		{
			AutomationId = "test-view"
		};
		Assert.Equal("test-view", view.AutomationId);
	}

	[Fact]
	public void AutomationIdPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.AutomationId = "my-automation-id";
		Assert.Equal(nameof(view.AutomationId), propertyName);
		Assert.Equal("my-automation-id", view.AutomationId);
	}

	[Fact]
	public void AutomationIdWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			AutomationId = "button-test"
		};
		Assert.Equal("button-test", button.AutomationId);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void AutomationIdWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			AutomationId = "label-test"
		};
		Assert.Equal("label-test", label.AutomationId);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void AutomationIdWorksOnEntry()
	{
		var entry = new Entry
		{
			Text = "Input",
			AutomationId = "entry-test"
		};
		Assert.Equal("entry-test", entry.AutomationId);
		Assert.Equal("Input", entry.Text);
	}

	[Fact]
	public void AutomationIdWorksOnActivityIndicator()
	{
		var activityIndicator = new ActivityIndicator
		{
			IsRunning = true,
			AutomationId = "spinner-test"
		};
		Assert.Equal("spinner-test", activityIndicator.AutomationId);
		Assert.True(activityIndicator.IsRunning);
	}

	[Fact]
	public void CanChangeAutomationId()
	{
		var view = new View { AutomationId = "old-id" };
		Assert.Equal("old-id", view.AutomationId);

		view.AutomationId = "new-id";
		Assert.Equal("new-id", view.AutomationId);

		view.AutomationId = null;
		Assert.Null(view.AutomationId);
	}

	[Fact]
	public void AutomationIdWorksInStackLayout()
	{
		var stackView = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			AutomationId = "stack-button"
		};

		stackView.Add(button);
		Assert.Single(stackView.Children);
		Assert.Same(button, stackView.Children[0]);
		Assert.Equal("stack-button", button.AutomationId);
	}

	[Fact]
	public void AutomationIdCanBeSetAfterCreation()
	{
		var view = new View();
		Assert.Null(view.AutomationId);

		view.AutomationId = "late-id";
		Assert.Equal("late-id", view.AutomationId);
	}

	[Fact]
	public void AutomationIdWorksWithMultipleViews()
	{
		var view1 = new View { AutomationId = "view1" };
		var view2 = new View { AutomationId = "view2" };
		var view3 = new View { AutomationId = "view3" };

		Assert.Equal("view1", view1.AutomationId);
		Assert.Equal("view2", view2.AutomationId);
		Assert.Equal("view3", view3.AutomationId);
	}

	[Fact]
	public void AutomationIdWorksOnImage()
	{
		var image = new Image
		{
			AutomationId = "image-test"
		};
		Assert.Equal("image-test", image.AutomationId);
	}

	[Fact]
	public void AutomationIdWorksOnImageButton()
	{
		var imageButton = new ImageButton
		{
			AutomationId = "image-button-test"
		};
		Assert.Equal("image-button-test", imageButton.AutomationId);
	}
}
