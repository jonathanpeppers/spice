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
		view.VerticalAlign = Align.End;

		Assert.Equal(nameof(view.VerticalAlign), property);
	}

	[Fact]
	public void PropertyChangingWorks()
	{
		string? property = null;
		var view = new View();
		view.PropertyChanging += (sender, e) => property = e.PropertyName;
		view.HorizontalAlign = Align.End;

		Assert.Equal(nameof(view.HorizontalAlign), property);
	}
}
