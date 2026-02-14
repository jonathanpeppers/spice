namespace Spice.Tests;

/// <summary>
/// Tests for CheckBox control
/// </summary>
public class CheckBoxTests
{
	[Fact]
	public void IsCheckedPropertyChanged()
	{
		string? property = null;
		var checkBox = new CheckBox();
		checkBox.PropertyChanged += (sender, e) => property = e.PropertyName;
		checkBox.IsChecked = true;

		Assert.Equal(nameof(checkBox.IsChecked), property);
		Assert.True(checkBox.IsChecked);
	}

	[Fact]
	public void CheckedChangedPropertyChanged()
	{
		string? property = null;
		var checkBox = new CheckBox();
		checkBox.PropertyChanged += (sender, e) => property = e.PropertyName;
		checkBox.CheckedChanged = _ => { };

		Assert.Equal(nameof(checkBox.CheckedChanged), property);
		Assert.NotNull(checkBox.CheckedChanged);
	}

	[Fact]
	public void DefaultIsCheckedIsFalse()
	{
		var checkBox = new CheckBox();
		Assert.False(checkBox.IsChecked);
	}

	[Fact]
	public void IsCheckedCanBeSet()
	{
		var checkBox = new CheckBox { IsChecked = true };
		Assert.True(checkBox.IsChecked);

		checkBox.IsChecked = false;
		Assert.False(checkBox.IsChecked);
	}

	[Fact]
	public void CheckedChangedCanBeSet()
	{
		bool invoked = false;
		var checkBox = new CheckBox { CheckedChanged = _ => invoked = true };
		
		Assert.NotNull(checkBox.CheckedChanged);
		checkBox.CheckedChanged?.Invoke(checkBox);
		Assert.True(invoked);
	}
}
