namespace Spice.Scenarios;

public class App : Application
{
	public App()
	{
		// Comment/uncomment to try different scenarios/samples
		Main = new HelloWorldScenario();
		//Main = new GhostButtonScenario();
	}
}