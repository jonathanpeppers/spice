namespace Spice;

public partial class DatePicker
{
	/// <summary>
	/// Returns datePicker.NativeView
	/// </summary>
	/// <param name="datePicker">The Spice.DatePicker</param>
	public static implicit operator UIDatePicker(DatePicker datePicker) => datePicker.NativeView;

	/// <summary>
	/// Control for date selection.
	/// Android -> Android.App.DatePickerDialog
	/// iOS -> UIKit.UIDatePicker
	/// </summary>
	public DatePicker() : base(_ => new UIDatePicker
	{
		AutoresizingMask = UIViewAutoresizing.None,
		Mode = UIDatePickerMode.Date,
		PreferredDatePickerStyle = UIDatePickerStyle.Compact
	})
	{
		NativeView.ValueChanged += OnValueChanged;
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public DatePicker(CGRect frame) : base(_ => new UIDatePicker(frame)
	{
		AutoresizingMask = UIViewAutoresizing.None,
		Mode = UIDatePickerMode.Date,
		PreferredDatePickerStyle = UIDatePickerStyle.Compact
	})
	{
		NativeView.ValueChanged += OnValueChanged;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected DatePicker(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIDatePicker
	/// </summary>
	public new UIDatePicker NativeView => (UIDatePicker)_nativeView.Value;

	void OnValueChanged(object? sender, EventArgs e)
	{
		Date = NativeView.Date.ToDateTime().Date;
	}

	partial void OnDateChanged(DateTime value)
	{
		NativeView.Date = value.ToNSDate();
	}

	partial void OnMinimumDateChanged(DateTime? value)
	{
		if (value.HasValue)
		{
			NativeView.MinimumDate = value.Value.ToNSDate();
		}
	}

	partial void OnMaximumDateChanged(DateTime? value)
	{
		if (value.HasValue)
		{
			NativeView.MaximumDate = value.Value.ToNSDate();
		}
	}

	partial void OnTextColorChanged(Color? value)
	{
		// UIDatePicker doesn't have a direct text color property in compact style
		// The color is controlled by the system theme
	}
}
