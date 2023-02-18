namespace Spice.Scenarios;

public class App : Application
{
	public App()
	{
		BackgroundColor = Colors.CornflowerBlue;

		// Comment/uncomment to try different scenarios/samples
		Main = new HelloWorldScenario();
		//Main = new GhostButtonScenario();
		//Main = new ImageModeScenario();
	}
}