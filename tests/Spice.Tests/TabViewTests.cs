namespace Spice.Tests;

/// <summary>
/// Contains unit tests verifying the behavior of TabView and Tab.
/// </summary>
public class TabViewTests
{
	class TestView : View
	{
		public TestView()
		{
			Title = "Test View";
		}
	}

	class SecondView : View
	{
		public SecondView()
		{
			Title = "Second View";
		}
	}

	[Fact]
	public void TabViewCanBeCreated()
	{
		var tabView = new TabView();
		Assert.NotNull(tabView);
	}

	[Fact]
	public void TabViewInheritsFromView()
	{
		var tabView = new TabView();
		Assert.IsAssignableFrom<View>(tabView);
	}

	[Fact]
	public void TabCanBeCreated()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		Assert.NotNull(tab);
	}

	[Fact]
	public void TabInheritsFromView()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		Assert.IsAssignableFrom<View>(tab);
	}

	[Fact]
	public void TabGenericCanBeCreated()
	{
		var tab = new Tab<TestView>("Home", "home.png");
		Assert.NotNull(tab);
	}

	[Fact]
	public void TabGenericInheritsFromTab()
	{
		var tab = new Tab<TestView>("Home", "home.png");
		Assert.IsAssignableFrom<Tab>(tab);
	}

	[Fact]
	public void TabTitleIsSet()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		Assert.Equal("Home", tab.Title);
	}

	[Fact]
	public void TabIconIsSet()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		Assert.Equal("home.png", tab.Icon);
	}

	[Fact]
	public void TabContentIsSet()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		Assert.Equal(content, tab.Content);
	}

	[Fact]
	public void TabWithFactoryCreatesContentLazily()
	{
		var factoryCalled = false;
		var tab = new Tab("Home", "home.png", () =>
		{
			factoryCalled = true;
			return new TestView();
		});
		
		// Factory should not be called yet
		Assert.False(factoryCalled);
		Assert.Null(tab.Content);
		
		// Ensure content creates it
		tab.EnsureContent();
		Assert.True(factoryCalled);
		Assert.NotNull(tab.Content);
	}

	[Fact]
	public void TabGenericCreatesContentLazily()
	{
		var tab = new Tab<TestView>("Home", "home.png");
		
		// Content should not be created yet
		Assert.Null(tab.Content);
		
		// Ensure content creates it
		tab.EnsureContent();
		Assert.NotNull(tab.Content);
		Assert.IsType<TestView>(tab.Content);
	}

	[Fact]
	public void TabViewAddMethodWorks()
	{
		var tabView = new TabView();
		var tab = new Tab<TestView>("Home", "home.png");
		
		tabView.Add(tab);
		
		Assert.Single(tabView.Children);
		Assert.Equal(tab, tabView.Children[0]);
	}

	[Fact]
	public void TabViewCollectionInitializerWorks()
	{
		var tabView = new TabView
		{
			new Tab<TestView>("Home", "home.png"),
			new Tab<SecondView>("Profile", "profile.png")
		};
		
		Assert.Equal(2, tabView.Children.Count);
	}

	[Fact]
	public void TabConstructorWithNullTitleThrows()
	{
		var content = new TestView();
		Assert.Throws<ArgumentNullException>(() => new Tab(null!, "icon.png", content));
	}

	[Fact]
	public void TabConstructorWithNullIconThrows()
	{
		var content = new TestView();
		Assert.Throws<ArgumentNullException>(() => new Tab("Title", null!, content));
	}

	[Fact]
	public void TabConstructorWithNullContentThrows()
	{
		Assert.Throws<ArgumentNullException>(() => new Tab("Title", "icon.png", (View)null!));
	}

	[Fact]
	public void TabConstructorWithNullFactoryThrows()
	{
		Assert.Throws<ArgumentNullException>(() => new Tab("Title", "icon.png", (Func<View>)null!));
	}

	[Fact]
	public void TabViewAddWithNullThrows()
	{
		var tabView = new TabView();
		Assert.Throws<ArgumentNullException>(() => tabView.Add(null!));
	}

	[Fact]
	public void PropertyChangedFiresOnIconChange()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		var propertyChangedFired = false;
		tab.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Tab.Icon))
				propertyChangedFired = true;
		};

		tab.Icon = "new_icon.png";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void EnsureContentOnlyCreatesContentOnce()
	{
		var callCount = 0;
		var tab = new Tab("Home", "home.png", () =>
		{
			callCount++;
			return new TestView();
		});
		
		tab.EnsureContent();
		tab.EnsureContent();
		tab.EnsureContent();
		
		Assert.Equal(1, callCount);
	}

	[Fact]
	public void TabContentAddsToChildren()
	{
		var content = new TestView();
		var tab = new Tab("Home", "home.png", content);
		
		Assert.Single(tab.Children);
		Assert.Equal(content, tab.Children[0]);
	}
}
