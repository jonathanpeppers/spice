using System.ComponentModel;

namespace Spice;

/// <summary>
/// The root "view" of a Spice application. Set Main to a single view.
/// </summary>
public partial class Application : View
{
	bool _isSettingSystemTheme;

	/// <summary>
	/// The single, main "view" of this application
	/// </summary>
	[ObservableProperty]
	View? _main;

	/// <summary>
	/// The current theme. Setting this applies colors to the entire view tree
	/// and subscribes to live updates. Null means no theme — views keep their
	/// individually-set colors (backward compatible default).
	/// </summary>
	[ObservableProperty]
	Theme? _theme;

	/// <summary>
	/// When true, automatically sets Theme to Theme.Light or Theme.Dark
	/// based on the OS appearance, and updates live when the system
	/// appearance changes.
	/// </summary>
	[ObservableProperty]
	bool _useSystemTheme;

	/// <summary>
	/// Optional callback invoked when the system appearance changes.
	/// The bool parameter is true when the system switched to dark mode.
	/// Use this to swap custom themes based on OS appearance.
	/// </summary>
	[ObservableProperty]
	Action<bool>? _appearanceChanged;

	partial void OnThemeChanging(Theme? value)
	{
		if (!_isSettingSystemTheme)
			UseSystemTheme = false;
	}

	partial void OnThemeChanged(Theme? oldValue, Theme? newValue)
	{
		if (oldValue is not null)
			oldValue.PropertyChanged -= OnThemePropertyChanged;

		if (newValue is not null)
		{
			newValue.PropertyChanged += OnThemePropertyChanged;
			ApplyThemeToTree(Main, newValue);
		}
	}

	partial void OnMainChanged(View? oldValue, View? newValue)
	{
		if (newValue is not null && Theme is not null)
			ApplyThemeToTree(newValue, Theme);
	}

	void OnThemePropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (Theme is not null)
			ApplyThemeToTree(Main, Theme);
	}

	partial void OnUseSystemThemeChanged(bool value)
	{
		if (value)
		{
			PlatformAppearance.Changed += OnPlatformAppearanceChanged;
			_isSettingSystemTheme = true;
			Theme = PlatformAppearance.IsDarkMode ? Theme.Dark : Theme.Light;
			_isSettingSystemTheme = false;
		}
		else
		{
			PlatformAppearance.Changed -= OnPlatformAppearanceChanged;
		}
	}

	void OnPlatformAppearanceChanged(bool isDarkMode)
	{
		if (_useSystemTheme)
		{
			_isSettingSystemTheme = true;
			Theme = isDarkMode ? Theme.Dark : Theme.Light;
			_isSettingSystemTheme = false;
		}
		AppearanceChanged?.Invoke(isDarkMode);
	}
}
