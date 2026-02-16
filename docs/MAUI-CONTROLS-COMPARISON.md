# .NET MAUI Controls vs Spice Implementation Status

This document compares the stable/supported controls from .NET MAUI with what is currently implemented in Spice.

## Pages

| MAUI Control | Implemented in Spice | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| ContentPage | ❌ No | ❌ No | Spice uses a different architecture without MAUI Controls |
| FlyoutPage | ❌ No | ❌ No | Spice uses a different architecture without MAUI Controls |
| NavigationPage | ❌ No | ❌ No | Spice uses a different architecture without MAUI Controls |
| TabbedPage | ❌ No | ❌ No | Spice uses a different architecture without MAUI Controls |

## Layouts

| MAUI Control | Implemented in Spice | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| AbsoluteLayout | ❌ No | ❌ No | Rare use case, complex |
| BindableLayout | ❌ No | ❌ No | Binding-focused pattern |
| FlexLayout | ❌ No | 🟡 Maybe | Powerful but complex CSS flexbox |
| Grid | ✅ Yes | ✅ Done | Essential for complex layouts |
| HorizontalStackLayout | ❌ No | ❌ No | StackLayout with Horizontal orientation |
| StackLayout | ✅ Yes | ✅ Done | Fully implemented |
| VerticalStackLayout | ❌ No | ❌ No | StackLayout with Vertical orientation |

## Views

| MAUI Control | Implemented in Spice | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| ActivityIndicator | ✅ Yes | ✅ Done | Loading spinner - very common |
| BlazorWebView | ✅ Yes | ✅ Done | Extends `WebView` in Blazor/ folders |
| Border | ✅ Yes | ✅ Done | Useful for rounded corners/borders |
| BoxView | ✅ Yes | ✅ Done | Colored rectangles - useful for dividers |
| Button | ✅ Yes | ✅ Done | Fully implemented |
| CarouselView | ❌ No | ❌ No | Complex, less common |
| CheckBox | ✅ Yes | ✅ Done | Standard checkbox input |
| CollectionView | ✅ Yes | ✅ Done | Powerful grid/list control |
| ContentView | ✅ Yes | ✅ Done | Custom control composition |
| DatePicker | ✅ Yes | ✅ Done | Date selection - common in forms |
| Editor | ✅ Yes | ✅ Done | Multi-line text input |
| Ellipse | ❌ No | 🟢 Maybe | Shape control - can use Image |
| Entry | ✅ Yes | ✅ Done | Single-line text input |
| Frame | ❌ No | ❌ No | Superseded by Border |
| GraphicsView | ❌ No | ❌ No | Advanced - Microsoft.Maui.Graphics available |
| HybridWebView | ❌ No | ❌ No | Specialized, newer control |
| Image | ✅ Yes | ✅ Done | Fully implemented |
| ImageButton | ✅ Yes | ✅ Done | Common pattern (Image + tap) |
| IndicatorView | ❌ No | ❌ No | Depends on CarouselView |
| Label | ✅ Yes | ✅ Done | Fully implemented |
| Line | ❌ No | ❌ No | Shape control - can use BoxView |
| ListView | ❌ No | 🟡 Yes | Scrollable lists - very common |
| Map | ❌ No | ❌ No | External dependency |
| Path | ❌ No | ❌ No | Complex shapes - can use Image |
| Picker | ✅ Yes | ✅ Done | Dropdown selection - essential |
| Polygon | ❌ No | ❌ No | Shape control - can use Image |
| Polyline | ❌ No | ❌ No | Shape control - can use Image |
| ProgressBar | ✅ Yes | ✅ Done | Progress display - common |
| RadioButton | ✅ Yes | ✅ Done | Single selection from a group; uses cross-platform GroupName (no Android RadioGroup) because iOS lacks a native radio button |
| Rectangle | ❌ No | 🟢 Maybe | Shape control - BoxView covers this |
| RefreshView | ✅ Yes | ✅ Done | Pull-to-refresh wrapper |
| RoundRectangle | ❌ No | ❌ No | Border can handle this |
| ScrollView | ✅ Yes | ✅ Done | Fully implemented |
| SearchBar | ✅ Yes | ✅ Done | Search input with search button |
| Slider | ✅ Yes | ✅ Done | Range selection - common |
| Stepper | ❌ No | ❌ No | Rare, can use buttons + label |
| SwipeView | ✅ Yes | ✅ Done | Swipe actions - nice UX feature |
| Switch | ✅ Yes | ✅ Done | Toggle control - essential |
| TableView | ❌ No | ❌ No | Settings-style list (less common) |
| TimePicker | ✅ Yes | ✅ Done | Time selection - common in forms |
| TitleBar | ❌ No | ❌ No | Desktop-focused |
| TwoPaneView | ❌ No | ❌ No | Foldable-specific |
| WebView | ✅ Yes | ✅ Done | Fully implemented |

