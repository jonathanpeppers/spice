namespace Spice.Scenarios;

/// <summary>
/// Scenario navigating a WebView
/// </summary>
class WebViewScenario : StackView
{
	public WebViewScenario()
	{
		HorizontalAlign = Align.Stretch;
		VerticalAlign = Align.Stretch;

		var webView = new WebView
		{
			Source = "https://www.google.com",
			VerticalAlign = Align.Stretch,
			HorizontalAlign = Align.Stretch,
		};

		var back = new Button
		{
			Text = "<",
			Clicked = _ => webView.Back(),
		};
		var forward = new Button
		{
			Text = ">",
			Clicked = _ => webView.Forward(),
		};

		Add(new StackView
		{
			Orientation = Orientation.Horizontal,
			Children =
			{
				back, forward,
			}
		});
		Add(webView);
	}
}
