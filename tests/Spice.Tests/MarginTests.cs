namespace Spice.Tests;

/// <summary>
/// Tests for Margin property on View
/// </summary>
public class MarginTests
{
	[Fact]
	public void MarginDefaultsToZero()
	{
		var view = new View();
		Assert.Equal(new Thickness(0), view.Margin);
		Assert.True(view.Margin.IsEmpty);
	}

	[Fact]
	public void CanSetUniformMargin()
	{
		var view = new View
		{
			Margin = 10
		};
		Assert.Equal(new Thickness(10), view.Margin);
		Assert.Equal(10, view.Margin.Left);
		Assert.Equal(10, view.Margin.Top);
		Assert.Equal(10, view.Margin.Right);
		Assert.Equal(10, view.Margin.Bottom);
	}

	[Fact]
	public void CanSetHorizontalVerticalMargin()
	{
		var view = new View
		{
			Margin = new Thickness(10, 20)
		};
		Assert.Equal(10, view.Margin.Left);
		Assert.Equal(20, view.Margin.Top);
		Assert.Equal(10, view.Margin.Right);
		Assert.Equal(20, view.Margin.Bottom);
	}

	[Fact]
	public void CanSetIndividualMargins()
	{
		var view = new View
		{
			Margin = new Thickness(5, 10, 15, 20)
		};
		Assert.Equal(5, view.Margin.Left);
		Assert.Equal(10, view.Margin.Top);
		Assert.Equal(15, view.Margin.Right);
		Assert.Equal(20, view.Margin.Bottom);
	}

	[Fact]
	public void MarginPropertyChangedEventFires()
	{
		string? propertyName = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		view.Margin = 10;
		Assert.Equal(nameof(view.Margin), propertyName);
		Assert.Equal(new Thickness(10), view.Margin);
	}

	[Fact]
	public void MarginWorksOnButton()
	{
		var button = new Button
		{
			Text = "Click me",
			Margin = 15
		};
		Assert.Equal(new Thickness(15), button.Margin);
		Assert.Equal("Click me", button.Text);
	}

	[Fact]
	public void MarginWorksOnLabel()
	{
		var label = new Label
		{
			Text = "Hello",
			Margin = new Thickness(5, 10, 15, 20)
		};
		Assert.Equal(5, label.Margin.Left);
		Assert.Equal(10, label.Margin.Top);
		Assert.Equal(15, label.Margin.Right);
		Assert.Equal(20, label.Margin.Bottom);
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void MarginWorksOnContentView()
	{
		var contentView = new ContentView
		{
			Margin = 10,
			Content = new Label { Text = "Test" }
		};
		Assert.Equal(new Thickness(10), contentView.Margin);
		Assert.NotNull(contentView.Content);
	}

	[Fact]
	public void MarginWorksOnBoxView()
	{
		var boxView = new BoxView
		{
			Margin = new Thickness(8),
			BackgroundColor = Colors.Red
		};
		Assert.Equal(new Thickness(8), boxView.Margin);
		Assert.Equal(Colors.Red, boxView.BackgroundColor);
	}

	[Fact]
	public void CanChangeMargin()
	{
		var view = new View { Margin = 10 };
		Assert.Equal(new Thickness(10), view.Margin);

		view.Margin = 20;
		Assert.Equal(new Thickness(20), view.Margin);

		view.Margin = new Thickness(5, 10, 15, 20);
		Assert.Equal(5, view.Margin.Left);
		Assert.Equal(10, view.Margin.Top);
		Assert.Equal(15, view.Margin.Right);
		Assert.Equal(20, view.Margin.Bottom);
	}

	[Fact]
	public void MarginWorksWithAlignment()
	{
		var view = new View
		{
			Margin = 10,
			HorizontalAlign = Align.Start,
			VerticalAlign = Align.Start
		};
		Assert.Equal(new Thickness(10), view.Margin);
		Assert.Equal(Align.Start, view.HorizontalAlign);
		Assert.Equal(Align.Start, view.VerticalAlign);
	}

	[Fact]
	public void MarginWorksWithBackgroundColor()
	{
		var view = new View
		{
			Margin = 10,
			BackgroundColor = Colors.Blue
		};
		Assert.Equal(new Thickness(10), view.Margin);
		Assert.Equal(Colors.Blue, view.BackgroundColor);
	}

	[Fact]
	public void MarginAndPaddingAreSeparateProperties()
	{
		var border = new Border
		{
			Margin = 10,
			Padding = 5
		};
		Assert.Equal(new Thickness(10), border.Margin);
		Assert.Equal(5.0, border.Padding);
	}

	[Fact]
	public void CanSetMarginInStackLayout()
	{
		var stackView = new StackLayout();
		var button = new Button
		{
			Text = "Test",
			Margin = 10
		};

		stackView.Add(button);
		Assert.Single(stackView.Children);
		Assert.Same(button, stackView.Children[0]);
		Assert.Equal(new Thickness(10), button.Margin);
	}

	[Fact]
	public void MultipleChildrenCanHaveDifferentMargins()
	{
		var stackView = new StackLayout();
		var button1 = new Button { Text = "Button 1", Margin = 5 };
		var button2 = new Button { Text = "Button 2", Margin = 10 };
		var label = new Label { Text = "Label", Margin = new Thickness(2, 4, 6, 8) };

		stackView.Add(button1);
		stackView.Add(button2);
		stackView.Add(label);

		Assert.Equal(new Thickness(5), button1.Margin);
		Assert.Equal(new Thickness(10), button2.Margin);
		Assert.Equal(2, label.Margin.Left);
		Assert.Equal(4, label.Margin.Top);
		Assert.Equal(6, label.Margin.Right);
		Assert.Equal(8, label.Margin.Bottom);
	}
}
