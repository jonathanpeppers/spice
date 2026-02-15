namespace Spice.Tests;

/// <summary>
/// Tests for view lifecycle and disposal behavior as per VIEW-LIFECYCLE-SPEC.md.
/// </summary>
public class ViewLifecycleTests
{
	/// <summary>
	/// A test view that implements IDisposable to track disposal
	/// </summary>
	private class DisposableView : View, IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	/// <summary>
	/// A test view that does not implement IDisposable
	/// </summary>
	private class NonDisposableView : View
	{
	}

	[Fact]
	public void NonDisposableViewDoesNotThrowWhenDisposedRecursively()
	{
		var view = new NonDisposableView();
		
		// Should not throw
		View.DisposeRecursive(view);
	}

	[Fact]
	public void DisposableViewIsDisposedWhenCallingDisposeRecursive()
	{
		var view = new DisposableView();
		Assert.False(view.IsDisposed);

		View.DisposeRecursive(view);

		Assert.True(view.IsDisposed);
	}

	[Fact]
	public void ChildViewsAreDisposedRecursively()
	{
		var parent = new DisposableView();
		var child1 = new DisposableView();
		var child2 = new DisposableView();

		parent.Children.Add(child1);
		parent.Children.Add(child2);

		Assert.False(parent.IsDisposed);
		Assert.False(child1.IsDisposed);
		Assert.False(child2.IsDisposed);

		View.DisposeRecursive(parent);

		Assert.True(parent.IsDisposed);
		Assert.True(child1.IsDisposed);
		Assert.True(child2.IsDisposed);
	}

	[Fact]
	public void NestedChildViewsAreDisposedRecursively()
	{
		var root = new DisposableView();
		var child = new DisposableView();
		var grandchild = new DisposableView();

		root.Children.Add(child);
		child.Children.Add(grandchild);

		View.DisposeRecursive(root);

		Assert.True(root.IsDisposed);
		Assert.True(child.IsDisposed);
		Assert.True(grandchild.IsDisposed);
	}

	[Fact]
	public void MixedDisposableAndNonDisposableChildrenAreHandled()
	{
		var parent = new DisposableView();
		var disposableChild = new DisposableView();
		var nonDisposableChild = new NonDisposableView();

		parent.Children.Add(disposableChild);
		parent.Children.Add(nonDisposableChild);

		View.DisposeRecursive(parent);

		Assert.True(parent.IsDisposed);
		Assert.True(disposableChild.IsDisposed);
		// NonDisposableView doesn't throw and continues normally
	}

	[Fact]
	public void ChildrenAreDisposedBeforeParent()
	{
		var disposalOrder = new List<string>();

		var parent = new DisposableViewWithCallback("parent", disposalOrder);
		var child = new DisposableViewWithCallback("child", disposalOrder);

		parent.Children.Add(child);

		View.DisposeRecursive(parent);

		Assert.Equal(2, disposalOrder.Count);
		Assert.Equal("child", disposalOrder[0]);
		Assert.Equal("parent", disposalOrder[1]);
	}

	[Fact]
	public void MultipleChildrenAreDisposedInOrder()
	{
		var disposalOrder = new List<string>();

		var parent = new DisposableViewWithCallback("parent", disposalOrder);
		var child1 = new DisposableViewWithCallback("child1", disposalOrder);
		var child2 = new DisposableViewWithCallback("child2", disposalOrder);
		var child3 = new DisposableViewWithCallback("child3", disposalOrder);

		parent.Children.Add(child1);
		parent.Children.Add(child2);
		parent.Children.Add(child3);

		View.DisposeRecursive(parent);

		Assert.Equal(4, disposalOrder.Count);
		// Children disposed first
		Assert.Equal("child1", disposalOrder[0]);
		Assert.Equal("child2", disposalOrder[1]);
		Assert.Equal("child3", disposalOrder[2]);
		// Parent disposed last
		Assert.Equal("parent", disposalOrder[3]);
	}

	[Fact]
	public void ViewWithNoChildrenIsDisposed()
	{
		var view = new DisposableView();
		
		View.DisposeRecursive(view);

		Assert.True(view.IsDisposed);
	}

	[Fact]
	public void DoubleDisposeDoesNotThrow()
	{
		var view = new DisposableView();

		View.DisposeRecursive(view);
		View.DisposeRecursive(view);

		Assert.True(view.IsDisposed);
	}

	/// <summary>
	/// Helper class that tracks disposal order
	/// </summary>
	private class DisposableViewWithCallback : View, IDisposable
	{
		private readonly string _name;
		private readonly List<string> _disposalOrder;

		public DisposableViewWithCallback(string name, List<string> disposalOrder)
		{
			_name = name;
			_disposalOrder = disposalOrder;
		}

		public void Dispose()
		{
			_disposalOrder.Add(_name);
		}
	}
}
