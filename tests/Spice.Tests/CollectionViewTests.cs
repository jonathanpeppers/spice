namespace Spice.Tests;

/// <summary>
/// Tests for CollectionView functionality
/// </summary>
public class CollectionViewTests
{
	[Fact]
	public void CanCreate()
	{
		var collectionView = new CollectionView();
		Assert.NotNull(collectionView);
	}

	[Fact]
	public void DefaultOrientationIsVertical()
	{
		var collectionView = new CollectionView();
		Assert.Equal(Orientation.Vertical, collectionView.Orientation);
	}

	[Fact]
	public void DefaultSelectionModeIsNone()
	{
		var collectionView = new CollectionView();
		Assert.Equal(SelectionMode.None, collectionView.SelectionMode);
	}

	[Fact]
	public void DefaultItemSpacingIsZero()
	{
		var collectionView = new CollectionView();
		Assert.Equal(0, collectionView.ItemSpacing);
	}

	[Fact]
	public void ItemsSourceIsNullByDefault()
	{
		var collectionView = new CollectionView();
		Assert.Null(collectionView.ItemsSource);
	}

	[Fact]
	public void ItemTemplateIsNullByDefault()
	{
		var collectionView = new CollectionView();
		Assert.Null(collectionView.ItemTemplate);
	}

	[Fact]
	public void SelectedItemIsNullByDefault()
	{
		var collectionView = new CollectionView();
		Assert.Null(collectionView.SelectedItem);
	}

	[Fact]
	public void CanSetItemsSource()
	{
		var collectionView = new CollectionView();
		var items = new List<string> { "Item 1", "Item 2", "Item 3" };
		
		collectionView.ItemsSource = items;
		
		Assert.NotNull(collectionView.ItemsSource);
		Assert.Equal(items, collectionView.ItemsSource);
	}

	[Fact]
	public void CanSetItemTemplate()
	{
		var collectionView = new CollectionView();
		Func<object, View> template = item => new Label { Text = item.ToString() };
		
		collectionView.ItemTemplate = template;
		
		Assert.NotNull(collectionView.ItemTemplate);
		Assert.Equal(template, collectionView.ItemTemplate);
	}

	[Fact]
	public void CanSetOrientation()
	{
		var collectionView = new CollectionView
		{
			Orientation = Orientation.Horizontal
		};
		Assert.Equal(Orientation.Horizontal, collectionView.Orientation);
	}

	[Fact]
	public void CanSetSelectionMode()
	{
		var collectionView = new CollectionView
		{
			SelectionMode = SelectionMode.Single
		};
		Assert.Equal(SelectionMode.Single, collectionView.SelectionMode);
	}

	[Fact]
	public void CanSetItemSpacing()
	{
		var collectionView = new CollectionView
		{
			ItemSpacing = 10
		};
		Assert.Equal(10, collectionView.ItemSpacing);
	}

	[Fact]
	public void CanSetSelectedItem()
	{
		var collectionView = new CollectionView
		{
			SelectionMode = SelectionMode.Single
		};
		var item = "Selected Item";
		
		collectionView.SelectedItem = item;
		
		Assert.Equal(item, collectionView.SelectedItem);
	}

	[Fact]
	public void PropertyChangedFiresOnItemsSourceChange()
	{
		string? property = null;
		var collectionView = new CollectionView();
		collectionView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		collectionView.ItemsSource = new List<string> { "Item" };

		Assert.Equal(nameof(collectionView.ItemsSource), property);
	}

	[Fact]
	public void PropertyChangedFiresOnItemTemplateChange()
	{
		string? property = null;
		var collectionView = new CollectionView();
		collectionView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		collectionView.ItemTemplate = item => new Label { Text = item.ToString() };

		Assert.Equal(nameof(collectionView.ItemTemplate), property);
	}

	[Fact]
	public void PropertyChangedFiresOnOrientationChange()
	{
		string? property = null;
		var collectionView = new CollectionView();
		collectionView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		collectionView.Orientation = Orientation.Horizontal;

		Assert.Equal(nameof(collectionView.Orientation), property);
	}

