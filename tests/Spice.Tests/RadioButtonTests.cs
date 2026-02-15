namespace Spice.Tests;

/// <summary>
/// Tests for RadioButton control
/// </summary>
public class RadioButtonTests
{
	[Fact]
	public void IsCheckedPropertyChanged()
	{
		string? property = null;
		var radioButton = new RadioButton();
		radioButton.PropertyChanged += (sender, e) => property = e.PropertyName;
		radioButton.IsChecked = true;

		Assert.Equal(nameof(radioButton.IsChecked), property);
		Assert.True(radioButton.IsChecked);
	}

	[Fact]
	public void CheckedChangedAssignmentRaisesPropertyChanged()
	{
		string? property = null;
		var radioButton = new RadioButton();
		radioButton.PropertyChanged += (sender, e) => property = e.PropertyName;
		radioButton.CheckedChanged = _ => { };

		Assert.Equal(nameof(radioButton.CheckedChanged), property);
		Assert.NotNull(radioButton.CheckedChanged);
	}

	[Fact]
	public void DefaultIsCheckedIsFalse()
	{
		var radioButton = new RadioButton();
		Assert.False(radioButton.IsChecked);
	}

	[Fact]
	public void IsCheckedCanBeSet()
	{
		var radioButton = new RadioButton { IsChecked = true };
		Assert.True(radioButton.IsChecked);

		radioButton.IsChecked = false;
		Assert.False(radioButton.IsChecked);
	}

	[Fact]
	public void CheckedChangedCanBeSet()
	{
		bool invoked = false;
		var radioButton = new RadioButton { CheckedChanged = _ => invoked = true };
		
		Assert.NotNull(radioButton.CheckedChanged);
		radioButton.CheckedChanged?.Invoke(radioButton);
		Assert.True(invoked);
	}

	[Fact]
	public void GroupNamePropertyChanged()
	{
		string? property = null;
		var radioButton = new RadioButton();
		radioButton.PropertyChanged += (sender, e) => property = e.PropertyName;
		radioButton.GroupName = "TestGroup";

		Assert.Equal(nameof(radioButton.GroupName), property);
		Assert.Equal("TestGroup", radioButton.GroupName);
	}

	[Fact]
	public void GroupNameCanBeSet()
	{
		var radioButton = new RadioButton { GroupName = "Options" };
		Assert.Equal("Options", radioButton.GroupName);

		radioButton.GroupName = "NewOptions";
		Assert.Equal("NewOptions", radioButton.GroupName);
	}

	[Fact]
	public void ContentPropertyChanged()
	{
		string? property = null;
		var radioButton = new RadioButton();
		radioButton.PropertyChanged += (sender, e) => property = e.PropertyName;
		radioButton.Content = "Option 1";

		Assert.Equal(nameof(radioButton.Content), property);
		Assert.Equal("Option 1", radioButton.Content);
	}

	[Fact]
	public void ContentCanBeSet()
	{
		var radioButton = new RadioButton { Content = "Test Option" };
		Assert.Equal("Test Option", radioButton.Content);

		radioButton.Content = "Updated Option";
		Assert.Equal("Updated Option", radioButton.Content);
	}

	[Fact]
	public void GroupExclusivity()
	{
		var radioButton1 = new RadioButton { GroupName = "Group1" };
		var radioButton2 = new RadioButton { GroupName = "Group1" };
		var radioButton3 = new RadioButton { GroupName = "Group1" };

		radioButton1.IsChecked = true;
		Assert.True(radioButton1.IsChecked);
		Assert.False(radioButton2.IsChecked);
		Assert.False(radioButton3.IsChecked);

		radioButton2.IsChecked = true;
		Assert.False(radioButton1.IsChecked);
		Assert.True(radioButton2.IsChecked);
		Assert.False(radioButton3.IsChecked);

		radioButton3.IsChecked = true;
		Assert.False(radioButton1.IsChecked);
		Assert.False(radioButton2.IsChecked);
		Assert.True(radioButton3.IsChecked);
	}

	[Fact]
	public void DifferentGroupsAreIndependent()
	{
		var radioButton1 = new RadioButton { GroupName = "Group1" };
		var radioButton2 = new RadioButton { GroupName = "Group2" };

		radioButton1.IsChecked = true;
		radioButton2.IsChecked = true;

		Assert.True(radioButton1.IsChecked);
		Assert.True(radioButton2.IsChecked);
	}

	[Fact]
	public void NoGroupNameAllowsMultipleChecked()
	{
		var radioButton1 = new RadioButton();
		var radioButton2 = new RadioButton();

		radioButton1.IsChecked = true;
		radioButton2.IsChecked = true;

		Assert.True(radioButton1.IsChecked);
		Assert.True(radioButton2.IsChecked);
	}

	[Fact]
	public void ObjectInitializerWithIsCheckedBeforeGroupName()
	{
		// Bug fix: IsChecked set before GroupName should still enforce group exclusivity
		var radioButton1 = new RadioButton { IsChecked = true, GroupName = "Options" };
		var radioButton2 = new RadioButton { GroupName = "Options" };

		Assert.True(radioButton1.IsChecked);
		Assert.False(radioButton2.IsChecked);

		radioButton2.IsChecked = true;
		Assert.False(radioButton1.IsChecked);
		Assert.True(radioButton2.IsChecked);
	}

	[Fact]
	public void ChangingGroupNameRemovesFromOldGroup()
	{
		var radioButton1 = new RadioButton { GroupName = "GroupA", IsChecked = true };
		var radioButton2 = new RadioButton { GroupName = "GroupA" };

		Assert.True(radioButton1.IsChecked);
		Assert.False(radioButton2.IsChecked);

		// Change radioButton1 to GroupB
		radioButton1.GroupName = "GroupB";

		// Checking radioButton2 in GroupA should not affect radioButton1 anymore
		radioButton2.IsChecked = true;
		Assert.True(radioButton1.IsChecked); // Still checked, now in different group
		Assert.True(radioButton2.IsChecked);
	}

	[Fact]
	public void ChangingGroupNameToSameGroupEnforcesExclusivity()
	{
		var radioButton1 = new RadioButton { GroupName = "GroupA", IsChecked = true };
		var radioButton2 = new RadioButton { GroupName = "GroupB", IsChecked = true };

		Assert.True(radioButton1.IsChecked);
		Assert.True(radioButton2.IsChecked);

		// Move radioButton2 to GroupA - should uncheck radioButton1
		radioButton2.GroupName = "GroupA";

		Assert.False(radioButton1.IsChecked);
		Assert.True(radioButton2.IsChecked);
	}

	[Fact]
	public void DisposeUnsubscribesEvents()
	{
		var radioButton = new RadioButton();
		radioButton.CheckedChanged = _ => { };
		
		// Dispose should not throw
		radioButton.Dispose();
		
		// Setting IsChecked after dispose should still work
		radioButton.IsChecked = true;
		Assert.True(radioButton.IsChecked);
		
		// CheckedChanged action is still set (it's a property), but native event handlers are unsubscribed
		Assert.NotNull(radioButton.CheckedChanged);
	}
}
