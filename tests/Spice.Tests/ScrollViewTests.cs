namespace Spice.Tests;

/// <summary>
/// Tests for ScrollView functionality
/// </summary>
public class ScrollViewTests
{
	[Fact]
	public void CanCreate()
	{
		var scrollView = new ScrollView();
		Assert.NotNull(scrollView);
	}

	[Fact]
	public void DefaultOrientationIsVertical()
	{
		var scrollView = new ScrollView();
		Assert.Equal(Orientation.Vertical, scrollView.Orientation);
	}

	[Fact]
	public void CanSetOrientation()
	{
		var scrollView = new ScrollView
		{
			Orientation = Orientation.Horizontal
		};
		Assert.Equal(Orientation.Horizontal, scrollView.Orientation);
	}

	[Fact]
	public void CanAddChild()
	{
		var scrollView = new ScrollView();
		var label = new Label { Text = "Test" };
		scrollView.Add(label);
		
		Assert.Single(scrollView.Children);
		Assert.Equal(label, scrollView.Children[0]);
	}

	[Fact]
	public void AddingMultipleChildrenTracksAll()
	{
		var scrollView = new ScrollView();
		var first = new Label { Text = "First" };
		var second = new Label { Text = "Second" };

		scrollView.Add(first);
		scrollView.Add(second);

		Assert.Equal(2, scrollView.Children.Count);
		Assert.Equal(first, scrollView.Children[0]);
		Assert.Equal(second, scrollView.Children[1]);
	}

	[Fact]
	public void PropertyChangedFiresOnOrientationChange()
	{
		string? property = null;
		var scrollView = new ScrollView();
		scrollView.PropertyChanged += (sender, e) => property = e.PropertyName;
		scrollView.Orientation = Orientation.Horizontal;

		Assert.Equal(nameof(scrollView.Orientation), property);
	}

	[Fact]
	public void CanContainStackView()
	{
		var scrollView = new ScrollView();
		var stackView = new StackView
		{
			new Label { Text = "Item 1" },
			new Label { Text = "Item 2" },
			new Label { Text = "Item 3" }
		};
		scrollView.Add(stackView);
		
		Assert.Single(scrollView.Children);
		Assert.Equal(stackView, scrollView.Children[0]);
		Assert.Equal(3, stackView.Children.Count);
	}
}
