namespace Spice.Tests;

/// <summary>
/// Unit tests validating the core behavior and defaults of the View class.
/// </summary>
public class ViewTests
{
[Fact]
public void ViewCanBeCreated()
{
var view = new View();
Assert.NotNull(view);
}

[Fact]
public void ChildrenCollectionIsInitialized()
{
var view = new View();
Assert.NotNull(view.Children);
Assert.Empty(view.Children);
}

[Fact]
public void HorizontalOptionsDefaultsToStart()
{
var view = new View();
Assert.Equal(default(LayoutOptions), view.HorizontalOptions);
}

[Fact]
public void VerticalOptionsDefaultsToStart()
{
var view = new View();
Assert.Equal(default(LayoutOptions), view.VerticalOptions);
}

[Fact]
public void BackgroundColorDefaultsToNull()
{
var view = new View();
Assert.Null(view.BackgroundColor);
}

[Fact]
public void IsVisibleDefaultsToTrue()
{
var view = new View();
Assert.True(view.IsVisible);
}

[Fact]
public void IsEnabledDefaultsToTrue()
{
var view = new View();
Assert.True(view.IsEnabled);
}

[Fact]
public void OpacityDefaultsToOne()
{
var view = new View();
Assert.Equal(1.0, view.Opacity);
}

[Fact]
public void MarginDefaultsToZero()
{
var view = new View();
Assert.Equal(0, view.Margin.Left);
Assert.Equal(0, view.Margin.Top);
Assert.Equal(0, view.Margin.Right);
Assert.Equal(0, view.Margin.Bottom);
Assert.True(view.Margin.IsEmpty);
}

[Fact]
public void WidthRequestDefaultsToNegativeOne()
{
var view = new View();
Assert.Equal(-1, view.WidthRequest);
}

[Fact]
public void HeightRequestDefaultsToNegativeOne()
{
var view = new View();
Assert.Equal(-1, view.HeightRequest);
}

[Fact]
public void CanSetHorizontalOptions()
{
var view = new View { HorizontalOptions = LayoutOptions.Start };
Assert.Equal(LayoutOptions.Start, view.HorizontalOptions);
}

[Fact]
public void CanSetVerticalOptions()
{
var view = new View { VerticalOptions = LayoutOptions.End };
Assert.Equal(LayoutOptions.End, view.VerticalOptions);
}

[Fact]
public void CanSetBackgroundColor()
{
var color = Colors.Blue;
var view = new View { BackgroundColor = color };
Assert.Equal(color, view.BackgroundColor);
}

[Fact]
public void CanSetIsVisible()
{
var view = new View { IsVisible = false };
Assert.False(view.IsVisible);
}

[Fact]
public void CanSetIsEnabled()
{
var view = new View { IsEnabled = false };
Assert.False(view.IsEnabled);
}

[Fact]
public void CanSetOpacity()
{
var view = new View { Opacity = 0.5 };
Assert.Equal(0.5, view.Opacity);
}

[Fact]
public void CanSetMargin()
{
var view = new View { Margin = new Thickness(10, 20, 30, 40) };
Assert.Equal(10, view.Margin.Left);
Assert.Equal(20, view.Margin.Top);
Assert.Equal(30, view.Margin.Right);
Assert.Equal(40, view.Margin.Bottom);
}

[Fact]
public void CanSetWidthRequest()
{
var view = new View { WidthRequest = 100 };
Assert.Equal(100, view.WidthRequest);
}

[Fact]
public void CanSetHeightRequest()
{
var view = new View { HeightRequest = 200 };
Assert.Equal(200, view.HeightRequest);
}

[Fact]
public void PropertyChangedFiresOnHorizontalOptionsChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.HorizontalOptions))
propertyChangedFired = true;
};

view.HorizontalOptions = LayoutOptions.Fill;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnVerticalOptionsChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.VerticalOptions))
propertyChangedFired = true;
};

view.VerticalOptions = LayoutOptions.Fill;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnBackgroundColorChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.BackgroundColor))
propertyChangedFired = true;
};

view.BackgroundColor = Colors.Red;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnIsVisibleChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.IsVisible))
propertyChangedFired = true;
};

view.IsVisible = false;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnIsEnabledChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.IsEnabled))
propertyChangedFired = true;
};

view.IsEnabled = false;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnOpacityChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.Opacity))
propertyChangedFired = true;
};

view.Opacity = 0.5;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnMarginChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.Margin))
propertyChangedFired = true;
};

view.Margin = new Thickness(10);
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnWidthRequestChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.WidthRequest))
propertyChangedFired = true;
};

view.WidthRequest = 150;
Assert.True(propertyChangedFired);
}

[Fact]
public void PropertyChangedFiresOnHeightRequestChange()
{
var view = new View();
var propertyChangedFired = false;
view.PropertyChanged += (s, e) =>
{
if (e.PropertyName == nameof(View.HeightRequest))
propertyChangedFired = true;
};

view.HeightRequest = 250;
Assert.True(propertyChangedFired);
}

[Fact]
public void CanAddChildToView()
{
var parent = new View();
var child = new View();

parent.Children.Add(child);

Assert.Single(parent.Children);
Assert.Same(child, parent.Children[0]);
}

[Fact]
public void CanAddMultipleChildren()
{
var parent = new View();
var child1 = new View();
var child2 = new View();
var child3 = new View();

parent.Children.Add(child1);
parent.Children.Add(child2);
parent.Children.Add(child3);

Assert.Equal(3, parent.Children.Count);
Assert.Same(child1, parent.Children[0]);
Assert.Same(child2, parent.Children[1]);
Assert.Same(child3, parent.Children[2]);
}

[Fact]
public void CanRemoveChild()
{
var parent = new View();
var child = new View();

parent.Children.Add(child);
Assert.Single(parent.Children);

parent.Children.Remove(child);
Assert.Empty(parent.Children);
}

[Fact]
public void AddMethodWorksForCollectionInitializer()
{
var parent = new View();
var child = new View();

parent.Add(child);

Assert.Single(parent.Children);
Assert.Same(child, parent.Children[0]);
}

[Fact]
public void ViewIsEnumerable()
{
var parent = new View();
var child1 = new View();
var child2 = new View();

parent.Children.Add(child1);
parent.Children.Add(child2);

var children = new List<View>();
foreach (var child in parent)
{
children.Add(child);
}

Assert.Equal(2, children.Count);
Assert.Same(child1, children[0]);
Assert.Same(child2, children[1]);
}

[Fact]
public void CanSetAllPropertiesAtOnce()
{
var view = new View
{
HorizontalOptions = LayoutOptions.Start,
VerticalOptions = LayoutOptions.End,
BackgroundColor = Colors.Green,
IsVisible = false,
IsEnabled = false,
Opacity = 0.5,
Margin = new Thickness(5),
WidthRequest = 300,
HeightRequest = 400
};

Assert.Equal(LayoutOptions.Start, view.HorizontalOptions);
Assert.Equal(LayoutOptions.End, view.VerticalOptions);
Assert.Equal(Colors.Green, view.BackgroundColor);
Assert.False(view.IsVisible);
Assert.False(view.IsEnabled);
Assert.Equal(0.5, view.Opacity);
Assert.Equal(5, view.Margin.Left);
Assert.Equal(300, view.WidthRequest);
Assert.Equal(400, view.HeightRequest);
}
}
