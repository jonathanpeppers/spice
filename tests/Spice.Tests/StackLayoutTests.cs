namespace Spice.Tests;

/// <summary>
/// Tests for StackLayout functionality
/// </summary>
public class StackLayoutTests
{
	[Fact]
	public void CanCreate()
	{
		var stackLayout = new StackLayout();
		Assert.NotNull(stackLayout);
	}

	[Fact]
	public void DefaultOrientationIsVertical()
	{
		var stackLayout = new StackLayout();
		Assert.Equal(Orientation.Vertical, stackLayout.Orientation);
	}

	[Fact]
	public void DefaultSpacingIsZero()
	{
		var stackLayout = new StackLayout();
		Assert.Equal(0.0, stackLayout.Spacing);
	}

	[Fact]
	public void CanSetOrientation()
	{
		var stackLayout = new StackLayout
		{
			Orientation = Orientation.Horizontal
		};
		Assert.Equal(Orientation.Horizontal, stackLayout.Orientation);
	}

	[Fact]
	public void CanSetSpacing()
	{
		var stackLayout = new StackLayout
		{
			Spacing = 10.0
		};
		Assert.Equal(10.0, stackLayout.Spacing);
	}

	[Fact]
	public void DefaultPaddingIsZero()
	{
		var stackLayout = new StackLayout();
		Assert.Equal(0.0, stackLayout.Padding);
	}

	[Fact]
	public void CanSetPadding()
	{
		var stackLayout = new StackLayout
		{
			Padding = 8.0
		};
		Assert.Equal(8.0, stackLayout.Padding);
	}

	[Fact]
	public void PropertyChangedFiresOnPaddingChange()
	{
		string? property = null;
		var stackLayout = new StackLayout();
		stackLayout.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		stackLayout.Padding = 12.0;

		Assert.Equal(nameof(stackLayout.Padding), property);
	}

	[Fact]
	public void CanAddChildren()
	{
		var stackLayout = new StackLayout();
		var label1 = new Label { Text = "First" };
		var label2 = new Label { Text = "Second" };
		
		stackLayout.Add(label1);
		stackLayout.Add(label2);

		Assert.Equal(2, stackLayout.Children.Count);
		Assert.Equal(label1, stackLayout.Children[0]);
		Assert.Equal(label2, stackLayout.Children[1]);
	}

	[Fact]
	public void PropertyChangedFiresOnOrientationChange()
	{
		string? property = null;
		var stackLayout = new StackLayout();
		stackLayout.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		stackLayout.Orientation = Orientation.Horizontal;

		Assert.Equal(nameof(stackLayout.Orientation), property);
	}

	[Fact]
	public void PropertyChangedFiresOnSpacingChange()
	{
		string? property = null;
		var stackLayout = new StackLayout();
		stackLayout.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		stackLayout.Spacing = 5.0;

		Assert.Equal(nameof(stackLayout.Spacing), property);
	}
}
