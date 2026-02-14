namespace Spice.Scenarios;

public class App : Application
{
	public App()
	{
		BackgroundColor = Colors.CornflowerBlue;

		Main = new StackLayout
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
					Text = "DatePicker",
					Clicked = _ => Main = new DatePickerScenario(),
				},
				new Button
				{
					Text = "TimePicker",
					Clicked = _ => Main = new TimePickerScenario(),
				},
				new Button
				{
					Text = "ActivityIndicator",
					Clicked = _ => Main = new ActivityIndicatorScenario(),
				},
				new Button
				{
					Text = "BoxView",
					Clicked = _ => Main = new BoxViewScenario(),
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
					Text = "ScrollView",
					Clicked = _ => Main = new ScrollViewScenario(),
				},
				new Button
				{
					Text = "Picker",
					Clicked = _ => Main = new PickerScenario(),
        },
				new Button
				{
					Text = "ContentView",
					Clicked = _ => Main = new ContentViewScenario(),
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