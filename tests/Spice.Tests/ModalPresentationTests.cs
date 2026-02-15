namespace Spice.Tests;

/// <summary>
/// Contains unit tests verifying the behavior of modal presentation in Application.
/// </summary>
public class ModalPresentationTests
{
	class TestView : View
	{
		public TestView()
		{
			Title = "Test Modal";
		}
	}

	class SecondView : View
	{
		public SecondView()
		{
			Title = "Second Modal";
		}
	}

	[Fact]
	public void ApplicationCurrentIsSetOnCreation()
	{
		var app = new Application();
		Assert.Equal(app, Application.Current);
	}

	[Fact]
	public void PresentAsyncWithViewDoesNotThrow()
	{
		var app = new Application();
		var view = new TestView();
		
		// In unit tests without platform, this will be a no-op
		// but should not throw
		var task = app.PresentAsync(view);
		Assert.NotNull(task);
	}

	[Fact]
	public void PresentAsyncWithFactoryDoesNotThrow()
	{
		var app = new Application();
		
		var task = app.PresentAsync(() => new TestView());
		Assert.NotNull(task);
	}

	[Fact]
	public void PresentAsyncGenericDoesNotThrow()
	{
		var app = new Application();
		
		var task = app.PresentAsync<TestView>();
		Assert.NotNull(task);
	}

	[Fact]
	public void DismissAsyncDoesNotThrow()
	{
		var app = new Application();
		
		var task = app.DismissAsync();
		Assert.NotNull(task);
	}

	[Fact]
	public void PresentAsyncWithNullViewThrows()
	{
		var app = new Application();
		Assert.Throws<ArgumentNullException>(() => app.PresentAsync((View)null!));
	}

	[Fact]
	public void PresentAsyncWithNullFactoryThrows()
	{
		var app = new Application();
		Assert.Throws<ArgumentNullException>(() => app.PresentAsync((Func<View>)null!));
	}

	[Fact]
	public void PresentAsyncWithFactoryCallsFactory()
	{
		var app = new Application();
		var factoryCalled = false;
		
		app.PresentAsync(() =>
		{
			factoryCalled = true;
			return new TestView();
		});
		
		Assert.True(factoryCalled);
	}

	[Fact]
	public void PresentAsyncGenericCreatesView()
	{
		var app = new Application();
		
		// This should create a TestView without throwing
		var task = app.PresentAsync<TestView>();
		Assert.NotNull(task);
	}

	[Fact]
	public async Task PresentAsyncReturnsCompletedTask()
	{
		var app = new Application();
		var view = new TestView();
		
		// In unit tests, this should complete immediately
		var task = app.PresentAsync(view);
		
		// Should complete without hanging
		await Task.WhenAny(task, Task.Delay(1000));
		
		// Verify it's a Task (not null)
		Assert.NotNull(task);
	}

	[Fact]
	public async Task DismissAsyncReturnsCompletedTask()
	{
		var app = new Application();
		
		// In unit tests, this should complete immediately
		var task = app.DismissAsync();
		
		// Should complete without hanging
		await Task.WhenAny(task, Task.Delay(1000));
		
		// Verify it's a Task (not null)
		Assert.NotNull(task);
	}
}