## Summary

**Implemented: 26 / 60+ controls**

### Spice Controls (Core)
- ✅ ActivityIndicator
- ✅ Application
- ✅ Border
- ✅ BoxView
- ✅ Button
- ✅ CheckBox
- ✅ CollectionView
- ✅ ContentView
- ✅ DatePicker
- ✅ Editor (multi-line text)
- ✅ Entry (single-line text)
- ✅ Grid
- ✅ Image
- ✅ ImageButton
- ✅ Label
- ✅ Picker
- ✅ ProgressBar
- ✅ RadioButton
- ✅ RefreshView
- ✅ ScrollView
- ✅ SearchBar
- ✅ Slider
- ✅ StackLayout
- ✅ SwipeView
- ✅ Switch (toggle control)
- ✅ TimePicker (time selection)
- ✅ View (base class)
- ✅ WebView
- ✅ BlazorWebView (Blazor-specific)

### Supporting Types
- LayoutAlignment (enums for alignment)
- LayoutOptions (alignment with expansion)
- Orientation (horizontal/vertical)
- RootComponent (Blazor)
- SelectionMode (selection in lists)
- SwipeBehaviorOnInvoked, SwipeDirection, SwipeItem, SwipeItems, SwipeMode (swipe gesture support)

### Key Differences
- **No XAML**: Spice uses POCOs, not XAML markup
- **No Data Binding**: No `System.Reflection` or binding infrastructure
- **No MVVM**: Direct code, no ViewModels required
- **Partial Class Pattern**: Each control has cross-platform Core + iOS + Android partials
- **Minimal Dependencies**: Only uses `Microsoft.Maui.Graphics` (Color) and MAUI's SingleProject

### Platform Mappings

#### iOS (UIKit)
- ActivityIndicator → UIActivityIndicatorView
- Border → UIView (with CALayer border)
- BoxView → UIView
- Button → UIButton
- CheckBox → UIButton (with checkmark styling)
- CollectionView → UICollectionView
- ContentView → UIView
- DatePicker → UIDatePicker
- Editor → UITextView
- Entry → UITextField
- Grid → Custom constraint-based layout
- Image → UIImageView
- ImageButton → UIButton
- Label → UILabel
- Picker → UIPickerView
- ProgressBar → UIProgressView
- RadioButton → UIButton (with circle/circle.fill SF Symbols; cross-platform GroupName for exclusivity)
- RefreshView → UIView with UIRefreshControl
- ScrollView → UIScrollView
- SearchBar → UISearchBar
- Slider → UISlider
- StackLayout → UIStackView
- SwipeView → UIView with gesture recognizers
- Switch → UISwitch
- TimePicker → UIDatePicker (Mode = Time)
- WebView → WKWebView

#### Android (Android Widgets)
- ActivityIndicator → ProgressBar (indeterminate)
- Border → FrameLayout (with GradientDrawable background)
- BoxView → View
- Button → AppCompatButton
- CheckBox → CheckBox
- CollectionView → AndroidX.RecyclerView.Widget.RecyclerView
- ContentView → FrameLayout
- DatePicker → DatePickerDialog
- Editor → EditText (multiline)
- Entry → AppCompatEditText
- Grid → GridLayout
- Image → AppCompatImageView
- ImageButton → ImageButton
- Label → AppCompatTextView
- Picker → Spinner
- ProgressBar → ProgressBar
- RadioButton → Android.Widget.RadioButton (cross-platform GroupName for exclusivity, not RadioGroup)
- RefreshView → AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
- ScrollView → ScrollView / HorizontalScrollView
- SearchBar → SearchView
- Slider → SeekBar
- StackLayout → LinearLayout
- SwipeView → Custom view with gesture detection
- Switch → SwitchCompat
- TimePicker → TimePickerDialog
- WebView → WebView

---

## MAUI View/VisualElement Properties vs Spice View

