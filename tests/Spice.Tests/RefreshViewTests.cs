namespace Spice.Tests;

/// <summary>
/// Tests for RefreshView functionality
/// </summary>
public class RefreshViewTests
{
	[Fact]
	public void CanCreate()
	{
		var refreshView = new RefreshView();
		Assert.NotNull(refreshView);
	}

	[Fact]
	public void ContentIsNullByDefault()
	{
		var refreshView = new RefreshView();
		Assert.Null(refreshView.Content);
	}

	[Fact]
	public void IsRefreshingIsFalseByDefault()
	{
		var refreshView = new RefreshView();
		Assert.False(refreshView.IsRefreshing);
	}

	[Fact]
	public void CommandIsNullByDefault()
	{
		var refreshView = new RefreshView();
		Assert.Null(refreshView.Command);
	}

	[Fact]
	public void RefreshColorIsNullByDefault()
	{
		var refreshView = new RefreshView();
		Assert.Null(refreshView.RefreshColor);
	}

	[Fact]
	public void CanSetContent()
	{
		var refreshView = new RefreshView();
		var scrollView = new ScrollView();
		
		refreshView.Content = scrollView;
		
		Assert.NotNull(refreshView.Content);
		Assert.Equal(scrollView, refreshView.Content);
	}

	[Fact]
	public void CanReplaceContent()
	{
		var refreshView = new RefreshView();
		var firstScroll = new ScrollView();
		var secondScroll = new ScrollView();

		refreshView.Content = firstScroll;
		Assert.Equal(firstScroll, refreshView.Content);

		refreshView.Content = secondScroll;
		Assert.Equal(secondScroll, refreshView.Content);
	}

	[Fact]
	public void CanClearContent()
	{
		var refreshView = new RefreshView();
		var scrollView = new ScrollView();
		
		refreshView.Content = scrollView;
		Assert.NotNull(refreshView.Content);

		refreshView.Content = null;
		Assert.Null(refreshView.Content);
	}

	[Fact]
	public void CanSetIsRefreshing()
	{
		var refreshView = new RefreshView();
		Assert.False(refreshView.IsRefreshing);

		refreshView.IsRefreshing = true;
		Assert.True(refreshView.IsRefreshing);

		refreshView.IsRefreshing = false;
		Assert.False(refreshView.IsRefreshing);
	}

	[Fact]
	public void CanSetCommand()
	{
		var refreshView = new RefreshView();
		var commandExecuted = false;
		Action command = () => commandExecuted = true;

		refreshView.Command = command;
		Assert.NotNull(refreshView.Command);
		Assert.Equal(command, refreshView.Command);

		// Execute the command
		refreshView.Command.Invoke();
		Assert.True(commandExecuted);
	}

	[Fact]
	public void CanSetRefreshColor()
	{
		var refreshView = new RefreshView();
		Assert.Null(refreshView.RefreshColor);

		var blue = new Color(0, 0, 255);
		refreshView.RefreshColor = blue;
		Assert.Equal(blue, refreshView.RefreshColor);

		var red = new Color(255, 0, 0);
		refreshView.RefreshColor = red;
		Assert.Equal(red, refreshView.RefreshColor);

		refreshView.RefreshColor = null;
		Assert.Null(refreshView.RefreshColor);
	}

	[Fact]
	public void PropertyChangedFiresOnContentChange()
	{
		string? property = null;
		var refreshView = new RefreshView();
		refreshView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		var scrollView = new ScrollView();
		refreshView.Content = scrollView;

		Assert.Equal(nameof(refreshView.Content), property);
	}

	[Fact]
	public void PropertyChangedFiresOnIsRefreshingChange()
	{
		string? property = null;
		var refreshView = new RefreshView();
		refreshView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		refreshView.IsRefreshing = true;

		Assert.Equal(nameof(refreshView.IsRefreshing), property);
	}

	[Fact]
	public void PropertyChangedFiresOnCommandChange()
	{
		string? property = null;
		var refreshView = new RefreshView();
		refreshView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		// Set command to test property change notification
		refreshView.Command = () => { };

		Assert.Equal(nameof(refreshView.Command), property);
	}

	[Fact]
	public void PropertyChangedFiresOnRefreshColorChange()
	{
		string? property = null;
		var refreshView = new RefreshView();
		refreshView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		refreshView.RefreshColor = new Color(255, 0, 0);

		Assert.Equal(nameof(refreshView.RefreshColor), property);
	}

	[Fact]
	public void SettingSameContentDoesNotFirePropertyChanged()
	{
		var refreshView = new RefreshView();
		var scrollView = new ScrollView();
		refreshView.Content = scrollView;

		int propertyChangedCount = 0;
		refreshView.PropertyChanged += (sender, e) => propertyChangedCount++;

		refreshView.Content = scrollView;
		Assert.Equal(0, propertyChangedCount);
	}

	[Fact]
	public void CanContainScrollView()
	{
		var refreshView = new RefreshView();
		var scrollView = new ScrollView();
		var label = new Label { Text = "Pull to refresh" };
		scrollView.Add(label);
		
		refreshView.Content = scrollView;
		
		Assert.NotNull(refreshView.Content);
		Assert.Equal(scrollView, refreshView.Content);
		Assert.Single(scrollView.Children);
	}

	[Fact]
	public void CanContainStackLayout()
	{
		var refreshView = new RefreshView();
		var stackLayout = new StackLayout
		{
			new Label { Text = "Item 1" },
			new Label { Text = "Item 2" },
			new Label { Text = "Item 3" }
		};
		
		refreshView.Content = stackLayout;
		
		Assert.NotNull(refreshView.Content);
		Assert.Equal(stackLayout, refreshView.Content);
		Assert.Equal(3, stackLayout.Children.Count);
	}

	[Fact]
	public void CanSetAllPropertiesAtOnce()
	{
		var commandExecuted = false;
		var blue = new Color(0, 0, 255);
		var refreshView = new RefreshView
		{
			Content = new ScrollView(),
			IsRefreshing = true,
			Command = () => commandExecuted = true,
			RefreshColor = blue
		};

		Assert.NotNull(refreshView.Content);
		Assert.True(refreshView.IsRefreshing);
		Assert.NotNull(refreshView.Command);
		Assert.Equal(blue, refreshView.RefreshColor);

		refreshView.Command.Invoke();
		Assert.True(commandExecuted);
	}

	[Fact]
	public void RefreshViewCanBeNestedInOtherViews()
	{
		var outerStack = new StackLayout();
		var refreshView = new RefreshView
		{
			Content = new ScrollView()
		};
		
		outerStack.Add(refreshView);
		
		Assert.Single(outerStack.Children);
		Assert.Equal(refreshView, outerStack.Children[0]);
		Assert.NotNull(refreshView.Content);
	}

	[Fact]
	public void MultipleRefreshViewsCanExist()
	{
		var refreshView1 = new RefreshView { Content = new ScrollView() };
		var refreshView2 = new RefreshView { Content = new ScrollView() };
		var refreshView3 = new RefreshView { Content = new ScrollView() };

		var stackLayout = new StackLayout
		{
			refreshView1,
			refreshView2,
			refreshView3
		};

		Assert.Equal(3, stackLayout.Children.Count);
		Assert.NotNull(refreshView1.Content);
		Assert.NotNull(refreshView2.Content);
		Assert.NotNull(refreshView3.Content);
	}
}
