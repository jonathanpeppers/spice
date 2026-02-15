# Data-Binding in Spice 🌶

**Status:** Design Specification  
**Created:** February 2026

## Overview

This document describes Spice's approach to data-binding that maintains its core philosophy: **NativeAOT safe, trimmer safe, strongly-typed, no reflection, and minimal**.

## Current Approach

Spice uses **direct lambda assignments** for property updates:

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

This is explicit, transparent, and works perfectly for simple apps.

## Using ViewModels with [ObservableProperty]

For complex UIs, create ViewModels using `[ObservableProperty]` from **CommunityToolkit.Mvvm**:

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

While this works, it's verbose for complex UIs. We can do better.

## Proposed Binding Helper

Add a strongly-typed `Bind<TSource, TTarget>()` extension method:

```csharp
namespace Spice;

public static class BindingExtensions
{
    /// <summary>
    /// One-way binding from source property to target action.
    /// Extracts property name at setup time (no runtime reflection).
    /// </summary>
    public static IDisposable Bind<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, TValue>> propertySelector,
        Action<TValue> onChanged)
        where TSource : INotifyPropertyChanged
    {
        // Extract property name from expression at setup time
        string propertyName = GetPropertyName(propertySelector);
        
        // Get the compiled accessor
        var accessor = propertySelector.Compile();
        
        // Set initial value
        onChanged(accessor(source));
        
        // Subscribe to changes
        PropertyChangedEventHandler handler = (s, e) =>
        {
            if (e.PropertyName == propertyName)
                onChanged(accessor(source));
        };
        
        source.PropertyChanged += handler;
        
        // Return disposable for cleanup
        return new BindingSubscription(source, handler);
    }
    
    /// <summary>
    /// Two-way binding between source property and target property setter/getter.
    /// </summary>
    public static IDisposable BindTwoWay<TSource, TTarget, TValue>(
        this TSource source,
        Expression<Func<TSource, TValue>> sourceProperty,
        TTarget target,
        Func<TTarget, TValue> targetGetter,
        Action<TTarget, TValue> targetSetter)
        where TSource : INotifyPropertyChanged
        where TTarget : INotifyPropertyChanged
    {
        string sourcePropertyName = GetPropertyName(sourceProperty);
        var sourceAccessor = sourceProperty.Compile();
        
        // Initial sync: source -> target
        targetSetter(target, sourceAccessor(source));
        
        // Source changes -> update target
        PropertyChangedEventHandler sourceHandler = (s, e) =>
        {
            if (e.PropertyName == sourcePropertyName)
                targetSetter(target, sourceAccessor(source));
        };
        source.PropertyChanged += sourceHandler;
        
        // Target changes -> update source (requires reflection or magic)
        // For now, this requires manual wiring in user code
        
        return new BindingSubscription(source, sourceHandler);
    }
    
    private static string GetPropertyName<TSource, TValue>(
        Expression<Func<TSource, TValue>> propertySelector)
    {
        if (propertySelector.Body is MemberExpression memberExpr)
        {
            return memberExpr.Member.Name;
        }
        else if (propertySelector.Body is UnaryExpression unaryExpr 
                 && unaryExpr.Operand is MemberExpression memberExpr2)
        {
            return memberExpr2.Member.Name;
        }
        
        throw new ArgumentException(
            "Expression must be a simple property access: x => x.Property",
            nameof(propertySelector));
    }
}

internal sealed class BindingSubscription : IDisposable
{
    private readonly INotifyPropertyChanged _source;
    private readonly PropertyChangedEventHandler _handler;
    private bool _disposed;
    
    public BindingSubscription(
        INotifyPropertyChanged source,
        PropertyChangedEventHandler handler)
    {
        _source = source;
        _handler = handler;
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            _source.PropertyChanged -= _handler;
            _disposed = true;
        }
    }
}
```

## Usage Examples

### Simple Binding

```csharp
public class TodoApp : Application
{
    public TodoApp()
    {
        var vm = new TodoViewModel();
        var titleLabel = new Label();
        
        // One-way binding: ViewModel -> Label
        vm.Bind(x => x.Title, text => titleLabel.Text = text);
        
        Main = titleLabel;
    }
}
```

### Multiple Bindings

```csharp
public class TodoApp : Application
{
    public TodoApp()
    {
        var vm = new TodoViewModel();
        
        var titleLabel = new Label();
        var countLabel = new Label();
        var completedCheckbox = new CheckBox();
        
        // Bind multiple properties
        vm.Bind(x => x.Title, text => titleLabel.Text = text);
        vm.Bind(x => x.Count, count => countLabel.Text = $"Count: {count}");
        vm.Bind(x => x.IsCompleted, completed => completedCheckbox.IsChecked = completed);
        
        // Manual reverse binding (view -> viewmodel)
        completedCheckbox.CheckedChanged += (_, isChecked) => 
            vm.IsCompleted = isChecked;
        
        Main = new StackLayout 
        { 
            titleLabel, 
            countLabel, 
            completedCheckbox 
        };
    }
}
```

### Form Example with Validation

