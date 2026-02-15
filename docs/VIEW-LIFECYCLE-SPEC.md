# View Lifecycle & Cleanup in Spice 🌶

**Status:** Design Specification  
**Created:** February 2026

## Overview

Spice Views are created lazily and kept alive by their parent hierarchy. To support proper
cleanup (e.g., disposing event subscriptions from `Bind()`), we propose a simple contract:
**if a View implements `IDisposable`, its `Dispose()` will be called when the view is no longer needed.**

This spec defines when and how disposal happens on each platform.

## Current State

- **Android:** Views are C# partial classes wrapping `Android.Views.ViewGroup`
- **iOS:** Views are C# partial classes wrapping `UIView`
- **Lifecycle:** Managed implicitly by platform (Activity destruction on Android, ARC on iOS)
- **Disposal:** Currently no explicit cleanup contract — memory leaks possible if custom Views hold subscriptions

## Proposed: IDisposable Contract

**Key principle:** Only custom Views that hold resources implement `IDisposable` (opt-in).
Built-in Views (Button, Label, StackLayout, etc.) do **not** implement `IDisposable`.

### What Gets Disposed

1. **Custom Views that implement `IDisposable`** — `Dispose()` is called when the view is destroyed
2. **Children of a disposed view** — if a parent is disposed, it recursively disposes children that are `IDisposable`
3. **Resources:** Event subscriptions, timers, and `Bind()` subscriptions can be freed this way

### Platform Semantics

#### Android

When the `Application`'s `Activity` is destroyed (user backs out, etc.):
1. Each View's `NativeView` (underlying `ViewGroup`) is removed from the hierarchy
2. **Top-level app View** — call `Dispose()` if it implements `IDisposable`
3. **Recursively dispose children** — foreach child in `Children`, call `Dispose()` if it's `IDisposable`
4. **Bottom-up** — children disposed before parents

```csharp
// Pseudo-code in MainActivity.OnDestroy or Activity.OnDestroy equivalent
protected override void OnDestroy()
{
    if (_appView is IDisposable disposable)
        disposable.Dispose();
    base.OnDestroy();
}
```

#### iOS

When a View is removed from the hierarchy (e.g., navigation pop, cell recycle):
1. View is **no longer strongly referenced** by its parent's `Children` collection
2. **ARC (Automatic Reference Counting) deinit** is called on the Spice View wrapper
3. In the Spice View's `deinit` (C# finalizer or explicit cleanup), call `Dispose()` if it's `IDisposable`
4. **Recursively dispose children** — same as Android

Since Spice Views are managed objects in a `Children` collection, we can wrap the collection
removal to trigger cleanup:

```csharp
// In iOS View.cs
void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
{
    if (e.Action == NotifyCollectionChangedAction.Remove)
    {
        foreach (var item in e.OldItems ?? [])
        {
            if (item is View view && view is IDisposable disposable)
                disposable.Dispose();
        }
    }
    // ... also handle Replace, etc.
}
```

#### CollectionView Recycling

**Special case:** CollectionView recycles ItemTemplate views. When a template view is recycled:
1. The item stays alive (kept in `ItemsSource`)
2. The **view wrapper** is removed from the hierarchy
3. Any `Bind()` subscriptions on the item should be disposed **OR** left alive if the item is reused

**Recommendation:** For CollectionView, don't dispose item ViewModels (they're recycled).
Instead, use weak references or check `IsVisible` before updating:

```csharp
// Don't dispose the TodoItem itself — it will be recycled
// The Bind() subscription will be garbage collected when the view is recycled
```

## Implementing IDisposable in Spice Views

### Pattern 1: Single IDisposable Resource

```csharp
public partial class TimerView : StackLayout, IDisposable
{
    private System.Timers.Timer? _timer;
    private IDisposable? _binding;

    public TimerView()
    {
        var vm = new TimerViewModel();
        var label = new Label();

        // This subscription needs cleanup
        _binding = vm.Bind(nameof(vm.Elapsed), v => v.Elapsed, 
            elapsed => label.Text = $"{elapsed}s");

        // And a timer that needs stopping
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (_, _) => vm.Tick();

        Children.Add(label);
    }

    public void Dispose()
    {
        _binding?.Dispose();
        _timer?.Dispose();
    }
}
```

