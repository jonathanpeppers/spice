using Android.App;
using Android.Content;
using Android.Widget;

namespace Spice;

public partial class DatePicker
{
	/// <summary>
	/// Returns datePicker.NativeView
	/// </summary>
	/// <param name="datePicker">The Spice.DatePicker</param>
	public static implicit operator Android.Widget.Button(DatePicker datePicker) => datePicker.NativeView;

	static Android.Widget.Button Create(Context context) => new(context);

	/// <summary>
	/// Control for date selection.
	/// Android -> Android.App.DatePickerDialog
	/// iOS -> UIKit.UIDatePicker
	/// </summary>
	public DatePicker() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public DatePicker(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected DatePicker(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected DatePicker(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		UpdateText();
		NativeView.Click += OnClick;
	}

	/// <summary>
	/// The underlying Android.Widget.Button
	/// </summary>
	public new Android.Widget.Button NativeView => (Android.Widget.Button)_nativeView.Value;

	void OnClick(object? sender, EventArgs e)
	{
		var context = NativeView.Context;
		if (context is not Activity activity)
		{
			return;
		}

		var dialog = new DatePickerDialog(
			activity,
			(s, args) =>
			{
				Date = new DateTime(args.Year, args.Month + 1, args.DayOfMonth);
			},
			Date.Year,
			Date.Month - 1,
			Date.Day);

		if (MinimumDate.HasValue)
		{
			dialog.DatePicker.MinDate = new DateTimeOffset(MinimumDate.Value).ToUnixTimeMilliseconds();
		}

		if (MaximumDate.HasValue)
		{
			dialog.DatePicker.MaxDate = new DateTimeOffset(MaximumDate.Value).ToUnixTimeMilliseconds();
		}

		dialog.Show();
	}

	void UpdateText()
	{
		NativeView.Text = Date.ToString("d");
	}

	partial void OnDateChanged(DateTime value)
	{
		UpdateText();
	}

	partial void OnMinimumDateChanged(DateTime? value)
	{
		// Applied when dialog is shown
	}

	partial void OnMaximumDateChanged(DateTime? value)
	{
		// Applied when dialog is shown
	}

	partial void OnTextColorChanged(Color? value)
	{
		if (value != null)
			NativeView.SetTextColor(Interop.GetDefaultColorStateList(value.ToAndroidInt()));
	}
}
