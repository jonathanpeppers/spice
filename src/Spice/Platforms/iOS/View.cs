using System.Collections.Specialized;
using CoreFoundation;
using Foundation;

namespace Spice;

public partial class View
{
	/// <summary>
	/// Returns view.NativeView
	/// </summary>
	/// <param name="view">The Spice.View</param>
	public static implicit operator UIView(View view) => view.NativeView;

	/// <summary>
	/// View ctor
	/// </summary>
	public View() : this(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <summary>
	/// View ctor
	/// </summary>
	/// <param name="frame">Pass the underlying view a frame</param>
	public View(CGRect frame) : this(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <summary>
	/// View ctor
	/// </summary>
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected View(Func<View, UIView> creator)
	{
		_nativeView = new Lazy<UIView>(() =>
		{
			// UIKit views must be created on the main thread
			if (NSThread.IsMain)
				return creator(this);

			UIView? view = null;
			DispatchQueue.MainQueue.DispatchSync(() => view = creator(this));
			return view!;
		});

		Children.CollectionChanged += OnChildrenChanged;
	}

	/// <summary>
	/// Subclasses can access the underlying Lazy
	/// </summary>
	protected readonly Lazy<UIView> _nativeView;

	/// <summary>
	/// The underlying UIView
	/// </summary>
	public UIView NativeView => _nativeView.Value;

	/// <summary>
	/// Subclasses can override when a view is added
	/// </summary>
	/// <param name="view">The Spice.View</param>
	protected virtual void AddSubview(View view)
	{
		NativeView.AddSubview(view);
		view.ApplyConstraints();
	}

	void OnChildrenChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.OldItems != null)
		{
			foreach (View item in e.OldItems)
			{
				((UIView)item).RemoveFromSuperview();
			}
		}

		if (e.NewItems != null)
		{
			foreach (View item in e.NewItems)
			{
				AddSubview(item);
			}
		}
	}

	partial void OnBackgroundColorChanged(Color? value) => NativeView.BackgroundColor = value.ToUIColor();

	partial void OnVerticalOptionsChanged(LayoutOptions value) => ApplyConstraints();

	partial void OnHorizontalOptionsChanged(LayoutOptions value) => ApplyConstraints();

	partial void OnIsVisibleChanged(bool value) => NativeView.Hidden = !value;

	partial void OnIsEnabledChanged(bool value) => NativeView.UserInteractionEnabled = value;

	partial void OnOpacityChanged(double value) => NativeView.Alpha = (nfloat)value;

	partial void OnAutomationIdChanged(string? value) => NativeView.AccessibilityIdentifier = value;

	partial void OnMarginChanged(Thickness value) => ApplyConstraints();

	partial void OnWidthRequestChanged(double value)
	{
		// Trigger layout update if the view is already in the view hierarchy
		if (NativeView.Superview != null)
		{
			ApplyConstraints();
		}
	}

	partial void OnHeightRequestChanged(double value)
	{
		// Trigger layout update if the view is already in the view hierarchy
		if (NativeView.Superview != null)
		{
			ApplyConstraints();
		}
	}

	private partial double GetWidth() => (double)NativeView.Frame.Width;

	private partial double GetHeight() => (double)NativeView.Frame.Height;

	WeakReference<UIViewController>? _presentedViewController;

	private partial Task PresentAsyncCore(View view)
	{
		var rootViewController = Platform.Window?.RootViewController;
		if (rootViewController == null)
		{
			return Task.CompletedTask;
		}

		// Only present if this view is in the view hierarchy
		if (NativeView.Window == null)
		{
			return Task.CompletedTask;
		}

		var viewController = new UIViewController();
		var nativeView = (UIView)view;
		viewController.View!.AddSubview(nativeView);
		ConstraintHelper.PinEdges(nativeView, viewController.View!);

		if (!string.IsNullOrEmpty(view.Title))
		{
			viewController.Title = view.Title;
		}

		_presentedViewController = new WeakReference<UIViewController>(viewController);

		return rootViewController.PresentViewControllerAsync(viewController, animated: true);
	}

	private partial Task DismissAsyncCore()
	{
		if (_presentedViewController != null && _presentedViewController.TryGetTarget(out var viewController))
		{
			_presentedViewController = null;
			return viewController.DismissViewControllerAsync(animated: true);
		}
		return Task.CompletedTask;
	}

	NSLayoutConstraint[]? _alignConstraints;

	internal void ApplyConstraints()
	{
		var view = NativeView;
		var superview = view.Superview;
		if (superview == null || superview is UIStackView)
			return;

		// Remove previous constraints
		ConstraintHelper.RemoveConstraints(_alignConstraints);

		_alignConstraints = ConstraintHelper.ApplyAlignment(
			view,
			superview,
			_horizontalOptions.Alignment,
			_verticalOptions.Alignment,
			_margin,
			_widthRequest,
			_heightRequest);
	}

	// Keep for backward compatibility with callers
	internal void UpdateAlign() => ApplyConstraints();
}