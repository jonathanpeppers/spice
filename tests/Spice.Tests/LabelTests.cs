namespace Spice.Tests;

public class LabelTests
{
	[Fact]
	public void LabelCanBeCreated()
	{
		var label = new Label();
		Assert.NotNull(label);
	}

	[Fact]
	public void LabelInheritsFromView()
	{
		var label = new Label();
		Assert.IsAssignableFrom<View>(label);
	}

	[Fact]
	public void TextPropertyDefaultsToEmptyString()
	{
		var label = new Label();
		Assert.Equal("", label.Text);
	}

	[Fact]
	public void TextPropertyCanBeSet()
	{
		var label = new Label { Text = "Hello World" };
		Assert.Equal("Hello World", label.Text);
	}

	[Fact]
	public void TextColorPropertyDefaultsToNull()
	{
		var label = new Label();
		Assert.Null(label.TextColor);
	}

	[Fact]
	public void TextColorPropertyCanBeSet()
	{
		var color = Colors.Red;
		var label = new Label { TextColor = color };
		Assert.Equal(color, label.TextColor);
	}

	[Fact]
	public void PropertyChangedFiresOnTextChange()
	{
		var label = new Label();
		var propertyChangedFired = false;
		label.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Label.Text))
				propertyChangedFired = true;
		};

		label.Text = "New Text";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnTextColorChange()
	{
		var label = new Label();
		var propertyChangedFired = false;
		label.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Label.TextColor))
				propertyChangedFired = true;
		};

		label.TextColor = Colors.Blue;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void TextCanBeSetToLongString()
	{
		var longText = new string('x', 1000);
		var label = new Label { Text = longText };
		Assert.Equal(longText, label.Text);
		Assert.Equal(1000, label.Text.Length);
	}

	[Fact]
	public void TextCanBeSetMultipleTimes()
	{
		var label = new Label { Text = "First" };
		Assert.Equal("First", label.Text);
		
		label.Text = "Second";
		Assert.Equal("Second", label.Text);
		
		label.Text = "";
		Assert.Equal("", label.Text);
	}

	[Fact]
	public void MultipleLabelInstancesAreIndependent()
	{
		var label1 = new Label { Text = "Label 1", TextColor = Colors.Red };
		var label2 = new Label { Text = "Label 2", TextColor = Colors.Blue };

		Assert.Equal("Label 1", label1.Text);
		Assert.Equal("Label 2", label2.Text);
		Assert.Equal(Colors.Red, label1.TextColor);
		Assert.Equal(Colors.Blue, label2.TextColor);
	}
}
