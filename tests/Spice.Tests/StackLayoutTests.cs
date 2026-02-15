namespace Spice.Tests;

public class StackLayoutTests
{
	[Fact]
	public void StackLayoutCanBeCreated()
	{
		var stackLayout = new StackLayout();
		Assert.NotNull(stackLayout);
	}

	[Fact]
	public void StackLayoutInheritsFromView()
	{
		var stackLayout = new StackLayout();
		Assert.IsAssignableFrom<View>(stackLayout);
	}

	[Fact]
	public void OrientationDefaultsToVertical()
	{
		var stackLayout = new StackLayout();
		Assert.Equal(Orientation.Vertical, stackLayout.Orientation);
	}

	[Fact]
	public void OrientationCanBeSetToHorizontal()
	{
		var stackLayout = new StackLayout { Orientation = Orientation.Horizontal };
		Assert.Equal(Orientation.Horizontal, stackLayout.Orientation);
	}

	[Fact]
	public void SpacingDefaultsToZero()
	{
		var stackLayout = new StackLayout();
		Assert.Equal(0, stackLayout.Spacing);
	}

	[Fact]
	public void SpacingCanBeSet()
	{
		var stackLayout = new StackLayout { Spacing = 10 };
		Assert.Equal(10, stackLayout.Spacing);
	}

	[Fact]
	public void PropertyChangedFiresOnOrientationChange()
	{
		var stackLayout = new StackLayout();
		var propertyChangedFired = false;
		stackLayout.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(StackLayout.Orientation))
				propertyChangedFired = true;
		};

		stackLayout.Orientation = Orientation.Horizontal;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnSpacingChange()
	{
		var stackLayout = new StackLayout();
		var propertyChangedFired = false;
		stackLayout.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(StackLayout.Spacing))
				propertyChangedFired = true;
		};

		stackLayout.Spacing = 5;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void CanAddChildrenToStackLayout()
	{
		var stackLayout = new StackLayout();
		var label = new Label { Text = "Test" };
		
		stackLayout.Children.Add(label);
		
		Assert.Single(stackLayout.Children);
		Assert.Same(label, stackLayout.Children[0]);
	}

	[Fact]
	public void CanAddMultipleChildrenToStackLayout()
	{
		var stackLayout = new StackLayout();
		var label1 = new Label { Text = "Label 1" };
		var label2 = new Label { Text = "Label 2" };
		var button = new Button { Text = "Button" };
		
		stackLayout.Children.Add(label1);
		stackLayout.Children.Add(label2);
		stackLayout.Children.Add(button);
		
		Assert.Equal(3, stackLayout.Children.Count);
		Assert.Same(label1, stackLayout.Children[0]);
		Assert.Same(label2, stackLayout.Children[1]);
		Assert.Same(button, stackLayout.Children[2]);
	}

	[Fact]
	public void CanRemoveChildrenFromStackLayout()
	{
		var stackLayout = new StackLayout();
		var label = new Label { Text = "Test" };
		
		stackLayout.Children.Add(label);
		Assert.Single(stackLayout.Children);
		
		stackLayout.Children.Remove(label);
		Assert.Empty(stackLayout.Children);
	}

	[Fact]
	public void CanClearChildrenFromStackLayout()
	{
		var stackLayout = new StackLayout();
		stackLayout.Children.Add(new Label { Text = "Label 1" });
		stackLayout.Children.Add(new Label { Text = "Label 2" });
		
		Assert.Equal(2, stackLayout.Children.Count);
		
		stackLayout.Children.Clear();
		Assert.Empty(stackLayout.Children);
	}

	[Fact]
	public void CanSetAllPropertiesAtOnce()
	{
		var stackLayout = new StackLayout
		{
			Orientation = Orientation.Horizontal,
			Spacing = 15
		};

		Assert.Equal(Orientation.Horizontal, stackLayout.Orientation);
		Assert.Equal(15, stackLayout.Spacing);
	}

	[Fact]
	public void NegativeSpacingIsAllowed()
	{
		// This might be useful for overlapping items
		var stackLayout = new StackLayout { Spacing = -5 };
		Assert.Equal(-5, stackLayout.Spacing);
	}
}
