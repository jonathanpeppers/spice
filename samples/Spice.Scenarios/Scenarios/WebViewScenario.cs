namespace Spice.Scenarios;

/// <summary>
/// Scenario navigating a WebView
/// </summary>
class WebViewScenario : WebView
{
	public WebViewScenario()
	{
		Title = "Web View";
		Source = "https://www.google.com";
	}
}
