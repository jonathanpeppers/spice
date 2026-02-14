using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace Spice.Tests;

/// <summary>
/// Tests for the Picker control
/// </summary>
public class PickerTests
{
	[Fact]
	public void PickerPropertyChangedWorks()
	{
		string? property = null;
		var picker = new Picker();
		picker.PropertyChanged += (sender, e) => property = e.PropertyName;
		picker.Title = "Select an option";

		Assert.Equal(nameof(picker.Title), property);
	}

	[Fact]
	public void PickerItemsCanBeAdded()
	{
		var picker = new Picker();
		picker.Items.Add("Item 1");
		picker.Items.Add("Item 2");

		Assert.Equal(2, picker.Items.Count);
		Assert.Equal("Item 1", picker.Items[0]);
		Assert.Equal("Item 2", picker.Items[1]);
	}

	[Fact]
	public void PickerSelectedIndexDefault()
	{
		var picker = new Picker();
		
		Assert.Equal(-1, picker.SelectedIndex);
		Assert.Null(picker.SelectedItem);
	}

	[Fact]
	public void PickerSelectedIndexUpdates()
	{
		var picker = new Picker();
		picker.Items.Add("Option 1");
		picker.Items.Add("Option 2");
		picker.Items.Add("Option 3");

		picker.SelectedIndex = 1;

		Assert.Equal(1, picker.SelectedIndex);
		Assert.Equal("Option 2", picker.SelectedItem);
	}

	[Fact]
	public void PickerSelectedItemReturnsNull()
	{
		var picker = new Picker();
		picker.Items.Add("Option 1");

		picker.SelectedIndex = -1;

		Assert.Null(picker.SelectedItem);
	}

	[Fact]
	public void PickerSelectedItemReturnsNullForOutOfRange()
	{
		var picker = new Picker();
		picker.Items.Add("Option 1");

		picker.SelectedIndex = 5; // Out of range

		Assert.Null(picker.SelectedItem);
	}

	[Fact]
	public void PickerTextColorCanBeSet()
	{
		var picker = new Picker();
		picker.TextColor = Colors.Red;

		Assert.Equal(Colors.Red, picker.TextColor);
	}

	[Fact]
	public void PickerItemsCollectionNotifiesChange()
	{
		string? property = null;
		var picker = new Picker();
		picker.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		picker.Items = new ObservableCollection<string> { "New Item" };

		Assert.Equal(nameof(picker.Items), property);
	}

	[Fact]
	public void PickerTitleCanBeSet()
	{
		var picker = new Picker { Title = "Choose one" };

		Assert.Equal("Choose one", picker.Title);
	}

	[Fact]
	public void PickerSelectedItemNotifiesPropertyChanged()
	{
		var picker = new Picker();
		picker.Items.Add("Option 1");
		picker.Items.Add("Option 2");

		string? property = null;
		picker.PropertyChanged += (sender, e) => property = e.PropertyName;

		picker.SelectedIndex = 1;

		Assert.Equal(nameof(picker.SelectedItem), property);
		Assert.Equal("Option 2", picker.SelectedItem);
	}
}
