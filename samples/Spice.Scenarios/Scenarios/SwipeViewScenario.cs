namespace Spice.Scenarios;

/// <summary>
/// Demonstrates SwipeView with swipe gestures to reveal action items
/// </summary>
public class SwipeViewScenario : ContentView
{
	public SwipeViewScenario()
	{
		Title = "Swipe View";
		Content = new ScrollView
		{
			new StackLayout
			{
				new Label
				{
					Text = "SwipeView Demo",
					HorizontalOptions = LayoutOptions.Center
				},
				new Label
				{
					Text = "Swipe items left or right to see actions",
					HorizontalOptions = LayoutOptions.Center
				},

				// Basic swipe view with right items (swipe left to reveal)
				CreateSwipeViewItem("Email from Alice", "Meeting tomorrow at 10am"),

				// Swipe view with left items (swipe right to reveal)
				CreateSwipeViewWithLeftItems("Email from Bob", "Project update attached"),

				// Swipe view with both left and right items
				CreateSwipeViewWithBothItems("Email from Carol", "Quarterly report ready"),

				// Additional examples
				CreateSwipeViewItem("Email from David", "Code review needed"),
				CreateSwipeViewItem("Email from Eve", "New feature proposal"),
			}
		};
	}

	SwipeView CreateSwipeViewItem(string title, string subtitle)
	{
		var swipeView = new SwipeView
		{
			RightItems = new SwipeItems
			{
				Mode = SwipeMode.Reveal,
				SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Close
			},
			Content = new Border
			{
				BackgroundColor = Colors.White,
				Content = new StackLayout
				{
					new Label { Text = title },
					new Label { Text = subtitle }
				}
			}
		};

		// Add swipe items
		swipeView.RightItems.Items.Add(new SwipeItem
		{
			Text = "Delete",
			BackgroundColor = Colors.Red,
			Invoked = item =>
			{
				// Handle delete action
			}
		});

		swipeView.RightItems.Items.Add(new SwipeItem
		{
			Text = "Archive",
			BackgroundColor = Colors.Orange,
			Invoked = item =>
			{
				// Handle archive action
			}
		});

		return swipeView;
	}

	SwipeView CreateSwipeViewWithLeftItems(string title, string subtitle)
	{
		var swipeView = new SwipeView
		{
			LeftItems = new SwipeItems
			{
				Mode = SwipeMode.Reveal,
				SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Close
			},
			Content = new Border
			{
				BackgroundColor = Colors.White,
				Content = new StackLayout
				{
					new Label { Text = title },
					new Label { Text = subtitle }
				}
			}
		};

		// Add left swipe items
		swipeView.LeftItems.Items.Add(new SwipeItem
		{
			Text = "Mark Read",
			BackgroundColor = Colors.Blue,
			Invoked = item =>
			{
				// Handle mark as read
			}
		});

		swipeView.LeftItems.Items.Add(new SwipeItem
		{
			Text = "Flag",
			BackgroundColor = Colors.Green,
			Invoked = item =>
			{
				// Handle flag action
			}
		});

		return swipeView;
	}

	SwipeView CreateSwipeViewWithBothItems(string title, string subtitle)
	{
		var swipeView = new SwipeView
		{
			LeftItems = new SwipeItems
			{
				Mode = SwipeMode.Reveal,
				SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Close
			},
			RightItems = new SwipeItems
			{
				Mode = SwipeMode.Reveal,
				SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Close
			},
			Content = new Border
			{
				BackgroundColor = Colors.White,
				Content = new StackLayout
				{
					new Label { Text = title },
					new Label { Text = subtitle }
				}
			}
		};

		// Left items
		swipeView.LeftItems.Items.Add(new SwipeItem
		{
			Text = "Reply",
			BackgroundColor = Colors.Blue,
			Invoked = item =>
			{
				// Handle reply
			}
		});

		// Right items
		swipeView.RightItems.Items.Add(new SwipeItem
		{
			Text = "Delete",
			BackgroundColor = Colors.Red,
			Invoked = item =>
			{
				// Handle delete
			}
		});

		return swipeView;
	}
}
