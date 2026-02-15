namespace Spice.Tests;

/// <summary>
/// Tests for SwipeView functionality
/// </summary>
public class SwipeViewTests
{
	[Fact]
	public void CanCreate()
	{
		var swipeView = new SwipeView();
		Assert.NotNull(swipeView);
	}

	[Fact]
	public void ContentIsNullByDefault()
	{
		var swipeView = new SwipeView();
		Assert.Null(swipeView.Content);
	}

	[Fact]
	public void CanSetContent()
	{
		var swipeView = new SwipeView();
		var label = new Label { Text = "Test" };

		swipeView.Content = label;

		Assert.NotNull(swipeView.Content);
		Assert.Equal(label, swipeView.Content);
	}

	[Fact]
	public void SwipeItemsAreNullByDefault()
	{
		var swipeView = new SwipeView();
		Assert.Null(swipeView.LeftItems);
		Assert.Null(swipeView.RightItems);
		Assert.Null(swipeView.TopItems);
		Assert.Null(swipeView.BottomItems);
	}

	[Fact]
	public void CanSetLeftItems()
	{
		var swipeView = new SwipeView();
		var items = new SwipeItems();
		items.Items.Add(new SwipeItem { Text = "Delete" });

		swipeView.LeftItems = items;

		Assert.NotNull(swipeView.LeftItems);
		Assert.Single(swipeView.LeftItems.Items);
	}

	[Fact]
	public void CanSetRightItems()
	{
		var swipeView = new SwipeView();
		var items = new SwipeItems();
		items.Items.Add(new SwipeItem { Text = "Archive" });

		swipeView.RightItems = items;

		Assert.NotNull(swipeView.RightItems);
		Assert.Single(swipeView.RightItems.Items);
	}

	[Fact]
	public void CanSetTopItems()
	{
		var swipeView = new SwipeView();
		var items = new SwipeItems();
		items.Items.Add(new SwipeItem { Text = "Flag" });

		swipeView.TopItems = items;

		Assert.NotNull(swipeView.TopItems);
		Assert.Single(swipeView.TopItems.Items);
	}

	[Fact]
	public void CanSetBottomItems()
	{
		var swipeView = new SwipeView();
		var items = new SwipeItems();
		items.Items.Add(new SwipeItem { Text = "Share" });

		swipeView.BottomItems = items;

		Assert.NotNull(swipeView.BottomItems);
		Assert.Single(swipeView.BottomItems.Items);
	}

	[Fact]
	public void SwipeItemHasDefaultProperties()
	{
		var swipeItem = new SwipeItem();

		Assert.Null(swipeItem.Text);
		Assert.Null(swipeItem.BackgroundColor);
		Assert.True(swipeItem.IsVisible);
		Assert.Null(swipeItem.Invoked);
	}

	[Fact]
	public void CanSetSwipeItemProperties()
	{
		var swipeItem = new SwipeItem
		{
			Text = "Delete",
			BackgroundColor = Colors.Red,
			IsVisible = false
		};

		Assert.Equal("Delete", swipeItem.Text);
		Assert.Equal(Colors.Red, swipeItem.BackgroundColor);
		Assert.False(swipeItem.IsVisible);
	}

	[Fact]
	public void SwipeItemInvokedFiresAction()
	{
		bool invoked = false;
		var swipeItem = new SwipeItem
		{
			Text = "Test",
			Invoked = item => invoked = true
		};

		swipeItem.Invoked?.Invoke(swipeItem);

		Assert.True(invoked);
	}

	[Fact]
	public void SwipeItemsHasDefaultMode()
	{
		var swipeItems = new SwipeItems();
		Assert.Equal(SwipeMode.Reveal, swipeItems.Mode);
	}

	[Fact]
	public void SwipeItemsHasDefaultBehavior()
	{
		var swipeItems = new SwipeItems();
		Assert.Equal(SwipeBehaviorOnInvoked.Auto, swipeItems.SwipeBehaviorOnInvoked);
	}

	[Fact]
	public void CanSetSwipeItemsMode()
	{
		var swipeItems = new SwipeItems { Mode = SwipeMode.Execute };
		Assert.Equal(SwipeMode.Execute, swipeItems.Mode);
	}

	[Fact]
	public void CanSetSwipeItemsBehavior()
	{
		var swipeItems = new SwipeItems { SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.RemainOpen };
		Assert.Equal(SwipeBehaviorOnInvoked.RemainOpen, swipeItems.SwipeBehaviorOnInvoked);
	}

	[Fact]
	public void CanCreateSwipeItemsWithCollection()
	{
		var items = new List<SwipeItem>
		{
			new SwipeItem { Text = "Delete" },
			new SwipeItem { Text = "Archive" }
		};

		var swipeItems = new SwipeItems(items);

		Assert.Equal(2, swipeItems.Items.Count);
		Assert.Equal("Delete", swipeItems.Items[0].Text);
		Assert.Equal("Archive", swipeItems.Items[1].Text);
	}

	[Fact]
	public void ThresholdDefaultsToZero()
	{
		var swipeView = new SwipeView();
		Assert.Equal(0, swipeView.Threshold);
	}

	[Fact]
	public void CanSetThreshold()
	{
		var swipeView = new SwipeView { Threshold = 100 };
		Assert.Equal(100, swipeView.Threshold);
	}

	[Fact]
	public void PropertyChangedFiresOnContentChange()
	{
		string? property = null;
		var swipeView = new SwipeView();
		swipeView.PropertyChanged += (sender, e) => property = e.PropertyName;

		var label = new Label { Text = "Test" };
		swipeView.Content = label;

		Assert.Equal(nameof(swipeView.Content), property);
	}

	[Fact]
	public void PropertyChangedFiresOnLeftItemsChange()
	{
		string? property = null;
		var swipeView = new SwipeView();
		swipeView.PropertyChanged += (sender, e) => property = e.PropertyName;

		swipeView.LeftItems = new SwipeItems();

		Assert.Equal(nameof(swipeView.LeftItems), property);
	}

	[Fact]
	public void CanContainComplexContent()
	{
		var swipeView = new SwipeView();
		var stackLayout = new StackLayout
		{
			new Label { Text = "Title" },
			new Label { Text = "Description" },
			new Button { Text = "Action" }
		};

		swipeView.Content = stackLayout;

		Assert.Equal(stackLayout, swipeView.Content);
		Assert.Equal(3, stackLayout.Children.Count);
	}

	[Fact]
	public void SwipeViewCanBeNestedInOtherViews()
	{
		var outerStack = new StackLayout();
		var swipeView = new SwipeView
		{
			Content = new Label { Text = "Inside SwipeView" }
		};

		outerStack.Add(swipeView);

		Assert.Single(outerStack.Children);
		Assert.Equal(swipeView, outerStack.Children[0]);
		Assert.NotNull(swipeView.Content);
	}

	[Fact]
	public void MultipleSwipeViewsCanExist()
	{
		var swipeView1 = new SwipeView { Content = new Label { Text = "First" } };
		var swipeView2 = new SwipeView { Content = new Label { Text = "Second" } };
		var swipeView3 = new SwipeView { Content = new Label { Text = "Third" } };

		var stackLayout = new StackLayout
		{
			swipeView1,
			swipeView2,
			swipeView3
		};

		Assert.Equal(3, stackLayout.Children.Count);
		Assert.NotNull(swipeView1.Content);
		Assert.NotNull(swipeView2.Content);
		Assert.NotNull(swipeView3.Content);
	}
}
