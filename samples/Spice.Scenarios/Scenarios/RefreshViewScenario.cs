namespace Spice.Scenarios;

/// <summary>
/// Scenario demonstrating RefreshView with pull-to-refresh functionality
/// </summary>
class RefreshViewScenario : StackLayout
{
	int _refreshCount = 0;

	public RefreshViewScenario()
	{
		Title = "Refresh View";
		Spacing = 10;

		var label = new Label
		{
			Text = "Pull down to refresh! 🔄",
			TextColor = Colors.White
		};

		var countLabel = new Label
		{
			Text = "Refresh count: 0",
			TextColor = Colors.White
		};

		var scrollView = new ScrollView();
		var stackLayout = new StackLayout { Spacing = 10 };

		stackLayout.Add(new Label
		{
			Text = "RefreshView Demo",
			TextColor = Colors.Blue
		});

		// Add items to demonstrate scrolling
		for (int i = 1; i <= 20; i++)
		{
			stackLayout.Add(new Label
			{
				Text = $"Item {i}"
			});
		}

		scrollView.Add(stackLayout);

		var refreshView = new RefreshView
		{
			Content = scrollView,
			RefreshColor = Colors.Blue,
			Command = () =>
			{
				// Increment refresh counter
				_refreshCount++;
				countLabel.Text = $"Refresh count: {_refreshCount}";

				// In a real app, you would:
				// 1. Fetch new data from a server
				// 2. Update your view models/data
				// 3. Set IsRefreshing = false when done
				// 
				// For this demo, the "Trigger Refresh Manually" button below
				// shows how to manually control the refresh state
			}
		};

		var manualRefreshButton = new Button
		{
			Text = "Trigger Refresh Manually",
			Clicked = _ =>
			{
				// Start refreshing
				refreshView.IsRefreshing = true;
				
				// Execute the refresh command
				refreshView.Command?.Invoke();
				
				// In a real app, you would stop refreshing after async work completes
				// For this simple demo, we'll stop it immediately
				refreshView.IsRefreshing = false;
			}
		};

		var colorButton = new Button
		{
			Text = "Change Refresh Color",
			Clicked = _ =>
			{
				// Cycle through colors
				if (refreshView.RefreshColor == Colors.Blue)
					refreshView.RefreshColor = Colors.Red;
				else if (refreshView.RefreshColor == Colors.Red)
					refreshView.RefreshColor = Colors.Green;
				else
					refreshView.RefreshColor = Colors.Blue;
			}
		};

		Add(label);
		Add(countLabel);
		Add(manualRefreshButton);
		Add(colorButton);
		Add(refreshView);
	}
}
