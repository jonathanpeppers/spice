using System.Collections.ObjectModel;

namespace Spice.Scenarios;

/// <summary>
/// Scenario demonstrating CollectionView with lambda-based item templates
/// </summary>
class CollectionViewScenario : StackLayout
{
	public CollectionViewScenario()
	{
		Spacing = 10;

		// Title
		Add(new Label
		{
			Text = "CollectionView Demo 📋",
			TextColor = Colors.Blue
		});

		// Create data source
		var items = new ObservableCollection<Person>
		{
			new Person { Name = "Alice", Age = 30, Color = Colors.Red },
			new Person { Name = "Bob", Age = 25, Color = Colors.Green },
			new Person { Name = "Charlie", Age = 35, Color = Colors.Blue },
			new Person { Name = "Diana", Age = 28, Color = Colors.Orange },
			new Person { Name = "Eve", Age = 32, Color = Colors.Purple },
			new Person { Name = "Frank", Age = 40, Color = Colors.Brown },
			new Person { Name = "Grace", Age = 27, Color = Colors.Pink },
			new Person { Name = "Henry", Age = 45, Color = Colors.DarkGreen }
		};

		// Label to show selected item
		var selectedLabel = new Label
		{
			Text = "Select an item from the list",
			TextColor = Colors.Gray
		};

		// Create CollectionView with lambda-based item template
		var collectionView = new CollectionView
		{
			ItemsSource = items,
			ItemTemplate = item =>
			{
				var person = (Person)item;
				return new Border
				{
					BackgroundColor = person.Color.WithAlpha(0.2f),
					Content = new StackLayout
					{
						Spacing = 5,
						Children =
						{
							new Label
							{
								Text = person.Name,
								TextColor = person.Color
							},
							new Label
							{
								Text = $"Age: {person.Age}",
								TextColor = Colors.Gray
							}
						}
					}
				};
			},
			SelectionMode = SelectionMode.Single,
			Orientation = Orientation.Vertical,
			ItemSpacing = 10
		};

		// Handle selection changes
		collectionView.PropertyChanged += (sender, e) =>
		{
			if (e.PropertyName == nameof(CollectionView.SelectedItem) && collectionView.SelectedItem is Person selected)
			{
				selectedLabel.Text = $"Selected: {selected.Name}, Age: {selected.Age}";
				selectedLabel.TextColor = selected.Color;
			}
		};

		// Button to add new items
		int newItemCount = 0;
		var addButton = new Button
		{
			Text = "Add New Person",
			Clicked = _ =>
			{
				newItemCount++;
				items.Add(new Person
				{
					Name = $"Person {newItemCount}",
					Age = 20 + newItemCount,
					Color = Colors.Teal
				});
			}
		};

		// Button to clear selection
		var clearButton = new Button
		{
			Text = "Clear Selection",
			Clicked = _ =>
			{
				collectionView.SelectedItem = null;
				selectedLabel.Text = "No item selected";
				selectedLabel.TextColor = Colors.Gray;
			}
		};

		// Add controls to layout
		Add(selectedLabel);
		Add(new ScrollView { Children = { collectionView } });
		Add(new StackLayout
		{
			Orientation = Orientation.Horizontal,
			Spacing = 10,
			Children = { addButton, clearButton }
		});
	}
}

class Person
{
	public string Name { get; set; } = "";
	public int Age { get; set; }
	public Color Color { get; set; } = Colors.Black;
}