### Pattern 2: CompositeDisposable

```csharp
public partial class FormView : StackLayout, IDisposable
{
    private readonly List<IDisposable> _subscriptions = new();

    public FormView()
    {
        var vm = new FormViewModel();
        // ... create UI ...

        _subscriptions.Add(vm.Bind(nameof(vm.IsValid), ...));
        _subscriptions.Add(vm.Bind(nameof(vm.ErrorMessage), ...));
    }

    public void Dispose()
    {
        foreach (var sub in _subscriptions)
            sub?.Dispose();
        _subscriptions.Clear();
    }
}
```

## When to Dispose

### ✅ DO dispose:
- Custom `Bind()` subscriptions when the binding outlives the view
- Event subscriptions (timers, HTTP clients, etc.)
- File handles, database connections
- WeakReference holders that prevent collection

### ❌ DON'T dispose:
- Items in `ItemsSource` (they're reused across recycled views)
- Shared singletons (Application state, Services)
- Views that are still in the hierarchy (parent disposes them)

## Default Behavior (No Explicit Dispose)

If a View does **not** implement `IDisposable`, nothing special happens—the view is garbage
collected when no longer referenced, and any subscriptions are dropped naturally.

This is fine for simple Views. Only implement `IDisposable` if your View holds resources
that need explicit cleanup.

## Implementation Roadmap

### Phase 1: Documentation (this spec) ✓
- Establish the contract: custom Views can implement `IDisposable`
- Show patterns and examples
- Guide developers on when to implement `IDisposable`

### Phase 2: Platform Integration (future)
**Android:** Call `Dispose()` on the app's top-level View during `Activity.OnDestroy()`

```csharp
protected override void OnDestroy()
{
    if (_appView is IDisposable disposable)
        disposable.Dispose();
    base.OnDestroy();
}
```

**iOS:** Hook `Children.CollectionChanged` to dispose removed views

```csharp
// In View.cs
private void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
{
    if (e.Action == NotifyCollectionChangedAction.Remove)
    {
        foreach (var item in e.OldItems ?? [])
        {
            if (item is IDisposable disposable)
                disposable.Dispose();
        }
    }
    // ... also handle Replace, etc.
}
```

## Example: Todo App with Cleanup

```csharp
public partial class TodoItemView : StackLayout, IDisposable
{
    private IDisposable? _binding;

    public TodoItemView(TodoItem item)
    {
        var label = new Label();
        var checkbox = new CheckBox();

        // This subscription must be cleaned up when the view is recycled
        _binding = item.Bind(nameof(item.Title), t => t.Title, 
            text => label.Text = text);

        item.Bind(nameof(item.IsCompleted), t => t.IsCompleted,
            done => checkbox.IsChecked = done);

        checkbox.CheckedChanged = cb => item.IsCompleted = cb.IsChecked;

        Children.Add(new StackLayout { label, checkbox });
    }

    public void Dispose()
    {
        _binding?.Dispose();
        // Bindings to checkbox are implicitly cleaned up with the view
    }
}

public class TodoListApp : Application
{
    public TodoListApp()
    {
        var vm = new TodoListViewModel();

        var collectionView = new CollectionView
        {
            ItemsSource = vm.Items,
            ItemTemplate = item => new TodoItemView((TodoItem)item)
        };

        Main = new StackLayout { collectionView };
    }
}
```

## Notes

- **No finalizers:** Avoid finalizers in Views — they create GC pressure. Rely on explicit
  disposal or being kept alive with the view until it's cleared.
- **Parent owns children:** When a parent View is disposed, it should dispose its children.
- **Weak references:** For internal caches or event handlers, use WeakReference to avoid
  circular dependencies.

## Summary

- Only custom Views implement `IDisposable` (opt-in) — built-in Views do not
- Disposal is recursive: when a View is disposed, children that implement `IDisposable` are disposed too
- Use this contract to clean up `Bind()` subscriptions and other resources
- Platform integration will make this automatic (future)
