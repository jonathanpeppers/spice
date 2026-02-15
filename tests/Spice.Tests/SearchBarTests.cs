namespace Spice.Tests;

public class SearchBarTests
{
	[Fact]
	public void Text_DefaultsToEmpty()
	{
		var searchBar = new SearchBar();
		Assert.Equal("", searchBar.Text);
	}

	[Fact]
	public void Placeholder_DefaultsToNull()
	{
		var searchBar = new SearchBar();
		Assert.Null(searchBar.Placeholder);
	}

	[Fact]
	public void TextColor_DefaultsToNull()
	{
		var searchBar = new SearchBar();
		Assert.Null(searchBar.TextColor);
	}

	[Fact]
	public void SearchButtonPressed_DefaultsToNull()
	{
		var searchBar = new SearchBar();
		Assert.Null(searchBar.SearchButtonPressed);
	}

	[Fact]
	public void PlaceholderColor_DefaultsToNull()
	{
		var searchBar = new SearchBar();
		Assert.Null(searchBar.PlaceholderColor);
	}

	[Fact]
	public void TextChanged_DefaultsToNull()
	{
		var searchBar = new SearchBar();
		Assert.Null(searchBar.TextChanged);
	}

	[Fact]
	public void Text_CanBeSet()
	{
		var searchBar = new SearchBar();
		searchBar.Text = "hello";
		Assert.Equal("hello", searchBar.Text);
	}

	[Fact]
	public void Placeholder_CanBeSet()
	{
		var searchBar = new SearchBar();
		searchBar.Placeholder = "Search...";
		Assert.Equal("Search...", searchBar.Placeholder);
	}

	[Fact]
	public void TextColor_CanBeSet()
	{
		var searchBar = new SearchBar();
		searchBar.TextColor = Colors.Red;
		Assert.Equal(Colors.Red, searchBar.TextColor);
	}

	[Fact]
	public void SearchButtonPressed_CanBeSet()
	{
		var searchBar = new SearchBar();
		bool pressed = false;
		searchBar.SearchButtonPressed = _ => pressed = true;
		searchBar.SearchButtonPressed.Invoke(searchBar);
		Assert.True(pressed);
	}

	[Fact]
	public void PlaceholderColor_CanBeSet()
	{
		var searchBar = new SearchBar();
		searchBar.PlaceholderColor = Colors.Gray;
		Assert.Equal(Colors.Gray, searchBar.PlaceholderColor);
	}

	[Fact]
	public void TextChanged_CanBeSet()
	{
		var searchBar = new SearchBar();
		bool changed = false;
		searchBar.TextChanged = _ => changed = true;
		searchBar.TextChanged.Invoke(searchBar);
		Assert.True(changed);
	}

	[Fact]
	public void PropertyChanged_Text()
	{
		string? property = null;
		var searchBar = new SearchBar();
		searchBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		searchBar.Text = "test";
		Assert.Equal(nameof(SearchBar.Text), property);
	}

	[Fact]
	public void PropertyChanged_Placeholder()
	{
		string? property = null;
		var searchBar = new SearchBar();
		searchBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		searchBar.Placeholder = "Search...";
		Assert.Equal(nameof(SearchBar.Placeholder), property);
	}

	[Fact]
	public void PropertyChanged_TextColor()
	{
		string? property = null;
		var searchBar = new SearchBar();
		searchBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		searchBar.TextColor = Colors.Blue;
		Assert.Equal(nameof(SearchBar.TextColor), property);
	}

	[Fact]
	public void PropertyChanged_SearchButtonPressed()
	{
		string? property = null;
		var searchBar = new SearchBar();
		searchBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		searchBar.SearchButtonPressed = _ => { };
		Assert.Equal(nameof(SearchBar.SearchButtonPressed), property);
	}

	[Fact]
	public void PropertyChanged_PlaceholderColor()
	{
		string? property = null;
		var searchBar = new SearchBar();
		searchBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		searchBar.PlaceholderColor = Colors.Gray;
		Assert.Equal(nameof(SearchBar.PlaceholderColor), property);
	}

	[Fact]
	public void PropertyChanged_TextChanged()
	{
		string? property = null;
		var searchBar = new SearchBar();
		searchBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		searchBar.TextChanged = _ => { };
		Assert.Equal(nameof(SearchBar.TextChanged), property);
	}
}
