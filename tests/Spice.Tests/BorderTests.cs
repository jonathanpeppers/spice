namespace Spice.Tests;

/// <summary>
/// Tests for Border control
/// </summary>
public class BorderTests
{
	[Fact]
	public void Constructor()
	{
		var border = new Border();
		Assert.NotNull(border);
	}

	[Fact]
	public void DefaultProperties()
	{
		var border = new Border();
		Assert.Null(border.Content);
		Assert.Null(border.Stroke);
		Assert.Equal(1.0, border.StrokeThickness);
		Assert.Equal(0.0, border.CornerRadius);
		Assert.Equal(0.0, border.Padding);
	}

	[Fact]
	public void SetContent()
	{
		var border = new Border();
		var label = new Label { Text = "Test" };
		border.Content = label;
		Assert.Equal(label, border.Content);
	}

	[Fact]
	public void SetStroke()
	{
		var border = new Border
		{
			Stroke = Colors.Red
		};
		Assert.Equal(Colors.Red, border.Stroke);
	}

	[Fact]
	public void SetStrokeThickness()
	{
		var border = new Border
		{
			StrokeThickness = 2.5
		};
		Assert.Equal(2.5, border.StrokeThickness);
	}

	[Fact]
	public void SetCornerRadius()
	{
		var border = new Border
		{
			CornerRadius = 10.0
		};
		Assert.Equal(10.0, border.CornerRadius);
	}

	[Fact]
	public void SetPadding()
	{
		var border = new Border
		{
			Padding = 8.0
		};
		Assert.Equal(8.0, border.Padding);
	}

	[Fact]
	public void PropertyChangedFires()
	{
		string? property = null;
		var border = new Border();
		border.PropertyChanged += (sender, e) => property = e.PropertyName;
		border.StrokeThickness = 3.0;

		Assert.Equal(nameof(border.StrokeThickness), property);
	}

	[Fact]
	public void CanSetAllProperties()
	{
		var label = new Label { Text = "Content" };
		var border = new Border
		{
			Content = label,
			Stroke = Colors.Blue,
			StrokeThickness = 2.0,
			CornerRadius = 15.0,
			Padding = 10.0,
			BackgroundColor = Colors.LightGray
		};

		Assert.Equal(label, border.Content);
		Assert.Equal(Colors.Blue, border.Stroke);
		Assert.Equal(2.0, border.StrokeThickness);
		Assert.Equal(15.0, border.CornerRadius);
		Assert.Equal(10.0, border.Padding);
		Assert.Equal(Colors.LightGray, border.BackgroundColor);
	}

	[Fact]
	public void ContentCanBeChanged()
	{
		var border = new Border();
		var label1 = new Label { Text = "First" };
		var label2 = new Label { Text = "Second" };

		border.Content = label1;
		Assert.Equal(label1, border.Content);

		border.Content = label2;
		Assert.Equal(label2, border.Content);
	}

	[Fact]
	public void ContentCanBeCleared()
	{
		var border = new Border();
		var label = new Label { Text = "Test" };

		border.Content = label;
		Assert.NotNull(border.Content);

		border.Content = null;
		Assert.Null(border.Content);
	}

	[Fact]
	public void AddingToChildrenDoesNotSetContent()
	{
		var border = new Border();
		var label1 = new Label { Text = "Label 1" };
		var label2 = new Label { Text = "Label 2" };

		// Border should only support Content, not Children collection
		// Children can be added to the collection but they won't be rendered
		border.Children.Add(label1);
		border.Children.Add(label2);

		// Children collection is modified but Content remains null
		Assert.Equal(2, border.Children.Count);
		Assert.Null(border.Content);
	}

	[Fact]
	public void BackgroundColorCanBeSet()
	{
		var border = new Border
		{
			BackgroundColor = Colors.Blue
		};
		Assert.Equal(Colors.Blue, border.BackgroundColor);
	}

	[Fact]
	public void BackgroundColorCanBeChanged()
	{
		var border = new Border
		{
			BackgroundColor = Colors.Blue
		};
		Assert.Equal(Colors.Blue, border.BackgroundColor);

		border.BackgroundColor = Colors.Red;
		Assert.Equal(Colors.Red, border.BackgroundColor);

		border.BackgroundColor = null;
		Assert.Null(border.BackgroundColor);
	}

	[Fact]
	public void StrokeThicknessCanBeSetWhenStrokeIsNull()
	{
		var border = new Border
		{
			Stroke = null,
			StrokeThickness = 5.0
		};
		Assert.Null(border.Stroke);
		Assert.Equal(5.0, border.StrokeThickness);

		// Setting Stroke after StrokeThickness should use the correct thickness
		border.Stroke = Colors.Red;
		Assert.Equal(Colors.Red, border.Stroke);
		Assert.Equal(5.0, border.StrokeThickness);
	}
}
