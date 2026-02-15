namespace Spice.Scenarios;

/// <summary>
/// Scenario demonstrating RefreshView with pull-to-refresh functionality
/// </summary>
class RefreshViewScenario : StackLayout
{
	int _refreshCount = 0;

	public RefreshViewScenario()
	{
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
				// Simulate a refresh operation
				_refreshCount++;
				countLabel.Text = $"Refresh count: {_refreshCount}";

				// In a real app, you would fetch new data here
				// For demo purposes, we just stop refreshing after a short delay
				Task.Run(async () =>
				{
					await Task.Delay(1500);
					refreshView.IsRefreshing = false;
				});
			}
		};

		var manualRefreshButton = new Button
		{
			Text = "Trigger Refresh Manually",
			Clicked = _ =>
			{
				refreshView.IsRefreshing = true;
				refreshView.Command?.Invoke();
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
