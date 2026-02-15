namespace Spice.Tests;

/// <summary>
/// Tests for the default behavior and configuration of <see cref="SwipeItems"/>.
/// </summary>

public class SwipeItemsTests
{
	[Fact]
	public void SwipeItemsCanBeCreated()
	{
		var swipeItems = new SwipeItems();
		Assert.NotNull(swipeItems);
	}

	[Fact]
	public void ItemsCollectionIsInitialized()
	{
		var swipeItems = new SwipeItems();
		Assert.NotNull(swipeItems.Items);
		Assert.Empty(swipeItems.Items);
	}

	[Fact]
	public void ModeDefaultsToReveal()
	{
		var swipeItems = new SwipeItems();
		Assert.Equal(SwipeMode.Reveal, swipeItems.Mode);
	}

	[Fact]
	public void SwipeBehaviorOnInvokedDefaultsToAuto()
	{
		var swipeItems = new SwipeItems();
		Assert.Equal(SwipeBehaviorOnInvoked.Auto, swipeItems.SwipeBehaviorOnInvoked);
	}

	[Fact]
	public void ModeCanBeSet()
	{
		var swipeItems = new SwipeItems { Mode = SwipeMode.Execute };
		Assert.Equal(SwipeMode.Execute, swipeItems.Mode);
	}

	[Fact]
	public void SwipeBehaviorOnInvokedCanBeSet()
	{
		var swipeItems = new SwipeItems { SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Close };
		Assert.Equal(SwipeBehaviorOnInvoked.Close, swipeItems.SwipeBehaviorOnInvoked);
	}

	[Fact]
	public void CanAddItemsToCollection()
	{
		var swipeItems = new SwipeItems();
		var item = new SwipeItem { Text = "Delete" };
		
		swipeItems.Items.Add(item);
		
		Assert.Single(swipeItems.Items);
		Assert.Same(item, swipeItems.Items[0]);
	}

	[Fact]
	public void ConstructorWithItemsCollectionWorks()
	{
		var item1 = new SwipeItem { Text = "Delete" };
		var item2 = new SwipeItem { Text = "Archive" };
		var items = new[] { item1, item2 };
		
		var swipeItems = new SwipeItems(items);
		
		Assert.Equal(2, swipeItems.Items.Count);
		Assert.Same(item1, swipeItems.Items[0]);
		Assert.Same(item2, swipeItems.Items[1]);
	}

	[Fact]
	public void ConstructorWithNullItemsThrows()
	{
		Assert.Throws<ArgumentNullException>(() => new SwipeItems(null!));
	}

	[Fact]
	public void PropertyChangedFiresOnModeChange()
	{
		var swipeItems = new SwipeItems();
		var propertyChangedFired = false;
		swipeItems.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(SwipeItems.Mode))
				propertyChangedFired = true;
		};

		swipeItems.Mode = SwipeMode.Execute;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnSwipeBehaviorOnInvokedChange()
	{
		var swipeItems = new SwipeItems();
		var propertyChangedFired = false;
		swipeItems.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(SwipeItems.SwipeBehaviorOnInvoked))
				propertyChangedFired = true;
		};

		swipeItems.SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.RemainOpen;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void ConstructorWithEmptyCollectionWorks()
	{
		var swipeItems = new SwipeItems(Array.Empty<SwipeItem>());
		Assert.Empty(swipeItems.Items);
	}
}
