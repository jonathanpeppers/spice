namespace Spice.Tests;

/// <summary>
/// Tests for the Bind() extension method and one-way data-binding functionality.
/// </summary>
public class BindingExtensionsTests
{
	[Fact]
	public void Bind_SyncsInitialValue()
	{
		// Arrange
		var label = new Label { Text = "Initial" };
		var viewModel = new TestViewModel { Title = "Hello" };

		// Act
		viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, text => label.Text = text);

		// Assert
		Assert.Equal("Hello", label.Text);
	}

	[Fact]
	public void Bind_UpdatesWhenPropertyChanges()
	{
		// Arrange
		var label = new Label();
		var viewModel = new TestViewModel { Title = "Initial" };
		viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, text => label.Text = text);

		// Act
		viewModel.Title = "Updated";

		// Assert
		Assert.Equal("Updated", label.Text);
	}

	[Fact]
	public void Bind_DoesNotUpdateForOtherProperties()
	{
		// Arrange
		var label = new Label();
		var viewModel = new TestViewModel { Title = "Initial", Count = 0 };
		viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, text => label.Text = text);

		// Act
		viewModel.Count = 42; // Change a different property

		// Assert
		Assert.Equal("Initial", label.Text); // Should not have changed
	}

	[Fact]
	public void Bind_SupportsMultipleBindings()
	{
		// Arrange
		var titleLabel = new Label();
		var countLabel = new Label();
		var viewModel = new TestViewModel { Title = "Test", Count = 5 };

		// Act
		viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, text => titleLabel.Text = text);
		viewModel.Bind(nameof(viewModel.Count), vm => vm.Count, n => countLabel.Text = $"Count: {n}");

		// Assert
		Assert.Equal("Test", titleLabel.Text);
		Assert.Equal("Count: 5", countLabel.Text);

		// Update both
		viewModel.Title = "Updated";
		viewModel.Count = 10;

		Assert.Equal("Updated", titleLabel.Text);
		Assert.Equal("Count: 10", countLabel.Text);
	}

	[Fact]
	public void Bind_DisposalStopsUpdates()
	{
		// Arrange
		var label = new Label();
		var viewModel = new TestViewModel { Title = "Initial" };
		var binding = viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, text => label.Text = text);

		// Act
		binding.Dispose();
		viewModel.Title = "After Dispose";

		// Assert
		Assert.Equal("Initial", label.Text); // Should not update after disposal
	}

	[Fact]
	public void Bind_MultipleDisposalsSafe()
	{
		// Arrange
		var label = new Label();
		var viewModel = new TestViewModel { Title = "Test" };
		var binding = viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, text => label.Text = text);

		// Act & Assert - should not throw
		binding.Dispose();
		binding.Dispose();
		binding.Dispose();
	}

	[Fact]
	public void Bind_ThrowsOnNullSource()
	{
		// Arrange
		TestViewModel? source = null;
		var label = new Label();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			source!.Bind(nameof(TestViewModel.Title), vm => vm.Title, text => label.Text = text));
	}

	[Fact]
	public void Bind_ThrowsOnNullPropertyName()
	{
		// Arrange
		var viewModel = new TestViewModel();
		var label = new Label();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.Bind(null!, vm => vm.Title, text => label.Text = text));
	}

	[Fact]
	public void Bind_ThrowsOnNullGetter()
	{
		// Arrange
		var viewModel = new TestViewModel();
		var label = new Label();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.Bind(nameof(viewModel.Title), (Func<TestViewModel, string>)null!, text => label.Text = text));
	}

	[Fact]
	public void Bind_ThrowsOnNullApply()
	{
		// Arrange
		var viewModel = new TestViewModel();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.Bind(nameof(viewModel.Title), vm => vm.Title, null!));
	}

	[Fact]
	public void Bind_WorksWithViewToView()
	{
		// Arrange
		var sourceLabel = new Label { Text = "Source" };
		var targetLabel = new Label();

		// Act
		sourceLabel.Bind(nameof(sourceLabel.Text), l => l.Text, text => targetLabel.Text = text);

		// Assert
		Assert.Equal("Source", targetLabel.Text);

		// Update
		sourceLabel.Text = "Changed";
		Assert.Equal("Changed", targetLabel.Text);
	}

	[Fact]
	public void Bind_WorksWithValueTypes()
	{
		// Arrange
		var viewModel = new TestViewModel { Count = 42 };
		int capturedValue = 0;

		// Act
		viewModel.Bind(nameof(viewModel.Count), vm => vm.Count, n => capturedValue = n);

		// Assert
		Assert.Equal(42, capturedValue);

		// Update
		viewModel.Count = 100;
		Assert.Equal(100, capturedValue);
	}

	[Fact]
	public void Bind_WorksWithBooleans()
	{
		// Arrange
		var viewModel = new TestViewModel { IsEnabled = true };
		var view = new View();

		// Act
		viewModel.Bind(nameof(viewModel.IsEnabled), vm => vm.IsEnabled, enabled => view.IsEnabled = enabled);

		// Assert
		Assert.True(view.IsEnabled);

		// Update
		viewModel.IsEnabled = false;
		Assert.False(view.IsEnabled);
	}
}
