using System.Collections.Specialized;

namespace Spice;

public partial class View
{
	public static implicit operator UIView(View view) => view._nativeView.Value;

	public View() : this(() => new UIView { TranslatesAutoresizingMaskIntoConstraints = false }) { }

	public View(CGRect frame) : this(() => new UIView(frame) { TranslatesAutoresizingMaskIntoConstraints = false }) { }

	public View(Func<UIKit.UIView> creator)
	{
		_nativeView = new Lazy<UIView>(creator);

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
		if (view.TranslatesAutoresizingMaskIntoConstraints)
			return;

		var superview = view.Superview;
		if (superview == null || superview is UIStackView)
			return;

		switch (_horizontalAlign)
		{
			case Align.Center:
				view.CenterXAnchor.ConstraintEqualTo(superview.CenterXAnchor).Active = true;
				break;
			case Align.Start:
				view.LeftAnchor.ConstraintEqualTo(superview.LeftAnchor).Active = true;
				break;
			case Align.End:
				view.RightAnchor.ConstraintEqualTo(superview.RightAnchor).Active = true;
				break;
			case Align.Stretch:
				view.LeftAnchor.ConstraintEqualTo(superview.LeftAnchor).Active = true;
				view.RightAnchor.ConstraintEqualTo(superview.RightAnchor).Active = true;
				break;
			default:
				throw new NotSupportedException($"{nameof(HorizontalAlign)} value '{_horizontalAlign}' not supported!");
		}

		switch (_horizontalAlign)
		{
			case Align.Center:
				view.CenterYAnchor.ConstraintEqualTo(superview.CenterYAnchor).Active = true;
				break;
			case Align.Start:
				view.TopAnchor.ConstraintEqualTo(superview.TopAnchor).Active = true;
				break;
			case Align.End:
				view.BottomAnchor.ConstraintEqualTo(superview.BottomAnchor).Active = true;
				break;
			case Align.Stretch:
				view.TopAnchor.ConstraintEqualTo(superview.TopAnchor).Active = true;
				view.BottomAnchor.ConstraintEqualTo(superview.BottomAnchor).Active = true;
				break;
			default:
				throw new NotSupportedException($"{nameof(VerticalAlign)} value '{_verticalAlign}' not supported!");
		}
	}
}