```csharp
public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    string _username = "";
    
    [ObservableProperty]
    string _password = "";
    
    [ObservableProperty]
    bool _canLogin;
    
    [ObservableProperty]
    string _errorMessage = "";
    
    partial void OnUsernameChanged(string value) => UpdateCanLogin();
    partial void OnPasswordChanged(string value) => UpdateCanLogin();
    
    void UpdateCanLogin()
    {
        if (string.IsNullOrEmpty(Username))
        {
            ErrorMessage = "Username required";
            CanLogin = false;
        }
        else if (string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Password required";
            CanLogin = false;
        }
        else
        {
            ErrorMessage = "";
            CanLogin = true;
        }
    }
}

public class LoginApp : Application
{
    public LoginApp()
    {
        var vm = new LoginViewModel();
        
        var usernameEntry = new Entry { Placeholder = "Username" };
        var passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };
        var errorLabel = new Label { TextColor = Colors.Red };
        var loginButton = new Button { Text = "Login" };
        
        // Bind view model to UI
        vm.Bind(x => x.CanLogin, canLogin => loginButton.IsEnabled = canLogin);
        vm.Bind(x => x.ErrorMessage, message => errorLabel.Text = message);
        
        // Initialize button state
        loginButton.IsEnabled = vm.CanLogin;
        
        // Update view model from UI
        usernameEntry.TextChanged += (_, text) => vm.Username = text;
        passwordEntry.TextChanged += (_, text) => vm.Password = text;
        
        loginButton.Clicked += _ =>
        {
            // Perform login with vm.Username and vm.Password
        };
        
        Main = new StackLayout 
        { 
            usernameEntry, 
            passwordEntry, 
            errorLabel,
            loginButton 
        };
    }
}
```

### CollectionView Example

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
                var label = new Label();
                var checkbox = new CheckBox();
                
                var layout = new StackLayout { label, checkbox };
                
                // Bind item properties to views
                var titleBinding = todoItem.Bind(x => x.Title, text => label.Text = text);
                var completedBinding = todoItem.Bind(x => x.IsCompleted, 
                    completed => checkbox.IsChecked = completed);
                
                // Cleanup subscriptions when item is recycled
                // Note: Spice would need to support a Disposed/Detached event on View
                // For now, this is a potential memory leak that needs addressing
                
                // Update model from view
                checkbox.CheckedChanged += (_, isChecked) => 
                    todoItem.IsCompleted = isChecked;
                
                return layout;
            }
        };
        
        addButton.Clicked += _ =>
        {
            if (!string.IsNullOrEmpty(newItemEntry.Text))
            {
                vm.AddItem(newItemEntry.Text);
                newItemEntry.Text = "";
            }
        };
        
        Main = new StackLayout 
        { 
            new StackLayout { newItemEntry, addButton },
            collectionView 
        };
    }
}
```

## Implementation Notes

### Expression Tree Analysis

The `Bind<TSource, TValue>()` method uses **expression trees** to extract property names:

```csharp
vm.Bind(x => x.Title, text => titleLabel.Text = text);
       ~~~~~~~~~~~~  <- Expression tree analyzed at setup time
```

The expression `x => x.Title` is compiled to a delegate for fast property access, and the property name "Title" is extracted for `PropertyChanged` filtering. This happens **once at setup**, not on every property change.

### No Runtime Reflection

The implementation uses:
- ✅ **Expression trees** - analyzed at setup time only
- ✅ **Compiled delegates** - for fast property access
- ✅ **String comparison** - for PropertyChanged event filtering
- ❌ **No reflection** - at runtime

This makes it fully compatible with NativeAOT and IL trimming.

### Memory Management

The `Bind()` method returns an `IDisposable` for cleanup:

```csharp
var binding = vm.Bind(x => x.Title, text => label.Text = text);

// Later, when done:
binding.Dispose(); // Unsubscribes from PropertyChanged
```

For most apps, bindings live for the lifetime of the page/view, so explicit disposal isn't necessary. But for long-lived ViewModels with short-lived views, proper disposal prevents memory leaks.

### Two-Way Binding Limitation

True two-way binding (where target changes automatically update source) is difficult without reflection or source generators. For now, the recommendation is:

```csharp
// Forward: ViewModel -> View
vm.Bind(x => x.Text, text => entry.Text = text);

// Reverse: View -> ViewModel (manual)
entry.TextChanged += (_, text) => vm.Text = text;
```

A future source generator could automate the reverse direction for Spice controls.

## Benefits

This approach provides:

1. **Strongly-typed** - `vm.Bind(x => x.Title, ...)` catches typos at compile-time
2. **Concise** - Much shorter than manual PropertyChanged handlers
3. **NativeAOT safe** - No runtime reflection
4. **Trimmer safe** - All code is statically analyzable
5. **Transparent** - Simple implementation, easy to debug
6. **Minimal** - Single extension method, no framework dependency
7. **Compatible** - Works with existing `[ObservableProperty]` pattern

## Testability

ViewModels remain fully testable as POCOs:

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
    Assert.Empty(vm.ErrorMessage);
}

[Fact]
public void LoginViewModel_EmptyUsername_ShowsError()
{
    var vm = new LoginViewModel
    {
        Username = "",
        Password = "password"
    };
    
    Assert.False(vm.CanLogin);
    Assert.Equal("Username required", vm.ErrorMessage);
}
```

## Future Enhancements

Potential improvements:

1. **Source Generator** - Auto-generate optimized bindings at compile-time
2. **View Lifecycle** - Add `Disposed` event to `View` for proper cleanup in CollectionView
3. **Two-Way Binding** - Generate reverse bindings for Spice controls
4. **Binding Converters** - Support for value transformation (e.g., bool to Visibility)
5. **MultiBinding** - Combine multiple properties into one expression

## Summary

Spice's data-binding approach:

- **Today**: Direct lambda assignments, `[ObservableProperty]` ViewModels
- **Proposed**: Add `Bind<TSource, TValue>()` extension method
- **Benefits**: Strongly-typed, concise, NativeAOT safe
- **Philosophy**: Explicit, minimal, no framework magic

The `Bind()` helper reduces boilerplate while maintaining Spice's core values of transparency and simplicity.
