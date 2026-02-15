using System.Collections.Specialized;

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
		_nativeView = new Lazy<UIView>(() => creator(this));

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

	partial void OnVerticalAlignChanged(Align value) => UpdateAlign();

	partial void OnHorizontalAlignChanged(Align value) => UpdateAlign();

	partial void OnIsVisibleChanged(bool value) => NativeView.Hidden = !value;

	partial void OnIsEnabledChanged(bool value) => NativeView.UserInteractionEnabled = value;

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

	internal void UpdateAlign()
	{
		var view = NativeView;
		var superview = view.Superview;
		if (superview == null || superview is UIStackView)
			return;

		var size = default(CGSize?);
		var superframe = superview.Frame;
		var frame = new CGRect();

		switch (_horizontalAlign)
		{
			case Align.Center:
				frame.Width = getSize().Width;
				frame.X = (superframe.Width - frame.Width) / 2;
				break;
			case Align.Start:
				frame.Width = getSize().Width;
				frame.X = 0;
				break;
			case Align.End:
				frame.Width = getSize().Width;
				frame.X = superframe.Width - frame.Width;
				break;
			case Align.Stretch:
				frame.Width = superframe.Width;
				frame.X = 0;
				break;
			default:
				throw new NotSupportedException($"{nameof(HorizontalAlign)} value '{_horizontalAlign}' not supported!");
		}

		switch (_verticalAlign)
		{
			case Align.Center:
				frame.Height = getSize().Height;
				frame.Y = (superframe.Height - frame.Height) / 2;
				break;
			case Align.Start:
				frame.Height = getSize().Height;
				frame.Y = 0;
				break;
			case Align.End:
				frame.Height = getSize().Height;
				frame.Y = superframe.Height - frame.Height;
				break;
			case Align.Stretch:
				frame.Height = superframe.Height;
				frame.Y = 0;
				break;
			default:
				throw new NotSupportedException($"{nameof(VerticalAlign)} value '{_verticalAlign}' not supported!");
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