This section compares the properties available on MAUI's `View` class (which inherits from `VisualElement`, `NavigableElement`, `Element`, and `BindableObject`) with Spice's `View` base class.

### Layout & Sizing Properties

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| Width | ✅ Yes | ✅ Done | Read-only - returns actual rendered width |
| Height | ✅ Yes | ✅ Done | Read-only - returns actual rendered height |
| WidthRequest | ✅ Yes | ✅ Done | Desired width - essential for sizing |
| HeightRequest | ✅ Yes | ✅ Done | Desired height - essential for sizing |
| MinimumWidthRequest | ❌ No | 🟡 Maybe | Useful for responsive layouts |
| MinimumHeightRequest | ❌ No | 🟡 Maybe | Useful for responsive layouts |
| MaximumWidthRequest | ❌ No | 🟡 Maybe | Useful for responsive layouts |
| MaximumHeightRequest | ❌ No | 🟡 Maybe | Useful for responsive layouts |
| HorizontalOptions | ✅ Yes | ✅ Done | Spice: `HorizontalOptions` (LayoutOptions) |
| VerticalOptions | ✅ Yes | ✅ Done | Spice: `VerticalOptions` (LayoutOptions) |
| Margin | ✅ Yes | ✅ Done | Outer spacing using Thickness struct |
| Bounds | ❌ No | ❌ No | Read-only - internal layout info |
| Frame | ❌ No | ❌ No | Read-only - screen position |
| DesiredSize | ❌ No | ❌ No | Read-only - layout system internal |

### Alignment Properties

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| HorizontalOptions (MAUI) | ✅ Yes | ✅ Done | Spice: `HorizontalOptions` (LayoutOptions) |
| VerticalOptions (MAUI) | ✅ Yes | ✅ Done | Spice: `VerticalOptions` (LayoutOptions) |

### Appearance Properties

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| BackgroundColor | ✅ Yes | ✅ Done | Color type |
| Background | ❌ No | ❌ No | Brush (gradients) - complex |
| Opacity | ✅ Yes | ✅ Done | 0-1 transparency - clamped range |
| IsVisible | ✅ Yes | ✅ Done | Show/hide element - very common |
| Shadow | ❌ No | ❌ No | Platform-inconsistent, use native |
| Clip | ❌ No | ❌ No | Advanced, less common |

### Transform Properties

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| Rotation | ❌ No | ❌ No | Animation-focused, adds complexity |
| RotationX | ❌ No | ❌ No | 3D transforms - rare use case |
| RotationY | ❌ No | ❌ No | 3D transforms - rare use case |
| Scale | ❌ No | ❌ No | Animation-focused |
| ScaleX | ❌ No | ❌ No | Animation-focused |
| ScaleY | ❌ No | ❌ No | Animation-focused |
| TranslationX | ❌ No | ❌ No | Animation-focused |
| TranslationY | ❌ No | ❌ No | Animation-focused |
| AnchorX | ❌ No | ❌ No | Transform origin - depends on transforms |
| AnchorY | ❌ No | ❌ No | Transform origin - depends on transforms |

### Interaction Properties

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| IsEnabled | ✅ Yes | ✅ Done | Enable/disable interaction - essential for forms |
| InputTransparent | ❌ No | 🟡 Maybe | Pass-through touch events - useful |
| IsFocused | ❌ No | ❌ No | Read-only focus state - advanced |
| GestureRecognizers | ❌ No | ❌ No | Add tap handlers directly to controls |

### Hierarchy & Navigation

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| Children (collection) | ✅ Yes | ✅ Done | `ObservableCollection<View>` |
| Parent | ❌ No | 🟡 Maybe | Parent element - useful for traversal |
| Navigation | ❌ No | ❌ No | MAUI page-based navigation |
| Id | ❌ No | ❌ No | Unique identifier - less useful |

### Styling & Resources

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| Style | ❌ No | ❌ No | XAML-focused pattern |
| StyleClass | ❌ No | ❌ No | CSS-like classes - binding-focused |
| Class | ❌ No | ❌ No | Style classes - binding-focused |
| ClassId | ❌ No | ❌ No | Semantic identifier - testing-focused |
| StyleId | ❌ No | ❌ No | User identifier - debugging-focused |
| Resources | ❌ No | ❌ No | XAML resource dictionary |
| Behaviors | ❌ No | ❌ No | XAML behavior system |
| Triggers | ❌ No | ❌ No | XAML property triggers |
| Effects | ❌ No | ❌ No | Platform effects - advanced |

