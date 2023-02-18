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

		// TODO: work on design here
		Recycled = (view, user) => ((Label)view.Children[1]).Text = $"{user.FirstName} {user.LastName}";

		var list = new List<User>();
		for (int i = 0; i < 10; i++)
		{
			list.AddRange(new[]
			{
				new User("Chuck", "Norris"),
				new User("Jon", "Peppers"),
				new User("Bob", "Dole"),
				new User("Steve", "Rogers"),
				new User("Will", "Ferrell"),
				new User("Tony", "Stark"),
				new User("Peter", "Parker"),
				new User("Bruce", "Banner"),
			});
		}

		Items = list;
	}
}

public record User(string FirstName, string LastName);

