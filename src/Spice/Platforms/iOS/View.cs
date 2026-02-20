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
		view.UpdateAlign();
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

	partial void OnVerticalOptionsChanged(LayoutOptions value) => UpdateAlign();

	partial void OnHorizontalOptionsChanged(LayoutOptions value) => UpdateAlign();

	partial void OnIsVisibleChanged(bool value) => NativeView.Hidden = !value;

	partial void OnIsEnabledChanged(bool value) => NativeView.UserInteractionEnabled = value;

	partial void OnOpacityChanged(double value) => NativeView.Alpha = (nfloat)value;

	partial void OnAutomationIdChanged(string? value) => NativeView.AccessibilityIdentifier = value;

	partial void OnMarginChanged(Thickness value) => UpdateAlign();

	partial void OnWidthRequestChanged(double value)
	{
		// Trigger layout update if the view is already in the view hierarchy
		if (NativeView.Superview != null)
		{
			UpdateAlign();
		}
	}

	partial void OnHeightRequestChanged(double value)
	{
		// Trigger layout update if the view is already in the view hierarchy
		if (NativeView.Superview != null)
		{
			UpdateAlign();
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
		nativeView.Frame = viewController.View.Bounds;
		nativeView.AutoresizingMask = UIViewAutoresizing.All;

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

	internal void UpdateAlign()
	{
		var view = NativeView;
		var superview = view.Superview;
		if (superview == null || superview is UIStackView)
			return;

		var size = default(CGSize?);
		var superframe = superview.Frame;
		var frame = new CGRect();

		// Apply margins
		var marginLeft = (nfloat)_margin.Left;
		var marginTop = (nfloat)_margin.Top;
		var marginRight = (nfloat)_margin.Right;
		var marginBottom = (nfloat)_margin.Bottom;
		var availableWidth = superframe.Width - marginLeft - marginRight;
		var availableHeight = superframe.Height - marginTop - marginBottom;

		switch (_horizontalOptions.Alignment)
		{
			case LayoutAlignment.Center:
				frame.Width = getSize().Width;
				frame.X = marginLeft + (availableWidth - frame.Width) / 2;
				break;
			case LayoutAlignment.Start:
				frame.Width = getSize().Width;
				frame.X = marginLeft;
				break;
			case LayoutAlignment.End:
				frame.Width = getSize().Width;
				frame.X = superframe.Width - marginRight - frame.Width;
				break;
			case LayoutAlignment.Fill:
				frame.Width = WidthRequest >= 0 ? (nfloat)WidthRequest : availableWidth;
				frame.X = marginLeft;
				break;
			default:
				throw new NotSupportedException($"{nameof(HorizontalOptions)} value '{_horizontalOptions.Alignment}' not supported!");
		}

		switch (_verticalOptions.Alignment)
		{
			case LayoutAlignment.Center:
				frame.Height = getSize().Height;
				frame.Y = marginTop + (availableHeight - frame.Height) / 2;
				break;
			case LayoutAlignment.Start:
				frame.Height = getSize().Height;
				frame.Y = marginTop;
				break;
			case LayoutAlignment.End:
				frame.Height = getSize().Height;
				frame.Y = superframe.Height - marginBottom - frame.Height;
				break;
			case LayoutAlignment.Fill:
				frame.Height = HeightRequest >= 0 ? (nfloat)HeightRequest : availableHeight;
				frame.Y = marginTop;
				break;
			default:
				throw new NotSupportedException($"{nameof(VerticalOptions)} value '{_verticalOptions.Alignment}' not supported!");
		}

		// Set the actual frame
		view.Frame = frame;

		CGSize getSize()
		{
			if (size != null)
				return size.Value;

			view!.SizeToFit();
			var result = view.Frame.Size;
			
			// If WidthRequest or HeightRequest are set, use them instead
			if (WidthRequest >= 0)
				result.Width = (nfloat)WidthRequest;
			if (HeightRequest >= 0)
				result.Height = (nfloat)HeightRequest;
			
			return (size = result).Value;
		}
	}
}