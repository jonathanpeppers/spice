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
		Orientation = Android.Widget.Orientation.Vertical
	})
	{
	}

	Toolbar? _toolbar;
	Android.Widget.FrameLayout? _contentFrame;
	readonly Stack<View> _backStack = new();

	/// <summary>
	/// Gets or creates the toolbar for navigation
	/// </summary>
	Toolbar Toolbar
	{
		get
		{
			if (_toolbar == null)
			{
				_toolbar = new Toolbar(Platform.Context)
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
			var root = _backStack.Last();
			
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
		// Use the built-in back arrow from Material Design
		var context = Platform.Context;
		if (context != null)
		{
			return AndroidX.Core.Content.ContextCompat.GetDrawable(context, Resource.Drawable.abc_ic_ab_back_material);
		}
		return null;
	}
}
