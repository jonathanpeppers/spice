using System.Collections.ObjectModel;

namespace Spice;

/// <summary>
/// Picker control for selecting an item from a list.
/// Android -> Android.Widget.Spinner
/// iOS -> UIKit.UIPickerView
/// </summary>
public partial class Picker : View
{
	bool _isTextColorSet;
	Color? _themedTextColor;

	/// <summary>
	/// The collection of items to display in the picker
	/// </summary>
	[ObservableProperty]
	ObservableCollection<string> _items = new();

	/// <summary>
	/// The index of the currently selected item, or -1 if no item is selected
	/// </summary>
	[ObservableProperty]
	int _selectedIndex = -1;

	/// <summary>
	/// The currently selected item, or null if no item is selected
	/// </summary>
	public string? SelectedItem => _selectedIndex >= 0 && _selectedIndex < Items.Count ? Items[_selectedIndex] : null;

	partial void OnSelectedIndexChanged(int oldValue, int newValue)
	{
		OnPropertyChanged(nameof(SelectedItem));
	}

	partial void OnItemsChanged(ObservableCollection<string> value)
	{
		OnPropertyChanged(nameof(SelectedItem));
	}

	/// <summary>
	/// Optional title text to display for the picker
	/// </summary>
	[ObservableProperty]
	string _title = "";

	/// <summary>
	/// Color of the text
	/// </summary>
	[ObservableProperty]
	Color? _textColor;

	/// <inheritdoc />
	protected override void ApplyTheme(Theme theme)
	{
		base.ApplyTheme(theme);
		_isApplyingTheme = true;
		_themedTextColor = theme.TextColor;
		if (!_isTextColorSet)
			TextColor = _themedTextColor;
		_isApplyingTheme = false;
	}

	partial void OnTextColorChanging(Color? value)
	{
		if (!_isApplyingTheme)
			_isTextColorSet = value is not null;
	}
}
