namespace Spice.Scenarios;

public class App : Application
{
	public App()
	{
		BackgroundColor = Colors.CornflowerBlue;

		Main = new StackView
		{
			Children =
			{
				new Button
				{
					Text = "Hello World",
					Clicked = _ => Main = new HelloWorldScenario(),
				},
				new Button
				{
					Text = "Ghost Button",
					Clicked = _ => Main = new GhostButtonScenario(),
				},
				new Button
				{
					Text = "WebView",
					Clicked = _ => Main = new WebViewScenario(),
				},
				new Button
				{
					Text = "Entry",
					Clicked = _ => Main = new EntryScenario(),
				},
				new Button
				{
					Text = "Switch",
					Clicked = _ => Main = new SwitchScenario(),
				},
				new Button
				{
					Text = "ProgressBar",
					Clicked = _ => Main = new ProgressBarScenario(),
				},
				new Button
				{
					Text = "Slider",
					Clicked = _ => Main = new SliderScenario(),
				},
			}
		};
	}
}