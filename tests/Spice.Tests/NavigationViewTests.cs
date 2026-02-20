namespace Spice.Tests;

/// <summary>
/// Contains unit tests verifying the behavior of the NavigationView.
/// </summary>
public class NavigationViewTests
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
	public void NavigationViewCanBeCreated()
	{
		var rootView = new TestView();
		var nav = new NavigationView(rootView);
		Assert.NotNull(nav);
	}

	[Fact]
	public void NavigationViewInheritsFromView()
	{
		var rootView = new TestView();
		var nav = new NavigationView(rootView);
		Assert.IsAssignableFrom<View>(nav);
	}

	[Fact]
	public void NavigationViewCanBeCreatedWithFactory()
	{
		var nav = new NavigationView(() => new TestView());
		Assert.NotNull(nav);
	}

	[Fact]
	public void NavigationViewGenericCanBeCreated()
	{
		var nav = new NavigationView<TestView>();
		Assert.NotNull(nav);
	}

	[Fact]
	public void NavigationViewGenericInheritsFromNavigationView()
	{
		var nav = new NavigationView<TestView>();
		Assert.IsAssignableFrom<NavigationView>(nav);
	}

	[Fact]
	public void PushSetsNavigationProperty()
	{
		var rootView = new TestView();
		var nav = new NavigationView(rootView);
		
		Assert.Equal(nav, rootView.Navigation);
	}

	[Fact]
	public void PushWithViewSetsNavigation()
	{
		var rootView = new TestView();
		var nav = new NavigationView(rootView);
		
		var secondView = new SecondView();
		nav.Push(secondView);
		
		Assert.Equal(nav, secondView.Navigation);
	}

	[Fact]
	public void PushWithFactorySetsNavigation()
	{
		var nav = new NavigationView<TestView>();
		
		SecondView? capturedView = null;
		nav.Push(() =>
		{
			var view = new SecondView();
			capturedView = view;
			return view;
		});
		
		Assert.NotNull(capturedView);
		Assert.Equal(nav, capturedView.Navigation);
	}

	[Fact]
	public void PushGenericSetsNavigation()
	{
		var rootView = new TestView();
		var nav = new NavigationView(rootView);
		
		// We can't directly check the pushed view without platform code,
		// but we can verify the method doesn't throw
		nav.Push<SecondView>();
	}

	[Fact]
	public void PopDoesNotThrow()
	{
		var nav = new NavigationView<TestView>();
		nav.Pop(); // Should not throw even if empty
	}

	[Fact]
	public void PopToRootDoesNotThrow()
	{
		var nav = new NavigationView<TestView>();
		nav.PopToRoot(); // Should not throw even if only root
	}

	[Fact]
	public void PushWithNullViewThrows()
	{
		var nav = new NavigationView<TestView>();
		Assert.Throws<ArgumentNullException>(() => nav.Push((View)null!));
	}

	[Fact]
	public void PushWithNullFactoryThrows()
	{
		var nav = new NavigationView<TestView>();
		Assert.Throws<ArgumentNullException>(() => nav.Push((Func<View>)null!));
	}

	[Fact]
	public void NavigationViewConstructorWithNullRootThrows()
	{
		Assert.Throws<ArgumentNullException>(() => new NavigationView((View)null!));
	}

	[Fact]
	public void NavigationViewConstructorWithNullFactoryThrows()
	{
		Assert.Throws<ArgumentNullException>(() => new NavigationView((Func<View>)null!));
	}

	[Fact]
	public void TitlePropertyDefaultsToEmptyString()
	{
		var view = new View();
		Assert.Equal("", view.Title);
	}

	[Fact]
	public void TitlePropertyCanBeSet()
	{
		var view = new View { Title = "My Title" };
		Assert.Equal("My Title", view.Title);
	}

	[Fact]
	public void NavigationPropertyDefaultsToNull()
	{
		var view = new View();
		Assert.Null(view.Navigation);
	}

	[Fact]
	public void PropertyChangedFiresOnTitleChange()
	{
		var view = new View();
		var propertyChangedFired = false;
		view.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(View.Title))
				propertyChangedFired = true;
		};

		view.Title = "New Title";
		Assert.True(propertyChangedFired);
	}
}
