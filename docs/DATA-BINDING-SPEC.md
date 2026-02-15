# Data-Binding in Spice 🌶

**Status:** Design Specification  
**Created:** February 2026

## Overview

Spice views already extend `ObservableObject` (via CommunityToolkit.Mvvm), so every `View`,
`Label`, `CheckBox`, etc. implements `INotifyPropertyChanged` out of the box. This spec
proposes a small `Bind()` helper that wires `PropertyChanged` subscriptions with less
boilerplate — while staying NativeAOT safe, trimmer safe, and reflection-free.

## What We Have Today

Direct lambda assignments — explicit, transparent, zero magic:

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

For larger UIs, a ViewModel with manual `PropertyChanged` wiring works but gets verbose:

```csharp
var vm = new TodoViewModel();
var titleLabel = new Label();
var countLabel = new Label();
var checkbox = new CheckBox();

vm.PropertyChanged += (s, e) =>
{
    if (e.PropertyName == nameof(TodoViewModel.Title))
        titleLabel.Text = vm.Title;
    else if (e.PropertyName == nameof(TodoViewModel.Count))
        countLabel.Text = $"Count: {vm.Count}";
    else if (e.PropertyName == nameof(TodoViewModel.IsCompleted))
        checkbox.IsChecked = vm.IsCompleted;
};

// Reverse: view → viewmodel (Action<CheckBox> property, not an event)
checkbox.CheckedChanged = cb => vm.IsCompleted = cb.IsChecked;

Main = new StackLayout { titleLabel, countLabel, checkbox };
```

## Proposed: `Bind()` Helper

### Core API

One method, no expression trees, no reflection:

```csharp
namespace Spice;

public static class BindingExtensions
{
    /// <summary>
    /// One-way binding: subscribes to source.PropertyChanged and invokes
    /// <paramref name="apply"/> whenever the named property changes.
    /// Sets the initial value immediately.
    /// </summary>
    public static IDisposable Bind<TSource, TValue>(
        this TSource source,
        string propertyName,
        Func<TSource, TValue> getter,
        Action<TValue> apply)
        where TSource : INotifyPropertyChanged
    {
        // Sync initial value
        apply(getter(source));

        PropertyChangedEventHandler handler = (_, e) =>
        {
            if (e.PropertyName == propertyName)
                apply(getter(source));
        };
        source.PropertyChanged += handler;

        return new BindingSubscription(source, handler);
    }
}

internal sealed class BindingSubscription : IDisposable
{
    private readonly INotifyPropertyChanged _source;
    private readonly PropertyChangedEventHandler _handler;
    private bool _disposed;

    public BindingSubscription(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
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

Usage with `nameof()` keeps everything compile-time safe:

```csharp
vm.Bind(nameof(vm.Title), v => v.Title, text => titleLabel.Text = text);
vm.Bind(nameof(vm.Count), v => v.Count, n => countLabel.Text = $"Count: {n}");
```

### Why Not `Expression<Func<TSource, TValue>>`?

An expression-tree overload like `vm.Bind(x => x.Title, ...)` is ergonomically nicer, but
`Expression.Compile()` internally uses `Reflection.Emit` (or an interpreter on .NET 8+).
For a library that promises NativeAOT safety, introducing expression trees is a real trade-off:

- ✅ Works on .NET 8+ NativeAOT via the built-in interpreter
- ⚠️ Slower than a plain `Func<>` (interpreter overhead + one-time compile cost)
- ⚠️ Adds `System.Linq.Expressions` as a de facto dependency
- ❌ Fails on older NativeAOT runtimes without the interpreter

If the convenience is deemed worth it, an **optional** overload can be added alongside
the `nameof` version — never as the only option:

```csharp
// Optional convenience overload (expression tree)
public static IDisposable Bind<TSource, TValue>(
    this TSource source,
    Expression<Func<TSource, TValue>> propertySelector,
    Action<TValue> apply)
    where TSource : INotifyPropertyChanged
{
    string name = ((MemberExpression)propertySelector.Body).Member.Name;
    var getter = propertySelector.Compile();
    return source.Bind(name, getter, apply);
}
```

### Two-Way Binding

Since Spice views **already implement `INotifyPropertyChanged`**, true two-way binding is
straightforward — subscribe in both directions with a guard to prevent infinite loops:

```csharp
public static IDisposable BindTwoWay<TSource, TValue>(
    this TSource source,
    string sourceProperty,
    Func<TSource, TValue> sourceGetter,
    Action<TSource, TValue> sourceSetter,
    INotifyPropertyChanged target,
    string targetProperty,
    Func<TValue> targetGetter,
    Action<TValue> targetSetter)
    where TSource : INotifyPropertyChanged
{
    bool updating = false;

    targetSetter(sourceGetter(source)); // initial sync

    PropertyChangedEventHandler sourceHandler = (_, e) =>
    {
        if (!updating && e.PropertyName == sourceProperty)
        {
            updating = true;
            targetSetter(sourceGetter(source));
            updating = false;
        }
    };

    PropertyChangedEventHandler targetHandler = (_, e) =>
    {
        if (!updating && e.PropertyName == targetProperty)
        {
            updating = true;
            sourceSetter(source, targetGetter());
            updating = false;
        }
    };

    source.PropertyChanged += sourceHandler;
    target.PropertyChanged += targetHandler;

    return new TwoWayBindingSubscription(source, sourceHandler, target, targetHandler);
}
```

Usage — ViewModel ↔ Entry (both are ObservableObjects):

```csharp
vm.BindTwoWay(
    nameof(vm.Username), v => v.Username, (v, val) => v.Username = val,
    usernameEntry,
    nameof(Entry.Text), () => usernameEntry.Text, val => usernameEntry.Text = val);
