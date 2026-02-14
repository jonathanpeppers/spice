namespace Spice.Tests;

/// <summary>
/// Tests for ContentView functionality
/// </summary>
public class ContentViewTests
{
	[Fact]
	public void CanCreate()
	{
		var contentView = new ContentView();
		Assert.NotNull(contentView);
	}

	[Fact]
	public void ContentIsNullByDefault()
	{
		var contentView = new ContentView();
		Assert.Null(contentView.Content);
	}

	[Fact]
	public void CanSetContent()
	{
		var contentView = new ContentView();
		var label = new Label { Text = "Test" };
		
		contentView.Content = label;
		
		Assert.NotNull(contentView.Content);
		Assert.Equal(label, contentView.Content);
	}

	[Fact]
	public void CanReplaceContent()
	{
		var contentView = new ContentView();
		var firstLabel = new Label { Text = "First" };
		var secondLabel = new Label { Text = "Second" };

		contentView.Content = firstLabel;
		Assert.Equal(firstLabel, contentView.Content);

		contentView.Content = secondLabel;
		Assert.Equal(secondLabel, contentView.Content);
	}

	[Fact]
	public void CanClearContent()
	{
		var contentView = new ContentView();
		var label = new Label { Text = "Test" };
		
		contentView.Content = label;
		Assert.NotNull(contentView.Content);

		contentView.Content = null;
		Assert.Null(contentView.Content);
	}

	[Fact]
	public void PropertyChangedFiresOnContentChange()
	{
		string? property = null;
		var contentView = new ContentView();
		contentView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		var label = new Label { Text = "Test" };
		contentView.Content = label;

		Assert.Equal(nameof(contentView.Content), property);
	}

	[Fact]
	public void SettingSameContentDoesNotFirePropertyChanged()
	{
		var contentView = new ContentView();
		var label = new Label { Text = "Test" };
		contentView.Content = label;

		int propertyChangedCount = 0;
		contentView.PropertyChanged += (sender, e) => propertyChangedCount++;

		contentView.Content = label;
		Assert.Equal(0, propertyChangedCount);
	}

	[Fact]
	public void CanContainStackView()
	{
		var contentView = new ContentView();
		var stackView = new StackView
		{
			new Label { Text = "Item 1" },
			new Label { Text = "Item 2" },
			new Label { Text = "Item 3" }
		};
		
		contentView.Content = stackView;
		
		Assert.NotNull(contentView.Content);
		Assert.Equal(stackView, contentView.Content);
		Assert.Equal(3, stackView.Children.Count);
	}

	[Fact]
	public void CanContainComplexLayout()
	{
		var contentView = new ContentView();
		var scrollView = new ScrollView();
		var innerStack = new StackView
		{
			new Label { Text = "Header" },
			new Button { Text = "Click Me" },
			new Entry { Text = "Enter text" }
		};
		scrollView.Add(innerStack);
		
		contentView.Content = scrollView;
		
		Assert.Equal(scrollView, contentView.Content);
		Assert.Single(scrollView.Children);
		Assert.Equal(innerStack, scrollView.Children[0]);
	}

	[Fact]
	public void ContentViewCanBeNestedInOtherViews()
	{
		var outerStack = new StackView();
		var contentView = new ContentView
		{
			Content = new Label { Text = "Inside ContentView" }
		};
		
		outerStack.Add(contentView);
		
		Assert.Single(outerStack.Children);
		Assert.Equal(contentView, outerStack.Children[0]);
		Assert.NotNull(contentView.Content);
	}

	[Fact]
	public void MultipleContentViewsCanExist()
	{
		var contentView1 = new ContentView { Content = new Label { Text = "First" } };
		var contentView2 = new ContentView { Content = new Label { Text = "Second" } };
		var contentView3 = new ContentView { Content = new Label { Text = "Third" } };

		var stackView = new StackView
		{
			contentView1,
			contentView2,
			contentView3
		};

		Assert.Equal(3, stackView.Children.Count);
		Assert.NotNull(contentView1.Content);
		Assert.NotNull(contentView2.Content);
		Assert.NotNull(contentView3.Content);
	}
}
