using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Spice;

public partial class Picker
{
	/// <summary>
	/// Returns picker.NativeView
	/// </summary>
	/// <param name="picker">The Spice.Picker</param>
	public static implicit operator UIPickerView(Picker picker) => picker.NativeView;

	/// <summary>
	/// Picker control for selecting an item from a list.
	/// Android -> Android.Widget.Spinner
	/// iOS -> UIKit.UIPickerView
	/// </summary>
	public Picker() : base(_ => new UIPickerView { AutoresizingMask = UIViewAutoresizing.None })
	{
		NativeView.Model = new PickerModel(this);
		Items.CollectionChanged += OnItemsCollectionChanged;
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Picker(CGRect frame) : base(_ => new UIPickerView(frame) { AutoresizingMask = UIViewAutoresizing.None })
	{
		NativeView.Model = new PickerModel(this);
		Items.CollectionChanged += OnItemsCollectionChanged;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Picker(Func<View, UIView> creator) : base(creator)
	{
		NativeView.Model = new PickerModel(this);
		Items.CollectionChanged += OnItemsCollectionChanged;
	}

	/// <summary>
	/// The underlying UIPickerView
	/// </summary>
	public new UIPickerView NativeView => (UIPickerView)_nativeView.Value;

	partial void OnItemsChanged(ObservableCollection<string>? oldValue, ObservableCollection<string> newValue)
	{
		if (oldValue != null)
			oldValue.CollectionChanged -= OnItemsCollectionChanged;
		newValue.CollectionChanged += OnItemsCollectionChanged;

		ReloadItems();
	}

	void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ReloadItems();
	}

	void ReloadItems()
	{
		NativeView.ReloadAllComponents();

		// Clamp selected index to valid range
		if (Items.Count == 0)
			return;

		if (_selectedIndex < 0 || _selectedIndex >= Items.Count)
		{
			SelectedIndex = 0;
		}
		else
		{
			NativeView.Select(_selectedIndex, 0, false);
		}
	}

	partial void OnSelectedIndexChanged(int value)
	{
		if (Items.Count == 0)
			return;

		// Clamp to valid range for UIPickerView
		var index = value;
		if (index < 0 || index >= Items.Count)
			index = 0;

		NativeView.Select(index, 0, true);
	}

	partial void OnTitleChanged(string value)
	{
		// UIPickerView doesn't have a title property, typically shown in a toolbar or label above it
	}

	partial void OnTextColorChanged(Color? value)
	{
		NativeView.ReloadAllComponents();
	}

	class PickerModel : UIPickerViewModel
	{
		readonly WeakReference<Picker> _picker;

		public PickerModel(Picker picker)
		{
			_picker = new WeakReference<Picker>(picker);
		}

		public override nint GetComponentCount(UIPickerView pickerView) => 1;

		public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
		{
			if (_picker.TryGetTarget(out var picker))
			{
				return picker.Items.Count;
			}
			return 0;
		}

		public override NSAttributedString GetAttributedTitle(UIPickerView pickerView, nint row, nint component)
		{
			if (!_picker.TryGetTarget(out var picker) || row < 0 || row >= picker.Items.Count)
				return new NSAttributedString(string.Empty);

			var text = picker.Items[(int)row];

			if (picker.TextColor is Color color)
			{
				var uiColor = UIColor.FromRGBA(
					(nfloat)color.Red,
					(nfloat)color.Green,
					(nfloat)color.Blue,
					(nfloat)color.Alpha);

				return new NSAttributedString(text, new UIStringAttributes { ForegroundColor = uiColor });
			}

			return new NSAttributedString(text);
		}

		public override void Selected(UIPickerView pickerView, nint row, nint component)
		{
			if (_picker.TryGetTarget(out var picker))
			{
				picker.SelectedIndex = (int)row;
			}
		}
	}
}
