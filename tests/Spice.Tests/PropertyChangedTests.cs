namespace Spice.Tests;

/// <summary>
/// Asserts that various events on base objects work
/// </summary>
public class PropertyChangedTests
{
	[Fact]
	public void PropertyChangedWorks()
	{
		string? property = null;
		var view = new View();
		view.PropertyChanged += (sender, e) => property = e.PropertyName;
		view.VerticalOptions = LayoutOptions.End;

		Assert.Equal(nameof(view.VerticalOptions), property);
	}

	[Fact]
	public void PropertyChangingWorks()
	{
		string? property = null;
		var view = new View();
		view.PropertyChanging += (sender, e) => property = e.PropertyName;
		view.HorizontalOptions = LayoutOptions.End;

		Assert.Equal(nameof(view.HorizontalOptions), property);
	}
}
