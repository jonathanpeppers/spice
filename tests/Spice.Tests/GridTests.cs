namespace Spice.Tests;

/// <summary>
/// Tests for Grid functionality
/// </summary>
public class GridTests
{
	[Fact]
	public void CanCreate()
	{
		var grid = new Grid();
		Assert.NotNull(grid);
	}

	[Fact]
	public void DefaultSpacingIsZero()
	{
		var grid = new Grid();
		Assert.Equal(0, grid.RowSpacing);
		Assert.Equal(0, grid.ColumnSpacing);
	}

	[Fact]
	public void CanSetSpacing()
	{
		var grid = new Grid
		{
			RowSpacing = 10,
			ColumnSpacing = 20
		};
		Assert.Equal(10, grid.RowSpacing);
		Assert.Equal(20, grid.ColumnSpacing);
	}

	[Fact]
	public void RowDefinitionsStartsEmpty()
	{
		var grid = new Grid();
		Assert.Empty(grid.RowDefinitions);
	}

	[Fact]
	public void ColumnDefinitionsStartsEmpty()
	{
		var grid = new Grid();
		Assert.Empty(grid.ColumnDefinitions);
	}

	[Fact]
	public void CanAddRowDefinitions()
	{
		var grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
		grid.RowDefinitions.Add(new RowDefinition(new GridLength(100)));
		grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

		Assert.Equal(3, grid.RowDefinitions.Count);
		Assert.True(grid.RowDefinitions[0].Height.IsAuto);
		Assert.True(grid.RowDefinitions[1].Height.IsAbsolute);
		Assert.Equal(100, grid.RowDefinitions[1].Height.Value);
		Assert.True(grid.RowDefinitions[2].Height.IsStar);
	}

