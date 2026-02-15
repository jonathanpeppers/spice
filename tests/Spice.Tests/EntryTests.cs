namespace Spice.Tests;

public class EntryTests
{
	[Fact]
	public void EntryCanBeCreated()
	{
		var entry = new Entry();
		Assert.NotNull(entry);
	}

	[Fact]
	public void EntryInheritsFromView()
	{
		var entry = new Entry();
		Assert.IsAssignableFrom<View>(entry);
	}

	[Fact]
	public void TextPropertyDefaultsToEmptyString()
	{
		var entry = new Entry();
		Assert.Equal("", entry.Text);
	}

	[Fact]
	public void TextPropertyCanBeSet()
	{
		var entry = new Entry { Text = "Hello World" };
		Assert.Equal("Hello World", entry.Text);
	}

	[Fact]
	public void TextColorPropertyDefaultsToNull()
	{
		var entry = new Entry();
		Assert.Null(entry.TextColor);
	}

	[Fact]
	public void TextColorPropertyCanBeSet()
	{
		var color = Colors.Green;
		var entry = new Entry { TextColor = color };
		Assert.Equal(color, entry.TextColor);
	}

	[Fact]
	public void IsPasswordPropertyDefaultsToFalse()
	{
		var entry = new Entry();
		Assert.False(entry.IsPassword);
	}

	[Fact]
	public void IsPasswordPropertyCanBeSet()
	{
		var entry = new Entry { IsPassword = true };
		Assert.True(entry.IsPassword);
	}

	[Fact]
	public void PropertyChangedFiresOnTextChange()
	{
		var entry = new Entry();
		var propertyChangedFired = false;
		entry.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Entry.Text))
				propertyChangedFired = true;
		};

		entry.Text = "New Text";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnTextColorChange()
	{
		var entry = new Entry();
		var propertyChangedFired = false;
		entry.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Entry.TextColor))
				propertyChangedFired = true;
		};

		entry.TextColor = Colors.Purple;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnIsPasswordChange()
	{
		var entry = new Entry();
		var propertyChangedFired = false;
		entry.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(Entry.IsPassword))
				propertyChangedFired = true;
		};

		entry.IsPassword = true;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void TextCanBeSetMultipleTimes()
	{
		var entry = new Entry { Text = "First" };
		Assert.Equal("First", entry.Text);
		
		entry.Text = "Second";
		Assert.Equal("Second", entry.Text);
		
		entry.Text = "";
		Assert.Equal("", entry.Text);
	}
}
