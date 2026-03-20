namespace Spice.Scenarios;

public class App : Application
{
	public App()
	{
		UseSystemTheme = true;

		Main = new NavigationView(new ScenarioList());
	}
}

class ScenarioList : StackLayout
{
	public ScenarioList()
	{
		Title = "Scenarios";

		Add(new Button { Text = "Hello World", Clicked = _ => Navigation!.Push<HelloWorldScenario>() });
		Add(new Button { Text = "Ghost Button", Clicked = _ => Navigation!.Push<GhostButtonScenario>() });
		Add(new Button { Text = "ImageButton", Clicked = _ => Navigation!.Push<ImageButtonScenario>() });
		Add(new Button { Text = "WebView", Clicked = _ => Navigation!.Push<WebViewScenario>() });
		Add(new Button { Text = "Entry", Clicked = _ => Navigation!.Push<EntryScenario>() });
		Add(new Button { Text = "Editor", Clicked = _ => Navigation!.Push<EditorScenario>() });
		Add(new Button { Text = "DatePicker", Clicked = _ => Navigation!.Push<DatePickerScenario>() });
		Add(new Button { Text = "TimePicker", Clicked = _ => Navigation!.Push<TimePickerScenario>() });
		Add(new Button { Text = "ActivityIndicator", Clicked = _ => Navigation!.Push<ActivityIndicatorScenario>() });
		Add(new Button { Text = "BoxView", Clicked = _ => Navigation!.Push<BoxViewScenario>() });
		Add(new Button { Text = "Switch", Clicked = _ => Navigation!.Push<SwitchScenario>() });
		Add(new Button { Text = "ProgressBar", Clicked = _ => Navigation!.Push<ProgressBarScenario>() });
		Add(new Button { Text = "ScrollView", Clicked = _ => Navigation!.Push<ScrollViewScenario>() });
		Add(new Button { Text = "RefreshView", Clicked = _ => Navigation!.Push<RefreshViewScenario>() });
		Add(new Button { Text = "Picker", Clicked = _ => Navigation!.Push<PickerScenario>() });
		Add(new Button { Text = "CheckBox", Clicked = _ => Navigation!.Push<CheckBoxScenario>() });
		Add(new Button { Text = "ContentView", Clicked = _ => Navigation!.Push<ContentViewScenario>() });
		Add(new Button { Text = "Slider", Clicked = _ => Navigation!.Push<SliderScenario>() });
		Add(new Button { Text = "Border", Clicked = _ => Navigation!.Push<BorderScenario>() });
		Add(new Button { Text = "Margin", Clicked = _ => Navigation!.Push<MarginScenario>() });
		Add(new Button { Text = "CollectionView", Clicked = _ => Navigation!.Push<CollectionViewScenario>() });
		Add(new Button { Text = "SearchBar", Clicked = _ => Navigation!.Push<SearchBarScenario>() });
		Add(new Button { Text = "SwipeView", Clicked = _ => Navigation!.Push<SwipeViewScenario>() });
		Add(new Button { Text = "Theme", Clicked = _ => Navigation!.Push<ThemeScenario>() });
	}
}