	[Fact]
	public void PropertyChangedFiresOnSelectionModeChange()
	{
		string? property = null;
		var collectionView = new CollectionView();
		collectionView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		collectionView.SelectionMode = SelectionMode.Single;

		Assert.Equal(nameof(collectionView.SelectionMode), property);
	}

	[Fact]
	public void PropertyChangedFiresOnSelectedItemChange()
	{
		string? property = null;
		var collectionView = new CollectionView();
		collectionView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		collectionView.SelectedItem = "Item";

		Assert.Equal(nameof(collectionView.SelectedItem), property);
	}

	[Fact]
	public void PropertyChangedFiresOnItemSpacingChange()
	{
		string? property = null;
		var collectionView = new CollectionView();
		collectionView.PropertyChanged += (sender, e) => property = e.PropertyName;
		
		collectionView.ItemSpacing = 5;

		Assert.Equal(nameof(collectionView.ItemSpacing), property);
	}

	[Fact]
	public void ItemTemplateCreatesViewForItem()
	{
		var collectionView = new CollectionView
		{
			ItemTemplate = item => new Label { Text = item.ToString() }
		};

		var view = collectionView.ItemTemplate!("Test Item");
		
		Assert.NotNull(view);
		Assert.IsType<Label>(view);
		Assert.Equal("Test Item", ((Label)view).Text);
	}

	[Fact]
	public void ItemTemplateCanReturnDifferentViewTypes()
	{
		var collectionView = new CollectionView
		{
			ItemTemplate = item =>
			{
				if (item is string s && s.StartsWith("Button"))
					return new Button { Text = s };
				else
					return new Label { Text = item.ToString() };
			}
		};

		var labelView = collectionView.ItemTemplate!("Label Item");
		var buttonView = collectionView.ItemTemplate!("Button Item");
		
		Assert.IsType<Label>(labelView);
		Assert.IsType<Button>(buttonView);
	}

	[Fact]
	public void CanUseObservableCollectionAsItemsSource()
	{
		var collectionView = new CollectionView();
		var items = new System.Collections.ObjectModel.ObservableCollection<string>
		{
			"Item 1", "Item 2"
		};
		
		collectionView.ItemsSource = items;
		
		Assert.NotNull(collectionView.ItemsSource);
		Assert.Equal(2, items.Count);
	}

	[Fact]
	public void CanReplaceItemsSource()
	{
		var collectionView = new CollectionView();
		var firstItems = new List<string> { "Item 1" };
		var secondItems = new List<string> { "Item A", "Item B" };

		collectionView.ItemsSource = firstItems;
		Assert.Equal(firstItems, collectionView.ItemsSource);

		collectionView.ItemsSource = secondItems;
		Assert.Equal(secondItems, collectionView.ItemsSource);
	}

	[Fact]
	public void CanClearItemsSource()
	{
		var collectionView = new CollectionView();
		var items = new List<string> { "Item 1" };
		
		collectionView.ItemsSource = items;
		Assert.NotNull(collectionView.ItemsSource);

		collectionView.ItemsSource = null;
		Assert.Null(collectionView.ItemsSource);
	}

	[Fact]
	public void MultipleSelectionModeAllowsMultipleItems()
	{
		var collectionView = new CollectionView
		{
			SelectionMode = SelectionMode.Multiple
		};

		Assert.Equal(SelectionMode.Multiple, collectionView.SelectionMode);
	}

	[Fact]
	public void CanCreateCompleteCollectionView()
	{
		var items = new List<string> { "Apple", "Banana", "Cherry" };
		var collectionView = new CollectionView
		{
			ItemsSource = items,
			ItemTemplate = item => new Label { Text = item.ToString() },
			Orientation = Orientation.Vertical,
			SelectionMode = SelectionMode.Single,
			ItemSpacing = 5
		};

		Assert.NotNull(collectionView.ItemsSource);
		Assert.NotNull(collectionView.ItemTemplate);
		Assert.Equal(Orientation.Vertical, collectionView.Orientation);
		Assert.Equal(SelectionMode.Single, collectionView.SelectionMode);
		Assert.Equal(5, collectionView.ItemSpacing);
	}
}
