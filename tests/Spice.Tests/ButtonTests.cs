namespace Spice.Tests;

/// <summary>
/// Contains unit tests verifying the behavior of the Button view.
/// </summary>

public class ButtonTests
{
	[Fact]
	public void ButtonCanBeCreated()
	{
		var button = new Button();
		Assert.NotNull(button);
	}

	[Fact]
	public void ButtonInheritsFromView()
	{
		var button = new Button();
		Assert.IsAssignableFrom<View>(button);
	}

	[Fact]
	public void TextPropertyDefaultsToEmptyString()
	{
		var button = new Button();
		Assert.Equal("", button.Text);
	}

	[Fact]
	public void TextPropertyCanBeSet()
	{
		var button = new Button { Text = "Click Me" };
		Assert.Equal("Click Me", button.Text);
	}

	[Fact]
	public void TextColorPropertyDefaultsToNull()
	{
		var button = new Button();
		Assert.Null(button.TextColor);
	}

	[Fact]
	public void TextColorPropertyCanBeSet()
	{
		var color = Colors.Blue;
		var button = new Button { TextColor = color };
		Assert.Equal(color, button.TextColor);
	}

	[Fact]
	public void ClickedActionDefaultsToNull()
	{
		var button = new Button();
		Assert.Null(button.Clicked);
	}

	[Fact]
	public void ClickedActionCanBeSet()
	{
		var actionCalled = false;
		var button = new Button
		{
			Clicked = _ => actionCalled = true
		};
		
		Assert.NotNull(button.Clicked);
		button.Clicked?.Invoke(button);
		Assert.True(actionCalled);
	}

	[Fact]
	public void PropertyChangedFiresOnTextChange()
	{
		var button = new Button();
		var propertyChangedFired = false;
		button.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Button.Text))
				propertyChangedFired = true;
		};

		button.Text = "New Text";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnTextColorChange()
	{
		var button = new Button();
		var propertyChangedFired = false;
		button.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Button.TextColor))
				propertyChangedFired = true;
		};

		button.TextColor = Colors.Red;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnClickedChange()
	{
		var button = new Button();
		var propertyChangedFired = false;
		button.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Button.Clicked))
				propertyChangedFired = true;
		};

		button.Clicked = _ => { };
		Assert.True(propertyChangedFired);
	}
}
