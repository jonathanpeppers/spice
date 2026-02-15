namespace Spice.Tests;

/// <summary>
/// Tests for the Editor view's default values, property setters, and change notifications.
/// </summary>

public class EditorTests
{
	[Fact]
	public void EditorCanBeCreated()
	{
		var editor = new Editor();
		Assert.NotNull(editor);
	}

	[Fact]
	public void EditorInheritsFromView()
	{
		var editor = new Editor();
		Assert.IsAssignableFrom<View>(editor);
	}

	[Fact]
	public void TextPropertyDefaultsToEmptyString()
	{
		var editor = new Editor();
		Assert.Equal("", editor.Text);
	}

	[Fact]
	public void TextPropertyCanBeSet()
	{
		var editor = new Editor { Text = "Hello World\nMulti-line text" };
		Assert.Equal("Hello World\nMulti-line text", editor.Text);
	}

	[Fact]
	public void TextColorPropertyDefaultsToNull()
	{
		var editor = new Editor();
		Assert.Null(editor.TextColor);
	}

	[Fact]
	public void TextColorPropertyCanBeSet()
	{
		var color = Colors.Green;
		var editor = new Editor { TextColor = color };
		Assert.Equal(color, editor.TextColor);
	}

	[Fact]
	public void PlaceholderPropertyDefaultsToNull()
	{
		var editor = new Editor();
		Assert.Null(editor.Placeholder);
	}

	[Fact]
	public void PlaceholderPropertyCanBeSet()
	{
		var editor = new Editor { Placeholder = "Enter text here..." };
		Assert.Equal("Enter text here...", editor.Placeholder);
	}

	[Fact]
	public void PlaceholderColorPropertyDefaultsToNull()
	{
		var editor = new Editor();
		Assert.Null(editor.PlaceholderColor);
	}

	[Fact]
	public void PlaceholderColorPropertyCanBeSet()
	{
		var color = Colors.Gray;
		var editor = new Editor { PlaceholderColor = color };
		Assert.Equal(color, editor.PlaceholderColor);
	}

	[Fact]
	public void AutoSizePropertyDefaultsToFalse()
	{
		var editor = new Editor();
		Assert.False(editor.AutoSize);
	}

	[Fact]
	public void AutoSizePropertyCanBeSet()
	{
		var editor = new Editor { AutoSize = true };
		Assert.True(editor.AutoSize);
	}

	[Fact]
	public void PropertyChangedFiresOnTextChange()
	{
		var editor = new Editor();
		var propertyChangedFired = false;
		editor.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Editor.Text))
				propertyChangedFired = true;
		};

		editor.Text = "New Text";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnTextColorChange()
	{
		var editor = new Editor();
		var propertyChangedFired = false;
		editor.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Editor.TextColor))
				propertyChangedFired = true;
		};

		editor.TextColor = Colors.Purple;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnPlaceholderChange()
	{
		var editor = new Editor();
		var propertyChangedFired = false;
		editor.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Editor.Placeholder))
				propertyChangedFired = true;
		};

		editor.Placeholder = "New placeholder";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnPlaceholderColorChange()
	{
		var editor = new Editor();
		var propertyChangedFired = false;
		editor.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Editor.PlaceholderColor))
				propertyChangedFired = true;
		};

		editor.PlaceholderColor = Colors.Blue;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnAutoSizeChange()
	{
		var editor = new Editor();
		var propertyChangedFired = false;
		editor.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Editor.AutoSize))
				propertyChangedFired = true;
		};

		editor.AutoSize = true;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void TextCanBeSetMultipleTimes()
	{
		var editor = new Editor { Text = "First line\nSecond line" };
		Assert.Equal("First line\nSecond line", editor.Text);
		
		editor.Text = "Updated text";
		Assert.Equal("Updated text", editor.Text);
		
		editor.Text = "";
		Assert.Equal("", editor.Text);
	}

	[Fact]
	public void MultiplePropertiesCanBeSetTogether()
	{
		var editor = new Editor 
		{ 
			Text = "Test text",
			TextColor = Colors.Red,
			Placeholder = "Enter text",
			PlaceholderColor = Colors.LightGray,
			AutoSize = true
		};
		
		Assert.Equal("Test text", editor.Text);
		Assert.Equal(Colors.Red, editor.TextColor);
		Assert.Equal("Enter text", editor.Placeholder);
		Assert.Equal(Colors.LightGray, editor.PlaceholderColor);
		Assert.True(editor.AutoSize);
	}
}
