# .NET MAUI Controls vs Spice Implementation Status

This document compares the stable/supported controls from .NET MAUI with what is currently implemented in Spice.

## Pages

| MAUI Control | Implemented in Spice | Notes |
|--------------|---------------------|-------|
| ContentPage | ❌ No | Spice uses a different architecture without MAUI Controls |
| FlyoutPage | ❌ No | Spice uses a different architecture without MAUI Controls |
| NavigationPage | ❌ No | Spice uses a different architecture without MAUI Controls |
| TabbedPage | ❌ No | Spice uses a different architecture without MAUI Controls |

## Layouts

| MAUI Control | Implemented in Spice | Notes |
|--------------|---------------------|-------|
| AbsoluteLayout | ❌ No | |
| BindableLayout | ❌ No | |
| FlexLayout | ❌ No | |
| Grid | ❌ No | |
| HorizontalStackLayout | ❌ No | |
| StackLayout | ✅ Yes | Implemented as `StackView` |
| VerticalStackLayout | ❌ No | |

## Views

| MAUI Control | Implemented in Spice | Notes |
|--------------|---------------------|-------|
| ActivityIndicator | ❌ No | |
| BlazorWebView | ✅ Yes | Extends `WebView` in Blazor/ folders |
| Border | ❌ No | |
| BoxView | ❌ No | |
| Button | ✅ Yes | Fully implemented |
| CarouselView | ❌ No | |
| CheckBox | ❌ No | |
| CollectionView | ❌ No | |
| ContentView | ❌ No | |
| DatePicker | ❌ No | |
| Editor | ❌ No | Multi-line text input |
| Ellipse | ❌ No | Shape control |
| Entry | ✅ Yes | Single-line text input |
| Frame | ❌ No | |
| GraphicsView | ❌ No | |
| HybridWebView | ❌ No | |
| Image | ✅ Yes | Fully implemented |
| ImageButton | ❌ No | |
| IndicatorView | ❌ No | |
| Label | ✅ Yes | Fully implemented |
| Line | ❌ No | Shape control |
| ListView | ❌ No | |
| Map | ❌ No | Requires Microsoft.Maui.Controls.Maps |
| Path | ❌ No | Shape control |
| Picker | ❌ No | |
| Polygon | ❌ No | Shape control |
| Polyline | ❌ No | Shape control |
| ProgressBar | ❌ No | |
| RadioButton | ❌ No | |
| Rectangle | ❌ No | Shape control |
| RefreshView | ❌ No | |
| RoundRectangle | ❌ No | Shape control |
| ScrollView | ❌ No | |
| SearchBar | ❌ No | |
| Slider | ❌ No | |
| Stepper | ❌ No | |
| SwipeView | ❌ No | |
| Switch | ❌ No | |
| TableView | ❌ No | |
| TimePicker | ❌ No | |
| TitleBar | ❌ No | |
| TwoPaneView | ❌ No | |
| WebView | ✅ Yes | Fully implemented |

## Summary

**Implemented: 6 / 60+ controls**

### Spice Controls (Core)
- ✅ Application
- ✅ Button
- ✅ Entry (single-line text)
- ✅ Image
- ✅ Label
- ✅ StackView (equivalent to StackLayout)
- ✅ View (base class)
- ✅ WebView
- ✅ BlazorWebView (Blazor-specific)

### Supporting Types
- Align (enums for alignment)
- Orientation (horizontal/vertical)
- RootComponent (Blazor)

### Key Differences
- **No XAML**: Spice uses POCOs, not XAML markup
- **No Data Binding**: No `System.Reflection` or binding infrastructure
- **No MVVM**: Direct code, no ViewModels required
- **Partial Class Pattern**: Each control has cross-platform Core + iOS + Android partials
- **Minimal Dependencies**: Only uses `Microsoft.Maui.Graphics` (Color) and MAUI's SingleProject

### Platform Mappings

#### iOS (UIKit)
- Button → UIButton
- Entry → UITextField
- Image → UIImageView
- Label → UILabel
- StackView → UIStackView
- WebView → WKWebView

#### Android (Android Widgets)
- Button → AppCompatButton
- Entry → AppCompatEditText
- Image → AppCompatImageView
- Label → AppCompatTextView
- StackView → LinearLayout
- WebView → WebView

---

*Note: Spice is focused on minimal cross-platform UI with no Microsoft.Maui.Controls dependency. Controls are added based on common mobile scenarios rather than full MAUI parity.*
