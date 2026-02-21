namespace Spice;

/// <summary>
/// Defines a set of semantic colors that are auto-applied to views.
/// Extends ObservableObject so that changing a color fires PropertyChanged
/// and updates all subscribed views live.
/// </summary>
public partial class Theme : ObservableObject
{
	// Avoid using the Colors class to prevent pulling Microsoft.Maui.Graphics
	// into the AOT-compiled output — keeps APK size down.
	static readonly Color Black = new(0f, 0f, 0f);
	static readonly Color White = new(1f, 1f, 1f);
	static readonly Color DarkGray = new(169f / 255f, 169f / 255f, 169f / 255f);
	static readonly Color LightGray = new(211f / 255f, 211f / 255f, 211f / 255f);

	/// <summary>Default text color for Label, Button, Entry, SearchBar, etc.</summary>
	[ObservableProperty]
	Color? _textColor;

	/// <summary>Default background color for all views.</summary>
	[ObservableProperty]
	Color? _backgroundColor;

	/// <summary>Accent/tint color for interactive controls (Button background, Switch tint, ActivityIndicator, etc.)</summary>
	[ObservableProperty]
	Color? _accentColor;

	/// <summary>Border/stroke color for Border views.</summary>
	[ObservableProperty]
	Color? _strokeColor;

	/// <summary>Placeholder text color for Editor, SearchBar, etc.</summary>
	[ObservableProperty]
	Color? _placeholderColor;

	/// <summary>
	/// A light theme with sensible defaults.
	/// </summary>
	public static Theme Light => new()
	{
		TextColor = Black,
		BackgroundColor = White,
		AccentColor = new Color(0f, 0.471f, 0.831f),       // #0078D4
		StrokeColor = new Color(0.878f, 0.878f, 0.878f),   // #E0E0E0
		PlaceholderColor = DarkGray,
	};

	/// <summary>
	/// A dark theme with sensible defaults.
	/// </summary>
	public static Theme Dark => new()
	{
		TextColor = White,
		BackgroundColor = new Color(0.118f, 0.118f, 0.118f), // #1E1E1E
		AccentColor = new Color(0.298f, 0.761f, 1f),         // #4CC2FF
		StrokeColor = new Color(0.251f, 0.251f, 0.251f),     // #404040
		PlaceholderColor = LightGray,
	};
}