	[Fact]
	public void CanAddColumnDefinitions()
	{
		var grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(200)));
		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

		Assert.Equal(3, grid.ColumnDefinitions.Count);
		Assert.True(grid.ColumnDefinitions[0].Width.IsAuto);
		Assert.True(grid.ColumnDefinitions[1].Width.IsAbsolute);
		Assert.Equal(200, grid.ColumnDefinitions[1].Width.Value);
		Assert.True(grid.ColumnDefinitions[2].Width.IsStar);
		Assert.Equal(2, grid.ColumnDefinitions[2].Width.Value);
	}

	[Fact]
	public void CanAddChildren()
	{
		var grid = new Grid();
		var label1 = new Label { Text = "Test 1" };
		var label2 = new Label { Text = "Test 2" };
		
		grid.Add(label1);
		grid.Add(label2);

		Assert.Equal(2, grid.Children.Count);
		Assert.Equal(label1, grid.Children[0]);
		Assert.Equal(label2, grid.Children[1]);
	}

	[Fact]
	public void DefaultRowIsZero()
	{
		var view = new Label { Text = "Test" };
		Assert.Equal(0, Grid.GetRow(view));
	}

	[Fact]
	public void DefaultColumnIsZero()
	{
		var view = new Label { Text = "Test" };
		Assert.Equal(0, Grid.GetColumn(view));
	}

	[Fact]
	public void DefaultRowSpanIsOne()
	{
		var view = new Label { Text = "Test" };
		Assert.Equal(1, Grid.GetRowSpan(view));
	}

	[Fact]
	public void DefaultColumnSpanIsOne()
	{
		var view = new Label { Text = "Test" };
		Assert.Equal(1, Grid.GetColumnSpan(view));
	}

	[Fact]
	public void CanSetRow()
	{
		var view = new Label { Text = "Test" };
		Grid.SetRow(view, 2);
		Assert.Equal(2, Grid.GetRow(view));
	}

	[Fact]
	public void CanSetColumn()
	{
		var view = new Label { Text = "Test" };
		Grid.SetColumn(view, 3);
		Assert.Equal(3, Grid.GetColumn(view));
	}

	[Fact]
	public void CanSetRowSpan()
	{
		var view = new Label { Text = "Test" };
		Grid.SetRowSpan(view, 2);
		Assert.Equal(2, Grid.GetRowSpan(view));
	}

	[Fact]
	public void CanSetColumnSpan()
	{
		var view = new Label { Text = "Test" };
		Grid.SetColumnSpan(view, 3);
		Assert.Equal(3, Grid.GetColumnSpan(view));
	}

	[Fact]
	public void RowCannotBeNegative()
	{
		var view = new Label { Text = "Test" };
		Assert.Throws<ArgumentOutOfRangeException>(() => Grid.SetRow(view, -1));
	}

	[Fact]
	public void ColumnCannotBeNegative()
	{
		var view = new Label { Text = "Test" };
		Assert.Throws<ArgumentOutOfRangeException>(() => Grid.SetColumn(view, -1));
	}

	[Fact]
	public void RowSpanCannotBeLessThanOne()
	{
		var view = new Label { Text = "Test" };
		Assert.Throws<ArgumentOutOfRangeException>(() => Grid.SetRowSpan(view, 0));
	}

	[Fact]
	public void ColumnSpanCannotBeLessThanOne()
	{
		var view = new Label { Text = "Test" };
		Assert.Throws<ArgumentOutOfRangeException>(() => Grid.SetColumnSpan(view, 0));
	}

	[Fact]
	public void CanCreateComplexGrid()
	{
		var grid = new Grid
		{
			RowSpacing = 5,
			ColumnSpacing = 10
		};

		grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
		grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
		grid.RowDefinitions.Add(new RowDefinition(new GridLength(50)));

		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(100)));
		grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
		grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

		var label1 = new Label { Text = "Header" };
		Grid.SetRow(label1, 0);
		Grid.SetColumn(label1, 0);
		Grid.SetColumnSpan(label1, 3);

		var label2 = new Label { Text = "Content" };
		Grid.SetRow(label2, 1);
		Grid.SetColumn(label2, 1);

		var label3 = new Label { Text = "Footer" };
		Grid.SetRow(label3, 2);
		Grid.SetColumn(label3, 0);
		Grid.SetColumnSpan(label3, 3);

		grid.Add(label1);
		grid.Add(label2);
		grid.Add(label3);

		Assert.Equal(3, grid.RowDefinitions.Count);
		Assert.Equal(3, grid.ColumnDefinitions.Count);
		Assert.Equal(3, grid.Children.Count);

		Assert.Equal(0, Grid.GetRow(label1));
		Assert.Equal(0, Grid.GetColumn(label1));
		Assert.Equal(3, Grid.GetColumnSpan(label1));

		Assert.Equal(1, Grid.GetRow(label2));
		Assert.Equal(1, Grid.GetColumn(label2));

		Assert.Equal(2, Grid.GetRow(label3));
		Assert.Equal(0, Grid.GetColumn(label3));
		Assert.Equal(3, Grid.GetColumnSpan(label3));
	}

	[Fact]
	public void PropertyChangedFiresOnRowSpacingChange()
	{
		string? property = null;
		var grid = new Grid();
		grid.PropertyChanged += (sender, e) => property = e.PropertyName;
		grid.RowSpacing = 15;

		Assert.Equal(nameof(grid.RowSpacing), property);
	}

	[Fact]
	public void PropertyChangedFiresOnColumnSpacingChange()
	{
		string? property = null;
		var grid = new Grid();
		grid.PropertyChanged += (sender, e) => property = e.PropertyName;
		grid.ColumnSpacing = 20;

		Assert.Equal(nameof(grid.ColumnSpacing), property);
	}

	[Fact]
	public void DefaultPaddingIsZero()
	{
		var grid = new Grid();
		Assert.Equal(0.0, grid.Padding);
	}

	[Fact]
	public void CanSetPadding()
	{
		var grid = new Grid
		{
			Padding = 15.0
		};
		Assert.Equal(15.0, grid.Padding);
	}

	[Fact]
	public void PropertyChangedFiresOnPaddingChange()
	{
		string? property = null;
		var grid = new Grid();
		grid.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		grid.Padding = 25.0;

		Assert.Equal(nameof(grid.Padding), property);
	}
}
