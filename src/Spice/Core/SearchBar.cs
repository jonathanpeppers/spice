namespace Spice;

/// <summary>
/// Control for search input with a search button.
/// Android -> Android.Widget.SearchView
/// iOS -> UIKit.UISearchBar
/// </summary>
public partial class SearchBar : View
{
	/// <summary>
	/// The search text input by the user
	/// </summary>
	[ObservableProperty]
	string _text = "";

	/// <summary>
	/// Placeholder text displayed when the search bar is empty
	/// </summary>
	[ObservableProperty]
	string? _placeholder;

	/// <summary>
	/// Color of the search text
	/// </summary>
	[ObservableProperty]
	Color? _textColor;

	/// <summary>
	/// Color of the placeholder text
	/// </summary>
	[ObservableProperty]
	Color? _placeholderColor;

	/// <summary>
	/// Action to run when the search button is pressed
	/// </summary>
	[ObservableProperty]
	Action<SearchBar>? _searchButtonPressed;

	/// <summary>
	/// Action to run when the text changes
	/// </summary>
	[ObservableProperty]
	Action<SearchBar>? _textChanged;

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		_isApplyingTheme = true;
		if (CanApplyTheme((int)ThemeProperty.TextColor))
			TextColor = theme.TextColor;
		if (CanApplyTheme((int)ThemeProperty.PlaceholderColor))
			PlaceholderColor = theme.PlaceholderColor;
		_isApplyingTheme = false;
	}

	partial void OnTextColorChanging(Color? value) => TrackExplicit((int)ThemeProperty.TextColor, value);

	partial void OnPlaceholderColorChanging(Color? value) => TrackExplicit((int)ThemeProperty.PlaceholderColor, value);
}
