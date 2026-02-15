# Data-Binding in Spice 🌶

**Status:** Specification / Design Document  
**Created:** February 2026  
**Author:** Spice Team

## Executive Summary

This document explores how data-binding could be implemented in Spice while maintaining its core philosophy: **NativeAOT safe, trimmer safe, strongly-typed, no reflection, and minimal**.

### Key Findings

After researching existing solutions in the .NET ecosystem, we have identified three viable approaches:

1. **Status Quo (Recommended)**: Continue with lambda-based property updates using existing `[ObservableProperty]` from CommunityToolkit.Mvvm
2. **ReactiveUI Integration**: Adopt ReactiveUI's source generators for expression-based reactive bindings
3. **Custom Source Generator**: Build a minimal Spice-specific binding generator

## Background

### Current State

Spice explicitly avoids traditional data-binding as stated in the README:
> "No XAML. No DI. No MVVM. No MVC. No data-binding. No System.Reflection."

The current pattern uses **direct lambda assignments**:

```csharp
public class App : Application
{
    public App()
    {
        int count = 0;
        var label = new Label { Text = "Hello, Spice 🌶" };
        var button = new Button 
        { 
            Clicked = _ => label.Text = $"Times: {++count}" 
        };
        Main = new StackLayout { label, button };
    }
}
```

This approach is:
- ✅ **NativeAOT safe** - no reflection
- ✅ **Trimmer safe** - no hidden dependencies
- ✅ **Strongly-typed** - compile-time checked
- ✅ **Testable** - POCOs work in plain `net10.0` projects

### Why Consider Data-Binding?

While the current approach works well for simple scenarios, data-binding could provide benefits for:

1. **Complex UIs** - Automatically synchronizing multiple views with shared data
2. **List scenarios** - Binding collections to CollectionView
3. **Form validation** - Coordinated property updates with validation logic
4. **Separation of concerns** - Cleaner code when UI logic becomes complex

## Requirements

Any data-binding solution for Spice **must meet these requirements**:

| Requirement | Description | Why It Matters |
|-------------|-------------|----------------|
| **NativeAOT Safe** | Must work with Native AOT compilation | Future-proofing for when NativeAOT comes to mobile |
| **Trimmer Safe** | Must not break with aggressive trimming | Keeps app size minimal |
| **Strongly-Typed** | Compile-time type checking | Catches errors early, better IDE support |
| **No Reflection** | No `System.Reflection` usage at runtime | Required for AOT and trimming |
| **Lambda Support** | Ideally uses lambdas for short expressions | Maintains Spice's minimal, code-first philosophy |
| **Source Generator** | Likely needs to be source-generator based | Only way to achieve above requirements |

## Solution Analysis

### Option 1: Status Quo (Lambda-Based) ⭐ RECOMMENDED

**Continue with the current approach**, enhanced with better patterns and documentation.

#### Current Usage

Spice already uses **CommunityToolkit.Mvvm** (v8.4.0) for `[ObservableProperty]`:

```csharp
// In Label.cs
public partial class Label : View
{
    [ObservableProperty]
    string _text = "";
    
    [ObservableProperty]
    Color? _textColor;
}
```

This generates:
- Public `Text` and `TextColor` properties
- `INotifyPropertyChanged` implementation
- `partial void OnTextChanged(string value)` methods for platform code

#### Enhanced Pattern: Observable ViewModels

For complex UIs, users can create their own observable models:

```csharp
public partial class TodoViewModel : ObservableObject
{
    [ObservableProperty]
    string _title = "";
    
    [ObservableProperty]
    bool _isCompleted;
    
    [ObservableProperty]
    int _count;
    
    partial void OnTitleChanged(string value)
    {
        // Validation logic
    }
}

public class TodoApp : Application
{
    public TodoApp()
    {
        var vm = new TodoViewModel();
        
        var titleLabel = new Label();
        var countLabel = new Label();
        var checkbox = new CheckBox();
        
        // Manual binding through lambdas
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(TodoViewModel.Title))
                titleLabel.Text = vm.Title;
            else if (e.PropertyName == nameof(TodoViewModel.Count))
                countLabel.Text = $"Count: {vm.Count}";
            else if (e.PropertyName == nameof(TodoViewModel.IsCompleted))
                checkbox.IsChecked = vm.IsCompleted;
        };
        
        checkbox.CheckedChanged += (s, isChecked) => 
            vm.IsCompleted = isChecked;
        
        Main = new StackLayout { titleLabel, countLabel, checkbox };
    }
}
```

