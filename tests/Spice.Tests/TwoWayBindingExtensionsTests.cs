namespace Spice.Tests;

/// <summary>
/// Tests for the BindTwoWay() extension method and two-way data-binding functionality.
/// </summary>
public class TwoWayBindingExtensionsTests
{
	[Fact]
	public void BindTwoWay_SyncsInitialValue()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Hello" };
		var entry = new Entry { Text = "Initial" };

		// Act
		viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Assert
		Assert.Equal("Hello", entry.Text);
	}

	[Fact]
	public void BindTwoWay_UpdatesTargetWhenSourceChanges()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Initial" };
		var entry = new Entry();
		viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Act
		viewModel.Title = "From ViewModel";

		// Assert
		Assert.Equal("From ViewModel", entry.Text);
	}

	[Fact]
	public void BindTwoWay_UpdatesSourceWhenTargetChanges()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Initial" };
		var entry = new Entry();
		viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Act
		entry.Text = "From Entry";

		// Assert
		Assert.Equal("From Entry", viewModel.Title);
	}

	[Fact]
	public void BindTwoWay_PreventsInfiniteLoop()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Start" };
		var entry = new Entry();
		int sourceUpdateCount = 0;
		int targetUpdateCount = 0;

		viewModel.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(viewModel.Title))
				sourceUpdateCount++;
		};

		entry.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(entry.Text))
				targetUpdateCount++;
		};

		viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Reset counts after initial sync
		sourceUpdateCount = 0;
		targetUpdateCount = 0;

		// Act
		viewModel.Title = "Changed";

		// Assert - should only trigger one update in each direction, not infinite
		Assert.Equal(1, sourceUpdateCount); // One update on the source
		Assert.Equal(1, targetUpdateCount); // One update on the target
		Assert.Equal("Changed", entry.Text);
	}

	[Fact]
	public void BindTwoWay_DoesNotUpdateForOtherProperties()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Title", Count = 0 };
		var entry = new Entry();
		viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Act
		viewModel.Count = 42; // Change a different property

		// Assert
		Assert.Equal("Title", entry.Text); // Should not have changed
	}

	[Fact]
	public void BindTwoWay_DisposalStopsUpdates()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Initial" };
		var entry = new Entry();
		var binding = viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Act
		binding.Dispose();
		viewModel.Title = "After Dispose VM";
		entry.Text = "After Dispose Entry";

		// Assert
		Assert.Equal("After Dispose VM", viewModel.Title); // VM changed but didn't sync
		Assert.Equal("After Dispose Entry", entry.Text); // Entry changed but didn't sync
	}

	[Fact]
	public void BindTwoWay_MultipleDisposalsSafe()
	{
		// Arrange
		var viewModel = new TestViewModel { Title = "Test" };
		var entry = new Entry();
		var binding = viewModel.BindTwoWay(
			nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
			entry,
			nameof(entry.Text), () => entry.Text, val => entry.Text = val);

		// Act & Assert - should not throw
		binding.Dispose();
		binding.Dispose();
		binding.Dispose();
	}

	[Fact]
	public void BindTwoWay_WorksWithValueTypes()
	{
		// Arrange
		var viewModel = new TestViewModel { Count = 42 };
		var slider = new Slider { Value = 0 };

		// Act
		viewModel.BindTwoWay(
			nameof(viewModel.Count), vm => vm.Count, (vm, val) => vm.Count = val,
			slider,
			nameof(slider.Value), () => (int)slider.Value, val => slider.Value = val);

		// Assert
		Assert.Equal(42, (int)slider.Value);

		// Update from source
		viewModel.Count = 100;
		Assert.Equal(100, (int)slider.Value);

		// Update from target
		slider.Value = 50;
		Assert.Equal(50, viewModel.Count);
	}

	[Fact]
	public void BindTwoWay_WorksWithBooleans()
	{
		// Arrange
		var viewModel = new TestViewModel { IsEnabled = true };
		var checkBox = new CheckBox { IsChecked = false };

		// Act
		viewModel.BindTwoWay(
			nameof(viewModel.IsEnabled), vm => vm.IsEnabled, (vm, val) => vm.IsEnabled = val,
			checkBox,
			nameof(checkBox.IsChecked), () => checkBox.IsChecked, val => checkBox.IsChecked = val);

		// Assert
		Assert.True(checkBox.IsChecked);

		// Update from source
		viewModel.IsEnabled = false;
		Assert.False(checkBox.IsChecked);

		// Update from target
		checkBox.IsChecked = true;
		Assert.True(viewModel.IsEnabled);
	}

	[Fact]
	public void BindTwoWay_ThrowsOnNullSource()
	{
		// Arrange
		TestViewModel? source = null;
		var entry = new Entry();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			source!.BindTwoWay(
				nameof(TestViewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
				entry,
				nameof(entry.Text), () => entry.Text, val => entry.Text = val));
	}

	[Fact]
	public void BindTwoWay_ThrowsOnNullTarget()
	{
		// Arrange
		var viewModel = new TestViewModel();
		Entry? target = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
				target!,
				nameof(Entry.Text), () => string.Empty, val => { }));
	}

	[Fact]
	public void BindTwoWay_ThrowsOnNullPropertyNames()
	{
		// Arrange
		var viewModel = new TestViewModel();
		var entry = new Entry();

		// Act & Assert - null source property
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				null!, vm => vm.Title, (vm, val) => vm.Title = val,
				entry,
				nameof(entry.Text), () => entry.Text, val => entry.Text = val));

		// Act & Assert - null target property
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
				entry,
				null!, () => entry.Text, val => entry.Text = val));
	}

	[Fact]
	public void BindTwoWay_ThrowsOnNullGettersOrSetters()
	{
		// Arrange
		var viewModel = new TestViewModel();
		var entry = new Entry();

		// Act & Assert - null source getter
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				nameof(viewModel.Title), null!, (vm, val) => vm.Title = val,
				entry,
				nameof(entry.Text), () => entry.Text, val => entry.Text = val));

		// Act & Assert - null source setter
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				nameof(viewModel.Title), vm => vm.Title, null!,
				entry,
				nameof(entry.Text), () => entry.Text, val => entry.Text = val));

		// Act & Assert - null target getter
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
				entry,
				nameof(entry.Text), null!, val => entry.Text = val));

		// Act & Assert - null target setter
		Assert.Throws<ArgumentNullException>(() =>
			viewModel.BindTwoWay(
				nameof(viewModel.Title), vm => vm.Title, (vm, val) => vm.Title = val,
				entry,
				nameof(entry.Text), () => entry.Text, null!));
	}

	[Fact]
	public void BindTwoWay_SupportsChainedUpdates()
	{
		// Arrange
		var vm1 = new TestViewModel { Title = "First" };
		var vm2 = new TestViewModel { Title = "Second" };

		// Act - Bind vm1 to vm2
		vm1.BindTwoWay(
			nameof(vm1.Title), v => v.Title, (v, val) => v.Title = val,
			vm2,
			nameof(vm2.Title), () => vm2.Title, val => vm2.Title = val);

		// Assert
		Assert.Equal("First", vm2.Title);

		// Update vm1
		vm1.Title = "Updated from VM1";
		Assert.Equal("Updated from VM1", vm2.Title);

		// Update vm2
		vm2.Title = "Updated from VM2";
		Assert.Equal("Updated from VM2", vm1.Title);
	}
}
