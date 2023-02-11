using System;

namespace Spice.Scenarios;

public class CollectionViewScenario : CollectionView<User>
{
	public CollectionViewScenario()
	{
		BackgroundColor = Colors.Red;
		ItemTemplate = user => new StackView
		{
			Orientation = Orientation.Horizontal,
			Children =
			{
				new Image { Source = "spice" },
				new Label
				{
					Text = $"{user.FirstName} {user.LastName}",
				}
			}
		};

		Items = new[] {
			new User("Chuck", "Norris"),
			new User("Jon", "Peppers"),
			new User("Bob", "Dole"),
			new User("Steve", "Rogers"),
		};
	}
}

public record User(string FirstName, string LastName);

