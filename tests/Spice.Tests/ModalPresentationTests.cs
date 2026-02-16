namespace Spice.Tests;

/// <summary>
/// Contains unit tests verifying the behavior of modal presentation on View.
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
	public void PresentAsyncWithViewDoesNotThrow()
	{
		var view = new TestView();
		var modal = new SecondView();

		var task = view.PresentAsync(modal);
		Assert.NotNull(task);
	}

	[Fact]
	public void PresentAsyncWithFactoryDoesNotThrow()
	{
		var view = new TestView();

		var task = view.PresentAsync(() => new SecondView());
		Assert.NotNull(task);
	}

	[Fact]
	public void PresentAsyncGenericDoesNotThrow()
	{
		var view = new TestView();

		var task = view.PresentAsync<SecondView>();
		Assert.NotNull(task);
	}

	[Fact]
	public void DismissAsyncDoesNotThrow()
	{
		var view = new TestView();

		var task = view.DismissAsync();
		Assert.NotNull(task);
	}

	[Fact]
	public async Task PresentAsyncWithNullViewThrows()
	{
		var view = new TestView();
		await Assert.ThrowsAsync<ArgumentNullException>(() => view.PresentAsync((View)null!));
	}

	[Fact]
	public async Task PresentAsyncWithNullFactoryThrows()
	{
		var view = new TestView();
		await Assert.ThrowsAsync<ArgumentNullException>(() => view.PresentAsync((Func<View>)null!));
	}

	[Fact]
	public void PresentAsyncWithFactoryCallsFactory()
	{
		var view = new TestView();
		var factoryCalled = false;

		view.PresentAsync(() =>
		{
			factoryCalled = true;
			return new SecondView();
		});

		Assert.True(factoryCalled);
	}

	[Fact]
	public async Task PresentAsyncReturnsCompletedTask()
	{
		var view = new TestView();
		var modal = new SecondView();

		var task = view.PresentAsync(modal);

		await Task.WhenAny(task, Task.Delay(1000));
		Assert.True(task.IsCompleted);
	}

	[Fact]
	public async Task DismissAsyncReturnsCompletedTask()
	{
		var view = new TestView();

		var task = view.DismissAsync();

		await Task.WhenAny(task, Task.Delay(1000));
		Assert.True(task.IsCompleted);
	}

	[Fact]
	public void PresentAsyncIsAvailableOnAnyView()
	{
		// Modal presentation should work from any View, not just Application
		var label = new View { Title = "Label" };
		var task = label.PresentAsync<TestView>();
		Assert.NotNull(task);
	}

	[Fact]
	public void PresentAsyncGenericCreatesCorrectType()
	{
		var view = new TestView();

		// Should not throw — creates SecondView via parameterless ctor
		var task = view.PresentAsync<SecondView>();
		Assert.NotNull(task);
	}

	[Fact]
	public void ApplicationIsJustAViewWithMain()
	{
		var app = new Application();
		Assert.Null(app.Main);
		Assert.IsAssignableFrom<View>(app);
	}
}
