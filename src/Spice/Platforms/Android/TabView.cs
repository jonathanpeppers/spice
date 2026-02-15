using Android.Content;
using Android.Views;
using System.Collections.Specialized;
using Google.Android.Material.BottomNavigation;

namespace Spice;

public partial class TabView
{
	/// <summary>
	/// TabView ctor - creates a container with BottomNavigationView
	/// </summary>
	public TabView() : this(Platform.Context) { }

	/// <summary>
	/// TabView ctor
	/// </summary>
	/// <param name="context">The Android context</param>
	public TabView(Context context) : base(context, ctx => new AndroidX.AppCompat.Widget.LinearLayoutCompat(ctx)
	{
		Orientation = (int)Android.Widget.Orientation.Vertical
	})
	{
		Children.CollectionChanged += OnTabsChanged;
	}

	BottomNavigationView? _bottomNavigationView;
	Android.Widget.FrameLayout? _contentFrame;
	int _selectedTabIndex = 0;

	/// <summary>
	/// Gets or creates the bottom navigation view
	/// </summary>
	BottomNavigationView BottomNavigationView
	{
		get
		{
			if (_bottomNavigationView == null)
			{
				_bottomNavigationView = new BottomNavigationView(Platform.Context)
				{
					LayoutParameters = new Android.Widget.LinearLayout.LayoutParams(
						ViewGroup.LayoutParams.MatchParent,
						ViewGroup.LayoutParams.WrapContent)
				};
				
				_bottomNavigationView.ItemSelected += OnNavigationItemSelected;
				
				((AndroidX.AppCompat.Widget.LinearLayoutCompat)NativeView).AddView(_bottomNavigationView);
			}
			return _bottomNavigationView;
		}
	}

	/// <summary>
	/// Gets or creates the content frame
	/// </summary>
	Android.Widget.FrameLayout ContentFrame
	{
		get
		{
			if (_contentFrame == null)
			{
				_contentFrame = new Android.Widget.FrameLayout(Platform.Context)
				{
					LayoutParameters = new Android.Widget.LinearLayout.LayoutParams(
						ViewGroup.LayoutParams.MatchParent,
						0, // height
						1.0f) // weight to fill remaining space
				};
				// Insert before bottom navigation
				((AndroidX.AppCompat.Widget.LinearLayoutCompat)NativeView).AddView(_contentFrame, 0);
			}
			return _contentFrame;
		}
	}

	void OnTabsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateTabs();
	}

	void UpdateTabs()
	{
		var menu = BottomNavigationView.Menu;
		menu.Clear();

		int index = 0;
		foreach (var child in Children)
		{
			if (child is Tab tab)
			{
				var menuItem = menu.Add(0, index, index, tab.Title);
				
				// Set icon if available
				if (!string.IsNullOrEmpty(tab.Icon))
				{
					// Try to load icon from resources
					var context = Platform.Context;
					if (context != null)
					{
						var resourceId = context.Resources?.GetIdentifier(tab.Icon, "drawable", context.PackageName);
						if (resourceId.HasValue && resourceId.Value != 0)
						{
							menuItem?.SetIcon(resourceId.Value);
						}
					}
				}
				
				index++;
			}
		}

		// Show first tab by default
		if (Children.Count > 0)
		{
			ShowTab(0);
		}
	}

	void OnNavigationItemSelected(object? sender, Google.Android.Material.Navigation.NavigationBarView.ItemSelectedEventArgs e)
	{
		var index = e.Item.ItemId;
		ShowTab(index);
	}

	void ShowTab(int index)
	{
		if (index < 0 || index >= Children.Count)
			return;

		_selectedTabIndex = index;

		var tab = Children[index] as Tab;
		if (tab != null)
		{
			tab.EnsureContent();
			
			ContentFrame.RemoveAllViews();
			
			if (tab.Content != null)
			{
				ContentFrame.AddView(tab.Content);
			}
		}
	}
}

public partial class Tab
{
	/// <summary>
	/// Tab ctor - tabs are logical containers
	/// </summary>
	public Tab() : this(Platform.Context) { }

	/// <summary>
	/// Tab ctor
	/// </summary>
	/// <param name="context">The Android context</param>
	public Tab(Context context) : base(context, ctx => new Android.Widget.FrameLayout(ctx))
	{
	}
}
