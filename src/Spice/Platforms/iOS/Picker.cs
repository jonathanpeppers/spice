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
		var model = new PickerModel(this);
		NativeView.Model = model;
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Picker(CGRect frame) : base(_ => new UIPickerView(frame) { AutoresizingMask = UIViewAutoresizing.None })
	{
		var model = new PickerModel(this);
		NativeView.Model = model;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Picker(Func<View, UIView> creator) : base(creator)
	{
		var model = new PickerModel(this);
		NativeView.Model = model;
	}

	/// <summary>
	/// The underlying UIPickerView
	/// </summary>
	public new UIPickerView NativeView => (UIPickerView)_nativeView.Value;

	partial void OnItemsChanged(ObservableCollection<string> value)
	{
		// Reload the picker data
		NativeView.ReloadAllComponents();
		
		// If selected index is out of range, reset it
		if (_selectedIndex >= value.Count)
		{
			SelectedIndex = -1;
		}
	}

	partial void OnSelectedIndexChanged(int value)
	{
		if (value >= 0 && value < Items.Count)
		{
			NativeView.Select(value, 0, true);
		}
	}

	partial void OnTitleChanged(string value)
	{
		// UIPickerView doesn't have a title property, typically shown in a toolbar or label above it
	}

	partial void OnTextColorChanged(Color? value)
	{
		// Text color is controlled via the model's GetAttributedTitle method
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

		public override string GetTitle(UIPickerView pickerView, nint row, nint component)
		{
			if (_picker.TryGetTarget(out var picker) && row >= 0 && row < picker.Items.Count)
			{
				return picker.Items[(int)row];
			}
			return string.Empty;
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