#### Pros
- ✅ **Zero additional dependencies** - uses existing CommunityToolkit.Mvvm
- ✅ **Maximum transparency** - developers see exactly what's happening
- ✅ **Fully AOT/trimmer safe** - no hidden magic
- ✅ **Easy to test** - straightforward unit tests
- ✅ **Stays true to Spice philosophy** - minimal, explicit, no framework magic
- ✅ **Flexible** - can bind anything to anything

#### Cons
- ❌ **Verbose for complex UIs** - lots of manual lambda code
- ❌ **No compile-time safety for property names** - uses strings in PropertyChanged
- ❌ **No automatic two-way binding** - must wire up both directions manually
- ❌ **Boilerplate for collections** - CollectionView binding requires manual list management

#### Improvement Ideas

1. **Add helper extension methods** for common binding patterns:

```csharp
public static class BindingExtensions
{
    /// <summary>
    /// One-way binding from source property to target property
    /// </summary>
    public static void Bind<TSource, TTarget>(
        this TSource source,
        Expression<Func<TSource, object?>> sourceProperty,
        TTarget target,
        Action<TTarget, object?> targetSetter)
        where TSource : INotifyPropertyChanged
    {
        // Implementation uses expression tree to extract property name
        // But only at setup time, not at runtime
    }
}

// Usage:
vm.Bind(x => x.Title, titleLabel, (lbl, value) => lbl.Text = (string)value);
```

2. **Document common patterns** in samples and docs
3. **Add CollectionView helpers** for observable collections

---

### Option 2: ReactiveUI Integration

**Adopt ReactiveUI** as an optional add-on package for users who need reactive bindings.

#### Overview

