namespace Spice.BlazorSample;

public class App : Application
{
	public App()
	{
		Main = new BlazorWebView
		{
			HostPage = "wwwroot/index.html",
			RootComponents =
			{
				new RootComponent { Selector = "#app", ComponentType = typeof(Main) }
			},
		};
	}
}