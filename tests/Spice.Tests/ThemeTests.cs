using System.ComponentModel;

namespace Spice.Tests;

/// <summary>
/// Tests for the Theme class and theming infrastructure (Phases 1-4 of THEME-SPEC.md).
/// </summary>
public class ThemeTests
{
	// ── Phase 1: Core Theme Class ──────────────────────────────────

	[Fact]
	public void ThemeCanBeCreated()
	{
		var theme = new Theme();
		Assert.NotNull(theme);
	}

	[Fact]
	public void ThemeDefaultsToNull()
	{
		var theme = new Theme();
		Assert.Null(theme.TextColor);
		Assert.Null(theme.BackgroundColor);
		Assert.Null(theme.AccentColor);
		Assert.Null(theme.StrokeColor);
		Assert.Null(theme.PlaceholderColor);
	}

	[Fact]
	public void ThemeLightHasExpectedColors()
	{
		var theme = Theme.Light;
		Assert.Equal(Colors.Black, theme.TextColor);
		Assert.Equal(Colors.White, theme.BackgroundColor);
		Assert.NotNull(theme.AccentColor);
		Assert.NotNull(theme.StrokeColor);
		Assert.Equal(Colors.DarkGray, theme.PlaceholderColor);
	}

	[Fact]
	public void ThemeDarkHasExpectedColors()
	{
		var theme = Theme.Dark;
		Assert.Equal(Colors.White, theme.TextColor);
		Assert.NotNull(theme.BackgroundColor);
		Assert.NotNull(theme.AccentColor);
		Assert.NotNull(theme.StrokeColor);
		Assert.Equal(Colors.LightGray, theme.PlaceholderColor);
	}

	[Fact]
	public void ThemeFiresPropertyChanged()
	{
		var theme = new Theme();
		string? changedProp = null;
		theme.PropertyChanged += (s, e) => changedProp = e.PropertyName;

		theme.TextColor = Colors.Red;
		Assert.Equal(nameof(Theme.TextColor), changedProp);
	}

	[Fact]
	public void ApplicationThemeDefaultsToNull()
	{
		var app = new Application();
		Assert.Null(app.Theme);
	}

	// ── Phase 1: ApplyTheme on View ────────────────────────────────

