using System.Collections.Specialized;

namespace Spice;

public partial class View
{
	public static implicit operator UIView(View view) => view._nativeView.Value;

	public View() : this(() => new UIView { AutoresizingMask = UIViewAutoresizing.All }) { }

	public View(CGRect frame) : this(() => new UIView(frame) { AutoresizingMask = UIViewAutoresizing.All }) { }

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
			}
		}
	}

	partial void OnBackgroundColorChanged(Color? value) => NativeView.BackgroundColor = value.ToUIColor();
}