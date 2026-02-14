namespace Spice.Tests;

/// <summary>
/// Tests for the Switch control
/// </summary>
public class SwitchTests
{
	[Fact]
	public void SwitchCreation()
	{
		var switchControl = new Switch();
		Assert.NotNull(switchControl);
		Assert.False(switchControl.IsOn);
	}

	[Fact]
	public void IsOnPropertyWorks()
	{
		var switchControl = new Switch { IsOn = true };
		Assert.True(switchControl.IsOn);
	}

	[Fact]
	public void IsOnPropertyChangedEvent()
	{
		string? property = null;
		var switchControl = new Switch();
		switchControl.PropertyChanged += (sender, e) => property = e.PropertyName;
		switchControl.IsOn = true;

		Assert.Equal(nameof(switchControl.IsOn), property);
		Assert.True(switchControl.IsOn);
	}

	[Fact]
	public void ToggledActionWorks()
	{
		var switchControl = new Switch();
		Assert.Null(switchControl.Toggled);

		switchControl.Toggled = _ => { };
		Assert.NotNull(switchControl.Toggled);
	}

	[Fact]
	public void ToggledActionCanBeCleared()
	{
		var switchControl = new Switch
		{
			Toggled = _ => { }
		};

		switchControl.Toggled = null;
		Assert.Null(switchControl.Toggled);
	}

	[Fact]
	public void SwitchInheritsFromView()
	{
		var switchControl = new Switch();
		Assert.IsAssignableFrom<View>(switchControl);
	}

}