### Data Binding & Context

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| BindingContext | ❌ No | ❌ No | Data binding context (not Spice's philosophy) |

### Platform & Accessibility

| MAUI Property | Spice Implementation | Should Implement? | Notes |
|--------------|---------------------|-------------------|-------|
| AutomationId | ✅ Yes | ✅ Done | UI testing identifier - useful for QA |
| Handler | ❌ No | ❌ No | Platform handler - internal |
| FlowDirection | ❌ No | 🟢 Maybe | RTL support - i18n feature |
| IsLoaded | ❌ No | ❌ No | Loaded state - internal lifecycle |
| Dispatcher | ❌ No | ❌ No | UI thread dispatcher - internal |

### Spice-Specific Properties

| Spice Property | MAUI Equivalent | Notes |
|---------------|-----------------|-------|
| Children | Yes (in Container types) | `ObservableCollection<View>`, supports collection initializers |
| HorizontalOptions | HorizontalOptions | Uses `LayoutOptions` with alignment and expansion flags |
| VerticalOptions | VerticalOptions | Uses `LayoutOptions` with alignment and expansion flags |
| BackgroundColor | BackgroundColor | Uses `Microsoft.Maui.Graphics.Color` |
| IsVisible | IsVisible | Show/hide element |
| IsEnabled | IsEnabled | Enable/disable interaction |
| Opacity | Opacity | 0-1 transparency, clamped range |
| AutomationId | AutomationId | UI testing identifier |
| Margin | Margin | Outer spacing using Thickness struct |
| WidthRequest | WidthRequest | Desired width for sizing |
| HeightRequest | HeightRequest | Desired height for sizing |
| Width | Width | Read-only actual width |
| Height | Height | Read-only actual height |

### Summary

**Spice View Properties: 14**
- Children (collection)
- HorizontalOptions (LayoutOptions)
- VerticalOptions (LayoutOptions)
- BackgroundColor
- IsVisible
- IsEnabled
- Opacity
- AutomationId
- Margin
- WidthRequest
- HeightRequest
- Width (read-only)
- Height (read-only)

**MAUI View/VisualElement Properties: 60+**

Spice's `View` class is intentionally minimal, focusing on the essential properties needed for basic layout and appearance. MAUI's extensive property set supports:
- Complex styling and theming (not in Spice)
- Data binding and MVVM (not in Spice)
- Advanced transforms and animations (not in Spice)
- Accessibility and testing infrastructure (not in Spice)
- Resource management and behaviors (not in Spice)

Spice uses `[ObservableProperty]` for property change notifications, generating `On{Prop}Changed` partial methods implemented in platform-specific files, rather than MAUI's `BindableProperty` system.

---

## Recommended Additions for Spice

Based on Spice's minimalist philosophy and common mobile UI needs, here are reasonable additions that would enhance functionality without compromising simplicity:

### 🔥 High Priority - Essential Controls

**Layouts**
- ✅ **Grid** - Essential for complex layouts; maps to UIStackView/LinearLayout with weights or constraint-based layout (IMPLEMENTED)
- ✅ **ScrollView** - Fundamental for scrollable content; maps to UIScrollView/ScrollView (IMPLEMENTED)

**Input Controls**
- ✅ **Switch** - Standard toggle control; maps to UISwitch/SwitchCompat (IMPLEMENTED)
- ✅ **Slider** - Common for settings/media controls; maps to UISlider/SeekBar (IMPLEMENTED)
- ✅ **Picker** - Standard dropdown/selection; maps to UIPickerView/Spinner (IMPLEMENTED)
- ✅ **DatePicker** - Date selection; maps to UIDatePicker/DatePickerDialog (IMPLEMENTED)
- ✅ **TimePicker** - Time selection; maps to UIDatePicker/TimePickerDialog (IMPLEMENTED)
- ✅ **CheckBox** - Boolean selection; maps to UIButton (checkmark)/CheckBox (IMPLEMENTED)

**Display Controls**
- ✅ **ActivityIndicator** - Loading spinner; maps to UIActivityIndicatorView/ProgressBar (indeterminate) (IMPLEMENTED)
- ✅ **ProgressBar** - Progress display; maps to UIProgressView/ProgressBar (determinate) (IMPLEMENTED)

### 🟡 Medium Priority - Very Useful

**Layouts**
- ✅ **ContentView** - Custom control container for composition (IMPLEMENTED)
- ✅ **Border** - Wraps content with border/rounded corners; common UI pattern (IMPLEMENTED)

**Lists**
- ✅ **CollectionView** - Flexible grid/list; maps to UICollectionView/RecyclerView (IMPLEMENTED)
- 🟡 **ListView** - Scrollable list of items; maps to UITableView/RecyclerView (critical for many apps)

**Input**
- ✅ **Editor** - Multi-line text input; maps to UITextView/EditText (multiline) (IMPLEMENTED)
- ✅ **SearchBar** - Search input; maps to UISearchBar/SearchView (IMPLEMENTED)

**Display**
- ✅ **ImageButton** - Tappable image; common pattern (can be done with Image + gesture) (IMPLEMENTED)

### 🟢 Nice to Have - Special Cases

**Advanced Controls**
- ✅ **RefreshView** - Pull-to-refresh wrapper (IMPLEMENTED)
- ✅ **SwipeView** - Swipe actions/context menus (IMPLEMENTED)
- ✅ **RadioButton** - Radio button groups; uses cross-platform GroupName since iOS has no native radio concept

**Shapes** (Lower priority - can use Image or GraphicsView)
- ✅ **BoxView** - Colored rectangle (useful for dividers/spacers) (IMPLEMENTED)
- 🟢 **Rectangle/Ellipse** - Basic shapes

### 📊 View Properties - High Priority

**Layout & Sizing**
- ✅ **WidthRequest/HeightRequest** - Essential for sizing views (IMPLEMENTED)
- ✅ **Margin** - Outer spacing (critical for layouts) (IMPLEMENTED)
- 🟡 **Padding** - Inner spacing (for containers)

**Appearance**
- ✅ **IsVisible** - Show/hide elements (very common) (IMPLEMENTED)
- ✅ **Opacity** - Transparency (common for fade effects) (IMPLEMENTED)

**Interaction**
- ✅ **IsEnabled** - Enable/disable controls (essential for forms) (IMPLEMENTED)

### ❌ Not Recommended

**Probably Skip**
- ❌ **Transforms** (Rotation, Scale, Translation) - Animation-focused, adds complexity
- ❌ **CarouselView** - Complex, less common
- ❌ **IndicatorView** - Depends on CarouselView
- ❌ **TabbedPage/NavigationPage** - Page-level navigation (different architecture)
- ❌ **Map** - External dependency (Microsoft.Maui.Controls.Maps)
- ❌ **GraphicsView** - Advanced graphics (Microsoft.Maui.Graphics already available)
- ❌ **HybridWebView** - Specialized, newer control
- ❌ **TitleBar** - Desktop-focused
- ❌ **TwoPaneView** - Foldable-specific
- ❌ **Shapes** (Path, Polygon, Polyline, Line) - Can use Image or custom drawing
- ❌ **Shadow** - Platform-inconsistent, can use native code
- ❌ **Clip** - Advanced, less common
- ❌ **GestureRecognizers** - Can add tap handlers directly to controls
- ❌ **Behaviors/Triggers/Effects** - XAML/binding-focused patterns
- ❌ **Stepper** - Rare, can use buttons + label

### Implementation Priority

**Phase 1 (Core Controls)** ✅
1. ✅ Grid layout
2. ✅ ScrollView
3. ✅ Switch
4. ✅ ActivityIndicator
5. ✅ ProgressBar
6. ✅ IsVisible property
7. ✅ IsEnabled property
8. ✅ WidthRequest/HeightRequest
9. ✅ Margin

**Phase 2 (Input Controls)** ✅
1. ✅ Picker
2. ✅ Slider
3. ✅ CheckBox
4. ✅ DatePicker
5. ✅ TimePicker
6. ✅ Editor (multiline text)

**Phase 3 (Lists & Advanced)** ✅
1. 🟡 ListView
2. ✅ SearchBar
3. ✅ CollectionView
4. ✅ Border
5. ✅ ContentView
6. ✅ ImageButton

**Phase 4 (Nice-to-Have)** ✅
1. ✅ RefreshView
2. ✅ SwipeView
3. ✅ BoxView
4. ✅ RadioButton
5. ✅ Opacity property

---

*Note: Spice is focused on minimal cross-platform UI with no Microsoft.Maui.Controls dependency. Controls are added based on common mobile scenarios rather than full MAUI parity.*