[ReactiveUI](https://www.reactiveui.net/) is a mature, battle-tested MVVM framework with:
- ✅ Source generator support (ReactiveUI.SourceGenerators)
- ✅ NativeAOT and trimmer safe
- ✅ Strongly-typed bindings using expressions
- ✅ Works on iOS, Android, .NET MAUI, Uno, etc.
- ✅ Active development (v19+ as of 2024)

#### Example Usage

```csharp
using ReactiveUI;

public partial class TodoViewModel : ReactiveObject
{
    [Reactive] public string Title { get; set; } = "";
    [Reactive] public bool IsCompleted { get; set; }
    [ReactiveCommand] public void Save() { /* ... */ }
}

public class TodoApp : Application
{
    public TodoApp()
    {
        var vm = new TodoViewModel();
        
        var titleLabel = new Label();
        var checkbox = new CheckBox();
        
        // ReactiveUI binding
        vm.WhenAnyValue(x => x.Title)
          .Subscribe(text => titleLabel.Text = text);
        
        vm.WhenAnyValue(x => x.IsCompleted)
          .Subscribe(isChecked => checkbox.IsChecked = isChecked);
        
        Main = new StackLayout { titleLabel, checkbox };
    }
}
```

#### Architecture

```
┌─────────────────────────────────────┐
│  Spice.ReactiveUI (optional)        │
│  - Extension methods for Spice      │
│  - Helper classes                   │
└─────────────────────────────────────┘
              ↓ depends on
┌─────────────────────────────────────┐
│  ReactiveUI + SourceGenerators      │
│  - [Reactive] attribute             │
│  - WhenAnyValue() expressions       │
└─────────────────────────────────────┘
              ↓ used by
┌─────────────────────────────────────┐
│  User App                           │
│  - Optional ReactiveUI patterns     │
└─────────────────────────────────────┘
```

#### Pros
- ✅ **Mature and proven** - used in production apps for years
- ✅ **Strongly-typed** - uses Expression<T> for compile-time safety
- ✅ **Powerful reactive patterns** - operators like Throttle, Debounce, CombineLatest
- ✅ **AOT/trimmer safe** - source generator approach
- ✅ **Great for complex scenarios** - async, validation, cross-property dependencies
- ✅ **Active community** - lots of documentation and samples

#### Cons
- ❌ **Large dependency** - adds significant weight to app size (~500KB+)
- ❌ **Learning curve** - reactive programming concepts are not intuitive for everyone
- ❌ **Framework commitment** - pulls in a whole ecosystem
- ❌ **May conflict with Spice philosophy** - "magical" binding might not fit
- ❌ **Overkill for simple apps** - most Spice apps don't need this power

#### Integration Approach

If we choose this option:

1. **Create separate package**: `Spice.ReactiveUI`
2. **Make it optional**: Users opt-in with `dotnet add package Spice.ReactiveUI`
3. **Provide Spice-specific extensions**:

```csharp
namespace Spice.ReactiveUI;

public static class SpiceReactiveExtensions
{
    /// <summary>
    /// Bind observable to Label.Text
    /// </summary>
    public static IDisposable BindTo<T>(
        this IObservable<T> source, 
        Label label,
        Func<T, string>? converter = null)
    {
        return source.Subscribe(value => 
            label.Text = converter?.Invoke(value) ?? value?.ToString() ?? "");
    }
    
    // Similar helpers for other Spice controls
}

// Usage becomes cleaner:
vm.WhenAnyValue(x => x.Title).BindTo(titleLabel);
vm.WhenAnyValue(x => x.Count).BindTo(countLabel, c => $"Count: {c}");
```

4. **Document clearly** when to use it vs. vanilla Spice

---

### Option 3: Custom Spice Source Generator

**Build a minimal, Spice-specific source generator** for data-binding.

#### Design Goals

- Minimal API surface
- Generate only what's needed
- Stay true to Spice philosophy
- No reflection at runtime

#### Proposed API

```csharp
// User code
public partial class TodoViewModel : ObservableObject
{
    [ObservableProperty]
    string _title = "";
}

public class TodoApp : Application
{
    public TodoApp()
    {
        var vm = new TodoViewModel();
        var titleLabel = new Label();
        
        // Spice binding syntax - generates code at compile time
        titleLabel.Bind(vm, x => x.Title, (label, value) => label.Text = value);
        
        // Two-way binding
        var entry = new Entry();
        entry.BindTwoWay(vm, x => x.Title);
        
        Main = new StackLayout { titleLabel, entry };
    }
}
```

#### Generated Code

The source generator would produce something like:

```csharp
// Generated by Spice.Binding.SourceGenerator
public static class GeneratedBindings
{
    public static IDisposable Bind_TodoViewModel_Title_To_Label(
        TodoViewModel source,
        Label target,
        Action<Label, string> setter)
    {
        // Initial set
        setter(target, source.Title);
        
        // Subscribe to changes
        PropertyChangedEventHandler handler = (s, e) =>
        {
            if (e.PropertyName == nameof(TodoViewModel.Title))
                setter(target, source.Title);
        };
        
        source.PropertyChanged += handler;
        
        return new ActionDisposable(() => 
            source.PropertyChanged -= handler);
    }
}

// Extension method calls generated code
public static IDisposable Bind<T>(
    this Label target,
    TodoViewModel source,
    Expression<Func<TodoViewModel, string>> propertyExpression,
    Action<Label, string> setter)
{
    return GeneratedBindings.Bind_TodoViewModel_Title_To_Label(
        source, target, setter);
}
```

#### Pros
- ✅ **Tailored for Spice** - exactly what we need, nothing more
- ✅ **Full control** - can optimize for Spice patterns
- ✅ **Strongly-typed** - compile-time checking via expressions
- ✅ **AOT/trimmer safe** - generated code is transparent
- ✅ **Lightweight** - no extra dependencies beyond CommunityToolkit.Mvvm

#### Cons
- ❌ **Development cost** - significant engineering effort to build and maintain
- ❌ **Testing burden** - need comprehensive test suite for generator
- ❌ **Documentation** - must write all docs ourselves
- ❌ **Community** - no existing ecosystem or samples
- ❌ **Edge cases** - will discover problems over time
- ❌ **Maintenance** - must keep up with C# language changes

#### Implementation Considerations

If we choose this path:

1. **Phase 1: Basic bindings**
   - One-way property bindings
   - Simple type conversions
   
2. **Phase 2: Two-way bindings**
   - Bidirectional property sync
   - Entry, CheckBox, etc. controls
   
3. **Phase 3: Collection bindings**
   - ObservableCollection support for CollectionView
   - Item templates
   
4. **Phase 4: Advanced scenarios**
   - Multi-property bindings
   - Value converters
   - Validation

---

## Comparison Matrix

| Feature | Status Quo | ReactiveUI | Custom Generator |
|---------|-----------|------------|------------------|
| **NativeAOT Safe** | ✅ | ✅ | ✅ |
| **Trimmer Safe** | ✅ | ✅ | ✅ |
| **Strongly-Typed** | ⚠️ Partial | ✅ | ✅ |
| **No Reflection** | ✅ | ✅ | ✅ |
| **App Size Impact** | ✅ None | ❌ ~500KB+ | ✅ Minimal |
| **Development Cost** | ✅ Zero | ✅ Integration only | ❌ High |
| **Learning Curve** | ✅ Low | ❌ High | ⚠️ Medium |
| **Maintenance** | ✅ Minimal | ✅ Community | ❌ On us |
| **Feature Richness** | ❌ Basic | ✅ Very rich | ⚠️ As built |
| **Fits Spice Philosophy** | ✅ Perfect | ⚠️ Questionable | ✅ Good |
| **Time to Ship** | ✅ Immediate | ✅ Quick | ❌ Months |

## Recommendations

### Primary Recommendation: Status Quo + Documentation

**Keep the current lambda-based approach** as the primary pattern, with improvements:

1. ✅ **Document best practices** - show patterns for common scenarios
2. ✅ **Add helper methods** - reduce boilerplate with extension methods  
3. ✅ **Provide samples** - demonstrate ViewModel patterns with `[ObservableProperty]`
4. ✅ **Improve CollectionView** - make list binding easier

**Why?**
- Stays true to Spice's minimal philosophy
- Zero additional dependencies or app size impact
- Fully transparent and debuggable
- Easy to understand and teach
- Already works perfectly

### Secondary Recommendation: Optional ReactiveUI Package

For users with complex requirements, **offer ReactiveUI integration as an opt-in package**:

```
dotnet add package Spice.ReactiveUI
```

This provides:
- Advanced binding for power users
- Reactive patterns for complex async scenarios
- Mature, proven solution
- Doesn't affect users who don't need it

**Implementation:**
- Create `Spice.ReactiveUI` NuGet package
- Provide Spice-specific extension methods
- Document when to use it vs. vanilla Spice
- Include samples showing both approaches

### Not Recommended: Custom Generator

**Do not build a custom source generator** unless:
- ReactiveUI proves inadequate
- We have dedicated resources for 6+ months
- There's clear evidence users need it

**Why not?**
- High development and maintenance cost
- ReactiveUI already exists and works
- Diverts resources from core Spice features
- Risk of creating a half-baked solution

## API Examples

### Example 1: Simple Counter (Status Quo)

```csharp
public class CounterApp : Application
{
    public CounterApp()
    {
        int count = 0;
        var label = new Label { Text = "Count: 0" };
        var button = new Button 
        { 
            Text = "Increment",
            Clicked = _ => label.Text = $"Count: {++count}"
        };
        Main = new StackLayout { label, button };
    }
}
```

**Status:** Works perfectly today. No binding needed.

### Example 2: Form with ViewModel (Status Quo Enhanced)

```csharp
public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    string _username = "";
    
    [ObservableProperty]
    string _password = "";
    
    [ObservableProperty]
    bool _canLogin;
    
    partial void OnUsernameChanged(string value) => UpdateCanLogin();
    partial void OnPasswordChanged(string value) => UpdateCanLogin();
    
    void UpdateCanLogin() => 
        CanLogin = !string.IsNullOrEmpty(Username) && 
                   !string.IsNullOrEmpty(Password);
}

public class LoginApp : Application
{
    public LoginApp()
    {
        var vm = new LoginViewModel();
        
        var usernameEntry = new Entry { Placeholder = "Username" };
        var passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };
        var loginButton = new Button { Text = "Login" };
        
        // Manual binding
        usernameEntry.TextChanged += (_, text) => vm.Username = text;
        passwordEntry.TextChanged += (_, text) => vm.Password = text;
        
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(LoginViewModel.CanLogin))
                loginButton.IsEnabled = vm.CanLogin;
        };
        
        loginButton.Clicked += _ => 
        {
            // Perform login
        };
        
        Main = new StackLayout 
        { 
            usernameEntry, 
            passwordEntry, 
            loginButton 
        };
    }
}
```

**Status:** Works with today's approach. Could be cleaner with binding helpers.

### Example 3: Todo List with CollectionView

```csharp
public partial class TodoItem : ObservableObject
{
    [ObservableProperty]
    string _title = "";
    
    [ObservableProperty]
    bool _isCompleted;
}

public partial class TodoListViewModel : ObservableObject
{
    public ObservableCollection<TodoItem> Items { get; } = new();
    
    public void AddItem(string title) => 
        Items.Add(new TodoItem { Title = title });
}

public class TodoListApp : Application
{
    public TodoListApp()
    {
        var vm = new TodoListViewModel();
        
        var newItemEntry = new Entry { Placeholder = "New todo..." };
        var addButton = new Button { Text = "Add" };
        
        var collectionView = new CollectionView
        {
            ItemsSource = vm.Items,
            ItemTemplate = item =>
            {
                var todoItem = (TodoItem)item;
                var label = new Label { Text = todoItem.Title };
                var checkbox = new CheckBox { IsChecked = todoItem.IsCompleted };
                
                // Update view when model changes
                todoItem.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(TodoItem.Title))
                        label.Text = todoItem.Title;
                    else if (e.PropertyName == nameof(TodoItem.IsCompleted))
                        checkbox.IsChecked = todoItem.IsCompleted;
                };
                
                // Update model when view changes
                checkbox.CheckedChanged += (_, isChecked) => 
                    todoItem.IsCompleted = isChecked;
                
                return new StackLayout { label, checkbox };
            }
        };
        
        addButton.Clicked += _ =>
        {
            vm.AddItem(newItemEntry.Text);
            newItemEntry.Text = "";
        };
        
        Main = new StackLayout 
        { 
            new StackLayout { newItemEntry, addButton },
            collectionView 
        };
    }
}
```

**Status:** Works but verbose. CollectionView already handles ObservableCollection changes. Item template binding could be cleaner.

## Migration Path

If users need data-binding:

### Path 1: Stay with Status Quo
```
Current App → Document patterns → Add helper methods → Done
```

### Path 2: Adopt ReactiveUI
```
Current App → Add Spice.ReactiveUI package → 
Refactor complex UIs → Keep simple UIs as-is
```

### Path 3: Future Custom Generator
```
Current App → Wait for Spice.Binding → Gradual migration → 
Use [[Bindable]] attributes
```

## App Size Impact

| Approach | Additional Size | Notes |
|----------|----------------|-------|
| Status Quo | 0 bytes | CommunityToolkit.Mvvm already included |
| ReactiveUI | ~500KB+ | Includes System.Reactive, Rx operators |
| Custom Generator | ~10-50KB | Only generated binding code |

For reference, a basic Spice app is **~8MB** vs. MAUI's **~15MB**.

## Performance Considerations

| Approach | Startup Time | Memory | CPU |
|----------|-------------|--------|-----|
| Status Quo | ✅ Best | ✅ Minimal | ✅ Only when properties change |
| ReactiveUI | ⚠️ Slower | ⚠️ More overhead | ⚠️ Observable streams |
| Custom Generator | ✅ Good | ✅ Minimal | ✅ Only when properties change |

## Testing Implications

All approaches remain fully testable:

```csharp
[Fact]
public void LoginViewModel_ValidCredentials_EnablesLogin()
{
    var vm = new LoginViewModel
    {
        Username = "test",
        Password = "password"
    };
    
    Assert.True(vm.CanLogin);
}
```

Spice views are POCOs in `net10.0` context, so testing is straightforward regardless of binding approach.

## Future Considerations

### C# Language Features

Future C# versions may provide features that make binding easier:
- **Interceptors** - could enable compile-time binding rewriting
- **Source generators v2** - more powerful code generation
- **Discriminated unions** - better patterns for state

### .NET MAUI Lessons

What we can learn from .NET MAUI's binding:
- ❌ String-based bindings are error-prone
- ❌ Compiled bindings are complex to implement
- ✅ `{x:Bind}` in XAML shows value of compile-time checking
- ✅ Source generators are the future

### Mobile NativeAOT

When NativeAOT comes to mobile:
- Our status quo will already work perfectly
- ReactiveUI will work if they maintain AOT support
- Custom generator would need no changes

## Conclusion

**Ship the spec, improve documentation, stay minimal.**

Spice's current approach is actually **ahead of the curve**:
- No reflection = NativeAOT ready today
- Lambda-based = explicit and debuggable  
- POCO views = easily testable
- Source-generated properties = best of both worlds

For the 20% of users who need more power, **offer ReactiveUI as optional integration**. For the 80% of users, **keep Spice beautifully simple**.

## References

### External Resources

1. **CommunityToolkit.Mvvm**
   - GitHub: https://github.com/CommunityToolkit/dotnet
   - Docs: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/
   - Current version in Spice: 8.4.0

2. **ReactiveUI**
   - Website: https://www.reactiveui.net/
   - GitHub: https://github.com/reactiveui/ReactiveUI
   - Source Generators: https://github.com/reactiveui/ReactiveUI.SourceGenerators

3. **NativeAOT Documentation**
   - Overview: https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/
   - Libraries: https://devblogs.microsoft.com/dotnet/creating-aot-compatible-libraries/
   - Optimization: https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/optimizing

4. **Configuration Binding Source Generator** (similar patterns)
   - Docs: https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-generator
   - Shows how .NET uses source generators for AOT-safe binding

### Internal References

- Spice README: `/README.md`
- CommunityToolkit.Mvvm usage: `src/Spice/Core/*.cs`
- Current architecture: `src/Spice/Platforms/*/`

## Appendix: Implementation Details

### A. Binding Helper Extensions (If Status Quo)

```csharp
namespace Spice.Binding;

/// <summary>
/// Optional helper methods for property binding patterns.
/// Source generator analyzes these at compile-time for optimization.
/// </summary>
public static class BindingExtensions
{
    /// <summary>
    /// Bind a source property to update a target action
    /// </summary>
    public static IDisposable BindProperty<TSource, TProperty>(
        this TSource source,
        string propertyName,
        Action<TProperty> onChanged)
        where TSource : INotifyPropertyChanged
    {
        PropertyChangedEventHandler handler = (s, e) =>
        {
            if (e.PropertyName == propertyName)
            {
                var value = (TProperty)typeof(TSource)
                    .GetProperty(propertyName)
                    ?.GetValue(source)!;
                onChanged(value);
            }
        };
        
        source.PropertyChanged += handler;
        
        return new ActionDisposable(() => 
            source.PropertyChanged -= handler);
    }
}

internal class ActionDisposable : IDisposable
{
    private readonly Action _action;
    public ActionDisposable(Action action) => _action = action;
    public void Dispose() => _action();
}
```

### B. Spice.ReactiveUI Extension Examples

```csharp
namespace Spice.ReactiveUI;

public static class SpiceReactiveExtensions
{
    public static IDisposable BindTo(
        this IObservable<string> source, 
        Label label)
    {
        return source.Subscribe(text => label.Text = text);
    }
    
    public static IDisposable BindTo(
        this IObservable<bool> source, 
        CheckBox checkbox)
    {
        return source.Subscribe(isChecked => 
            checkbox.IsChecked = isChecked);
    }
    
    public static IDisposable BindTo<T>(
        this IObservable<T> source,
        Entry entry,
        Func<T, string> converter)
    {
        return source.Subscribe(value => 
            entry.Text = converter(value));
    }
}
```

### C. Custom Generator Pseudo-code

```csharp
// Analyzer finds patterns like:
label.Bind(vm, x => x.Title, (l, v) => l.Text = v);

// Generates:
#region Generated by Spice.Binding.SourceGenerator
public static class GeneratedBindings_TodoViewModel
{
    public static IDisposable Bind_Title(
        TodoViewModel source,
        Action<string> setter)
    {
        // Initial value
        setter(source.Title);
        
        // Subscribe
        PropertyChangedEventHandler handler = (s, e) =>
        {
            if (e.PropertyName == "Title")
                setter(source.Title);
        };
        
        source.PropertyChanged += handler;
        
        return new BindingDisposable(() => 
            source.PropertyChanged -= handler);
    }
}
#endregion
```

---

**END OF SPECIFICATION**