	[Fact]
	public void ThemeAppliesBackgroundColorToView()
	{
		var app = new Application();
		var view = new View();
		app.Main = new StackLayout { view };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.White, view.BackgroundColor);
	}

	[Fact]
	public void ThemeAppliesTextColorToLabel()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, label.TextColor);
		Assert.Equal(Colors.White, label.BackgroundColor);
	}

	[Fact]
	public void ThemeAppliesTextColorAndAccentToButton()
	{
		var app = new Application();
		var button = new Button { Text = "Tap" };
		app.Main = new StackLayout { button };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, button.TextColor);
		// Button BackgroundColor should be AccentColor, not BackgroundColor
		Assert.Equal(Theme.Light.AccentColor, button.BackgroundColor);
	}

	[Fact]
	public void ThemeAppliesStrokeColorToBorder()
	{
		var app = new Application();
		var border = new Border();
		app.Main = new StackLayout { border };
		app.Theme = Theme.Light;

		Assert.Equal(Theme.Light.StrokeColor, border.Stroke);
		Assert.Equal(Colors.White, border.BackgroundColor);
	}

	[Fact]
	public void ThemeAppliesTextColorToEntry()
	{
		var app = new Application();
		var entry = new Entry();
		app.Main = new StackLayout { entry };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, entry.TextColor);
	}

	[Fact]
	public void ThemeAppliesTextAndPlaceholderColorToEditor()
	{
		var app = new Application();
		var editor = new Editor();
		app.Main = new StackLayout { editor };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, editor.TextColor);
		Assert.Equal(Colors.DarkGray, editor.PlaceholderColor);
	}

	[Fact]
	public void ThemeAppliesTextAndPlaceholderColorToSearchBar()
	{
		var app = new Application();
		var searchBar = new SearchBar();
		app.Main = new StackLayout { searchBar };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, searchBar.TextColor);
		Assert.Equal(Colors.DarkGray, searchBar.PlaceholderColor);
	}

	[Fact]
	public void ThemeAppliesTextColorToDatePicker()
	{
		var app = new Application();
		var datePicker = new DatePicker();
		app.Main = new StackLayout { datePicker };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, datePicker.TextColor);
	}

	[Fact]
	public void ThemeAppliesTextColorToPicker()
	{
		var app = new Application();
		var picker = new Picker();
		app.Main = new StackLayout { picker };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, picker.TextColor);
	}

	[Fact]
	public void ThemeAppliesAccentColorToActivityIndicator()
	{
		var app = new Application();
		var ai = new ActivityIndicator();
		app.Main = new StackLayout { ai };
		app.Theme = Theme.Light;

		Assert.Equal(Theme.Light.AccentColor, ai.Color);
	}

	// ── Phase 1: Tree Walking ──────────────────────────────────────

	[Fact]
	public void ThemeAppliedToEntireTree()
	{
		var app = new Application();
		var label1 = new Label { Text = "A" };
		var label2 = new Label { Text = "B" };
		var nested = new StackLayout { label2 };
		app.Main = new StackLayout { label1, nested };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, label1.TextColor);
		Assert.Equal(Colors.Black, label2.TextColor);
		Assert.Equal(Colors.White, nested.BackgroundColor);
	}

	[Fact]
	public void ThemeAppliedWhenMainSetAfterTheme()
	{
		var app = new Application();
		app.Theme = Theme.Light;
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };

		Assert.Equal(Colors.Black, label.TextColor);
	}

	[Fact]
	public void SwappingThemeUpdatesAllViews()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };

		app.Theme = Theme.Light;
		Assert.Equal(Colors.Black, label.TextColor);
		Assert.Equal(Colors.White, label.BackgroundColor);

		app.Theme = Theme.Dark;
		Assert.Equal(Colors.White, label.TextColor);
		Assert.Equal(Theme.Dark.BackgroundColor, label.BackgroundColor);
	}

	[Fact]
	public void LiveThemePropertyChangeUpdatesViews()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };

		var theme = new Theme { TextColor = Colors.Red };
		app.Theme = theme;
		Assert.Equal(Colors.Red, label.TextColor);

		// Live update: change a color on the existing theme
		theme.TextColor = Colors.Green;
		Assert.Equal(Colors.Green, label.TextColor);
	}

	[Fact]
	public void NullThemeDoesNotChangeColors()
	{
		var app = new Application();
		var label = new Label { Text = "Hello", TextColor = Colors.Red };
		app.Main = new StackLayout { label };

		// No theme set - colors should remain as-is
		Assert.Equal(Colors.Red, label.TextColor);
	}

	[Fact]
	public void SettingThemeToNullDoesNotCrash()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };
		app.Theme = Theme.Light;
		app.Theme = null;
		// Should not throw
	}

	// ── Phase 2: Developer Override Tracking ───────────────────────

	[Fact]
	public void ExplicitTextColorWins()
	{
		var app = new Application();
		var label = new Label { Text = "Always red", TextColor = Colors.Red };
		app.Main = new StackLayout { label };
		app.Theme = Theme.Light;

		// Explicit override wins over theme
		Assert.Equal(Colors.Red, label.TextColor);
	}

	[Fact]
	public void ExplicitBackgroundColorWins()
	{
		var app = new Application();
		var view = new View { BackgroundColor = Colors.Green };
		app.Main = new StackLayout { view };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Green, view.BackgroundColor);
	}

	[Fact]
	public void ExplicitStrokeColorWins()
	{
		var app = new Application();
		var border = new Border { Stroke = Colors.Red };
		app.Main = new StackLayout { border };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Red, border.Stroke);
	}

	[Fact]
	public void ExplicitButtonBackgroundColorWins()
	{
		var app = new Application();
		var button = new Button { Text = "Custom", BackgroundColor = Colors.Purple };
		app.Main = new StackLayout { button };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Purple, button.BackgroundColor);
	}

	[Fact]
	public void ExplicitActivityIndicatorColorWins()
	{
		var app = new Application();
		var ai = new ActivityIndicator { Color = Colors.Red };
		app.Main = new StackLayout { ai };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Red, ai.Color);
	}

	[Fact]
	public void NullResetsToThemeColor()
	{
		var app = new Application();
		var label = new Label { Text = "Hello", TextColor = Colors.Red };
		app.Main = new StackLayout { label };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Red, label.TextColor);

		// Null clears the explicit override
		label.TextColor = null;
		// Re-apply theme to pick up the change
		app.Theme = Theme.Light;
		Assert.Equal(Colors.Black, label.TextColor);
	}

	[Fact]
	public void ExplicitOverrideSurvivesThemeSwap()
	{
		var app = new Application();
		var label = new Label { Text = "Always red", TextColor = Colors.Red };
		app.Main = new StackLayout { label };

		app.Theme = Theme.Light;
		Assert.Equal(Colors.Red, label.TextColor);

		app.Theme = Theme.Dark;
		Assert.Equal(Colors.Red, label.TextColor);
	}

	[Fact]
	public void ExplicitSetAfterThemeIsRespected()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };
		app.Theme = Theme.Light;

		Assert.Equal(Colors.Black, label.TextColor);

		// Developer sets explicit color after theme
		label.TextColor = Colors.Orange;
		Assert.Equal(Colors.Orange, label.TextColor);

		// Swapping theme should not overwrite the explicit value
		app.Theme = Theme.Dark;
		Assert.Equal(Colors.Orange, label.TextColor);
	}

	// ── Phase 3: Dynamic View Addition ─────────────────────────────

	[Fact]
	public void NewChildrenGetThemed()
	{
		var app = new Application();
		var stack = new StackLayout();
		app.Main = stack;
		app.Theme = Theme.Light;

		// Add a label AFTER the theme was applied
		var label = new Label { Text = "Dynamic" };
		stack.Children.Add(label);

		Assert.Equal(Colors.Black, label.TextColor);
		Assert.Equal(Colors.White, label.BackgroundColor);
	}

	[Fact]
	public void NestedDynamicChildrenGetThemed()
	{
		var app = new Application();
		var stack = new StackLayout();
		app.Main = stack;
		app.Theme = Theme.Light;

		var innerStack = new StackLayout
		{
			new Label { Text = "Nested" }
		};
		stack.Children.Add(innerStack);

		var label = (Label)innerStack.Children[0];
		Assert.Equal(Colors.Black, label.TextColor);
		Assert.Equal(Colors.White, innerStack.BackgroundColor);
	}

	// ── Phase 4: System Appearance Detection ──────────────────────

	[Fact]
	public void UseSystemThemeSetsTheme()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };

		app.UseSystemTheme = true;
		// On net10.0 (VANILLA), IsDarkMode returns false, so Theme.Light is used
		Assert.NotNull(app.Theme);
		Assert.Equal(Colors.Black, label.TextColor);
	}

	[Fact]
	public void ExplicitThemeDisablesUseSystemTheme()
	{
		var app = new Application();
		app.UseSystemTheme = true;
		Assert.True(app.UseSystemTheme);

		// Setting Theme explicitly should disable UseSystemTheme
		app.Theme = Theme.Dark;
		Assert.False(app.UseSystemTheme);
	}

	[Fact]
	public void PlatformAppearanceChangedUpdatesTheme()
	{
		var app = new Application();
		var label = new Label { Text = "Hello" };
		app.Main = new StackLayout { label };
		app.UseSystemTheme = true;

		// Simulate system appearance change
		PlatformAppearance.OnChanged(true);
		Assert.Equal(Colors.White, label.TextColor);

		PlatformAppearance.OnChanged(false);
		Assert.Equal(Colors.Black, label.TextColor);

		// Cleanup: unsubscribe
		app.UseSystemTheme = false;
	}

	[Fact]
	public void AppearanceChangedCallbackInvoked()
	{
		var app = new Application();
		app.UseSystemTheme = true;
		bool? receivedDark = null;
		app.AppearanceChanged = isDark => receivedDark = isDark;

		PlatformAppearance.OnChanged(true);
		Assert.True(receivedDark);

		PlatformAppearance.OnChanged(false);
		Assert.False(receivedDark);

		// Cleanup
		app.UseSystemTheme = false;
	}

	// ── Custom Theme ──────────────────────────────────────────────

	[Fact]
	public void CustomThemeApplied()
	{
		var corporate = new Theme
		{
			TextColor = Color.FromArgb("#333333"),
			BackgroundColor = Color.FromArgb("#F5F5F5"),
			AccentColor = Color.FromArgb("#FF6600"),
			StrokeColor = Color.FromArgb("#CCCCCC"),
			PlaceholderColor = Color.FromArgb("#999999"),
		};

		var app = new Application();
		var label = new Label { Text = "Corp" };
		var button = new Button { Text = "Go" };
		var border = new Border();
		var editor = new Editor();

		app.Main = new StackLayout { label, button, border, editor };
		app.Theme = corporate;

		Assert.Equal(Color.FromArgb("#333333"), label.TextColor);
		Assert.Equal(Color.FromArgb("#FF6600"), button.BackgroundColor);
		Assert.Equal(Color.FromArgb("#CCCCCC"), border.Stroke);
		Assert.Equal(Color.FromArgb("#999999"), editor.PlaceholderColor);
	}

	// ── Full scenario from spec ────────────────────────────────────

	[Fact]
	public void FullScenarioFromSpec()
	{
		var app = new Application { Theme = Theme.Light };

		var label = new Label { Text = "Hello, Spice 🌶" };
		var counter = new Label { Text = "Times: 0" };
		var button = new Button { Text = "Tap me" };
		var overrideLabel = new Label
		{
			Text = "I'm always red",
			TextColor = Colors.Red,
		};

		app.Main = new StackLayout
		{
			label,
			counter,
			button,
			overrideLabel,
		};

		// Light theme applied
		Assert.Equal(Colors.Black, label.TextColor);
		Assert.Equal(Theme.Light.AccentColor, button.BackgroundColor);
		Assert.Equal(Colors.Red, overrideLabel.TextColor);

		// Switch to dark
		app.Theme = Theme.Dark;
		Assert.Equal(Colors.White, label.TextColor);
		Assert.Equal(Theme.Dark.AccentColor, button.BackgroundColor);
		Assert.Equal(Colors.Red, overrideLabel.TextColor); // explicit override preserved
	}
}
