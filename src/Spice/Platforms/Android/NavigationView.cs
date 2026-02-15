using Android.Content;
using Android.Views;
using AndroidX.AppCompat.Widget;

namespace Spice;

public partial class NavigationView
{
	/// <summary>
	/// NavigationView ctor - creates a container for navigation
	/// </summary>
	public NavigationView() : this(Platform.Context) { }

	/// <summary>
	/// NavigationView ctor
	/// </summary>
	/// <param name="context">The Android context</param>
	public NavigationView(Context context) : base(context, ctx => new AndroidX.AppCompat.Widget.LinearLayoutCompat(ctx)
	{
		Orientation = (int)Android.Widget.Orientation.Vertical
	})
	{
	}

	AndroidX.AppCompat.Widget.Toolbar? _toolbar;
	Android.Widget.FrameLayout? _contentFrame;
	readonly Stack<View> _backStack = new();

	/// <summary>
	/// Gets or creates the toolbar for navigation
	/// </summary>
	AndroidX.AppCompat.Widget.Toolbar Toolbar
	{
		get
		{
			if (_toolbar == null)
			{
				_toolbar = new AndroidX.AppCompat.Widget.Toolbar(Platform.Context)
				{
					LayoutParameters = new Android.Widget.LinearLayout.LayoutParams(
						ViewGroup.LayoutParams.MatchParent,
						ViewGroup.LayoutParams.WrapContent)
				};
				((AndroidX.AppCompat.Widget.LinearLayoutCompat)NativeView).AddView(_toolbar);

				// Setup back button
				_toolbar.NavigationClick += (s, e) => Pop();
			}
			return _toolbar;
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
						ViewGroup.LayoutParams.MatchParent)
				};
				((AndroidX.AppCompat.Widget.LinearLayoutCompat)NativeView).AddView(_contentFrame);
			}
			return _contentFrame;
		}
	}

	partial void PushCore(View view)
	{
		// Remove current view
		if (_backStack.Count > 0)
		{
			ContentFrame.RemoveAllViews();
		}

		// Add new view
		_backStack.Push(view);
		ContentFrame.AddView(view);

		// Update toolbar
		Toolbar.Title = view.Title;
		
		// Show back button if not root
		if (_backStack.Count > 1)
		{
			Toolbar.NavigationIcon = GetBackIcon();
		}
		else
		{
			Toolbar.NavigationIcon = null;
		}
	}

	partial void PopCore()
	{
		if (_backStack.Count > 1)
		{
			var current = _backStack.Pop();
			ContentFrame.RemoveView(current);

			// Show previous view
			var previous = _backStack.Peek();
			ContentFrame.AddView(previous);

			// Update toolbar
			Toolbar.Title = previous.Title;
			
			// Hide back button if back at root
			if (_backStack.Count == 1)
			{
				Toolbar.NavigationIcon = null;
			}
		}
	}

	partial void PopToRootCore()
	{
		if (_backStack.Count > 1)
		{
			// Get the root (bottom of stack) - need to reverse since Stack.Last() is LIFO
			var root = _backStack.Reverse().First();
			
			// Clear all but root
			ContentFrame.RemoveAllViews();
			_backStack.Clear();
			_backStack.Push(root);

			// Show root view
			ContentFrame.AddView(root);

			// Update toolbar
			Toolbar.Title = root.Title;
			Toolbar.NavigationIcon = null;
		}
	}

	Android.Graphics.Drawables.Drawable? GetBackIcon()
	{
		// Create a simple back arrow using Material icon if available
		// Otherwise toolbar will work without an icon
		var context = Platform.Context;
		if (context != null)
		{
			// Try to get the home as up indicator from AppCompat theme
			var typedArray = context.Theme?.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.HomeAsUpIndicator });
			if (typedArray != null)
			{
				var drawable = typedArray.GetDrawable(0);
				typedArray.Recycle();
				return drawable;
			}
		}
		return null;
	}
}