```

This is verbose, which is intentional — it makes data flow explicit. For common cases,
convenience overloads or a fluent builder could be added later.

## Usage Examples

### Simple One-Way

```csharp
public class CounterApp : Application
{
    public CounterApp()
    {
        var vm = new CounterViewModel();
        var label = new Label();
        var button = new Button
        {
            Text = "Increment",
            Clicked = _ => vm.Count++
        };

        vm.Bind(nameof(vm.Count), v => v.Count, n => label.Text = $"Count: {n}");

        Main = new StackLayout { label, button };
    }
}
```

### CollectionView with Per-Item Binding

For CollectionView recycling, wrap the template in a custom `IDisposable` View:

```csharp
public class TodoItemView : StackLayout, IDisposable
{
    private IDisposable? _titleBinding;

    public TodoItemView(TodoItem item)
    {
        var label = new Label();
        var checkbox = new CheckBox();

        // One-way: model → view
        _titleBinding = item.Bind(nameof(item.Title), t => t.Title, 
            text => label.Text = text);
        item.Bind(nameof(item.IsCompleted), t => t.IsCompleted,
            done => checkbox.IsChecked = done);

        // Reverse: view → model
        checkbox.CheckedChanged = cb => item.IsCompleted = cb.IsChecked;

        Children.Add(new StackLayout { label, checkbox });
    }

    public void Dispose()
    {
        _titleBinding?.Dispose();
    }
}

var collectionView = new CollectionView
{
    ItemsSource = vm.Items,
    ItemTemplate = item => new TodoItemView((TodoItem)item)
};
```

When the view is recycled, `Dispose()` will be called and subscriptions cleaned up.
See [VIEW-LIFECYCLE-SPEC.md](./VIEW-LIFECYCLE-SPEC.md) for details.

## Memory Management

`Bind()` returns `IDisposable`. In most Spice apps, bindings live as long as the view, so
disposal is unnecessary. For short-lived views bound to long-lived sources, dispose explicitly:

```csharp
var binding = vm.Bind(nameof(vm.Title), v => v.Title, text => label.Text = text);
// ...
binding.Dispose(); // unsubscribes from PropertyChanged
```

Multiple bindings can be grouped with `CompositeDisposable` or a simple list:

```csharp
var bindings = new List<IDisposable>
{
    vm.Bind(nameof(vm.Title), v => v.Title, t => label.Text = t),
    vm.Bind(nameof(vm.Count), v => v.Count, n => countLabel.Text = $"{n}"),
};

// Dispose all at once
foreach (var b in bindings) b.Dispose();
```

## Alternatives Considered

| Approach | Pros | Cons |
|---|---|---|
| **Manual `PropertyChanged` (status quo)** | Zero abstraction, fully explicit | Verbose, error-prone string matching |
| **`nameof` + `Func<>` (this proposal)** | NativeAOT safe, no new dependencies | Property name repeated in `nameof()` call |
| **`Expression<Func<>>` only** | Ergonomic (`x => x.Title`) | `Expression.Compile()` has NativeAOT caveats |
| **Source generator** | Zero runtime cost, best ergonomics | Significant implementation effort, new tooling |
| **CallerArgumentExpression hack** | No expression trees | Fragile string parsing, breaks with refactoring |

The `nameof` + `Func<>` approach is the right default for Spice: zero new dependencies,
truly NativeAOT safe, and the `nameof()` repetition is a minor cost for full transparency.

## Future Work

1. **View lifecycle** — Add `Detached` event to `View` for CollectionView recycling cleanup
2. **Source generator** — Compile-time codegen to eliminate `nameof()` boilerplate
3. **Value converters** — `Bind(..., converter: boolToColor)` for transformations

## Summary

- Spice views already implement `INotifyPropertyChanged` — leverage it
- `Bind()` is a thin helper over `PropertyChanged`, not a framework
- `nameof()` + `Func<>` = truly NativeAOT safe, no expression trees required
- Two-way binding works because both sides are `ObservableObject`
- Expression-tree overload is opt-in, not the default
