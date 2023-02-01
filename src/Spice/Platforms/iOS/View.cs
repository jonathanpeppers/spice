using System.Collections.Specialized;

namespace Spice;

public partial class View
{
	public static implicit operator UIView(View view) => view._nativeView.Value;

	public View() : this(_ => new UIView { AutoresizingMask = UIViewAutoresizing.None }) { }

	public View(CGRect frame) : this(_ => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	public View(Func<View, UIKit.UIView> creator)
	{
		_nativeView = new Lazy<UIView>(() => creator(this));

		Children.CollectionChanged += OnChildrenChanged;
	}

	protected readonly Lazy<UIView> _nativeView;

	public UIView NativeView => _nativeView.Value;

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
				NativeView.AddSubview(item);
				item.UpdateAlign();
			}
		}
	}

	partial void OnBackgroundColorChanged(Color? value) => NativeView.BackgroundColor = value.ToUIColor();

	partial void OnVerticalAlignChanged(Align value) => UpdateAlign();

	partial void OnHorizontalAlignChanged(Align value) => UpdateAlign();

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

		switch (_horizontalAlign)
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
			return (size = view.Frame.Size).Value;
		}
	}
}