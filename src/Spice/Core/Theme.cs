using Microsoft.Maui.Graphics;

namespace Spice;

/// <summary>
/// Defines a set of semantic colors that are auto-applied to views.
/// Extends ObservableObject so that changing a color fires PropertyChanged
/// and updates all subscribed views live.
/// </summary>
public partial class Theme : ObservableObject
{
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
		TextColor = Colors.Black,
		BackgroundColor = Colors.White,
		AccentColor = Color.FromArgb("#0078D4"),
		StrokeColor = Color.FromArgb("#E0E0E0"),
		PlaceholderColor = Colors.DarkGray,
	};

	/// <summary>
	/// A dark theme with sensible defaults.
	/// </summary>
	public static Theme Dark => new()
	{
		TextColor = Colors.White,
		BackgroundColor = Color.FromArgb("#1E1E1E"),
		AccentColor = Color.FromArgb("#4CC2FF"),
		StrokeColor = Color.FromArgb("#404040"),
		PlaceholderColor = Colors.LightGray,
	};
}
