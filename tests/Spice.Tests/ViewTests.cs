namespace Spice.Tests;

public class ViewTests
{
	[Fact]
	public void ViewCanBeCreated()
	{
		var view = new View();
		Assert.NotNull(view);
	}

	[Fact]
	public void ChildrenCollectionIsInitialized()
	{
		var view = new View();
		Assert.NotNull(view.Children);
		Assert.Empty(view.Children);
	}

	[Fact]
	public void HorizontalAlignDefaultsToCenter()
	{
		var view = new View();
		Assert.Equal(Align.Center, view.HorizontalAlign);
	}

	[Fact]
	public void VerticalAlignDefaultsToCenter()
	{
		var view = new View();
		Assert.Equal(Align.Center, view.VerticalAlign);
	}

	[Fact]
	public void BackgroundColorDefaultsToNull()
	{
		var view = new View();
		Assert.Null(view.BackgroundColor);
	}

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
	public void MarginDefaultsToZero()
	{
		var view = new View();
		Assert.Equal(0, view.Margin.Left);
		Assert.Equal(0, view.Margin.Top);
		Assert.Equal(0, view.Margin.Right);
		Assert.Equal(0, view.Margin.Bottom);
		Assert.True(view.Margin.IsEmpty);
	}

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
	public void CanSetHorizontalAlign()
	{
		var view = new View { HorizontalAlign = Align.Start };
		Assert.Equal(Align.Start, view.HorizontalAlign);
	}

	[Fact]
	public void CanSetVerticalAlign()
	{
		var view = new View { VerticalAlign = Align.End };
		Assert.Equal(Align.End, view.VerticalAlign);
	}

	[Fact]
	public void CanSetBackgroundColor()
	{
		var color = Colors.Blue;
		var view = new View { BackgroundColor = color };
		Assert.Equal(color, view.BackgroundColor);
	}

	[Fact]
	public void CanSetIsVisible()
	{
		var view = new View { IsVisible = false };
		Assert.False(view.IsVisible);
	}

	[Fact]
	public void CanSetIsEnabled()
	{
		var view = new View { IsEnabled = false };
		Assert.False(view.IsEnabled);
	}

	[Fact]
	public void CanSetMargin()
	{
		var view = new View { Margin = new Thickness(10, 20, 30, 40) };
		Assert.Equal(10, view.Margin.Left);
		Assert.Equal(20, view.Margin.Top);
		Assert.Equal(30, view.Margin.Right);
		Assert.Equal(40, view.Margin.Bottom);
	}

	[Fact]
	public void CanSetWidthRequest()
	{
		var view = new View { WidthRequest = 100 };
		Assert.Equal(100, view.WidthRequest);
	}

	[Fact]
	public void CanSetHeightRequest()
	{
		var view = new View { HeightRequest = 200 };
		Assert.Equal(200, view.HeightRequest);
	}

	[Fact]
	public void PropertyChangedFiresOnHorizontalAlignChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.HorizontalAlign))
				propertyChangedFired = true;
		};

		view.HorizontalAlign = Align.Stretch;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnVerticalAlignChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.VerticalAlign))
				propertyChangedFired = true;
		};

		view.VerticalAlign = Align.Stretch;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnBackgroundColorChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.BackgroundColor))
				propertyChangedFired = true;
		};

		view.BackgroundColor = Colors.Red;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnIsVisibleChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.IsVisible))
				propertyChangedFired = true;
		};

		view.IsVisible = false;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnIsEnabledChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.IsEnabled))
				propertyChangedFired = true;
		};

		view.IsEnabled = false;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnMarginChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.Margin))
				propertyChangedFired = true;
		};

		view.Margin = new Thickness(10);
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnWidthRequestChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.WidthRequest))
				propertyChangedFired = true;
		};

		view.WidthRequest = 150;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnHeightRequestChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.HeightRequest))
				propertyChangedFired = true;
		};

		view.HeightRequest = 250;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void CanAddChildToView()
	{
		var parent = new View();
		var child = new View();
		
		parent.Children.Add(child);
		
		Assert.Single(parent.Children);
		Assert.Same(child, parent.Children[0]);
	}

	[Fact]
	public void CanAddMultipleChildren()
	{
		var parent = new View();
		var child1 = new View();
		var child2 = new View();
		var child3 = new View();
		
		parent.Children.Add(child1);
		parent.Children.Add(child2);
		parent.Children.Add(child3);
		
		Assert.Equal(3, parent.Children.Count);
		Assert.Same(child1, parent.Children[0]);
		Assert.Same(child2, parent.Children[1]);
		Assert.Same(child3, parent.Children[2]);
	}

	[Fact]
	public void CanRemoveChild()
	{
		var parent = new View();
		var child = new View();
		
		parent.Children.Add(child);
		Assert.Single(parent.Children);
		
		parent.Children.Remove(child);
		Assert.Empty(parent.Children);
	}

	[Fact]
	public void AddMethodWorksForCollectionInitializer()
	{
		// This tests the Add() method that enables collection initializer syntax
		var parent = new View();
		var child = new View();
		
		parent.Add(child);
		
		Assert.Single(parent.Children);
		Assert.Same(child, parent.Children[0]);
	}

	[Fact]
	public void ViewIsEnumerable()
	{
		var parent = new View();
		var child1 = new View();
		var child2 = new View();
		
		parent.Children.Add(child1);
		parent.Children.Add(child2);
		
		var children = new List<View>();
		foreach (var child in parent)
		{
			children.Add(child);
		}
		
		Assert.Equal(2, children.Count);
		Assert.Same(child1, children[0]);
		Assert.Same(child2, children[1]);
	}

	[Fact]
	public void CanSetAllPropertiesAtOnce()
	{
		var view = new View
		{
			HorizontalAlign = Align.Start,
			VerticalAlign = Align.End,
			BackgroundColor = Colors.Green,
			IsVisible = false,
			IsEnabled = false,
			Margin = new Thickness(5),
			WidthRequest = 300,
			HeightRequest = 400
		};

		Assert.Equal(Align.Start, view.HorizontalAlign);
		Assert.Equal(Align.End, view.VerticalAlign);
		Assert.Equal(Colors.Green, view.BackgroundColor);
		Assert.False(view.IsVisible);
		Assert.False(view.IsEnabled);
		Assert.Equal(5, view.Margin.Left);
		Assert.Equal(300, view.WidthRequest);
		Assert.Equal(400, view.HeightRequest);
	}
}
