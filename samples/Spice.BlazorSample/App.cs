namespace Spice.Scenarios;

public class App : Application
{
	public App()
	{
		Main = new BlazorWebView
		{
			HostPage = "wwwroot/index.html",
		};
	}
}