using Android.Content;
using Android.Widget;
using AndroidCheckBox = Android.Widget.CheckBox;

namespace Spice;

public partial class CheckBox
{
	/// <summary>
	/// Returns checkBox.NativeView
	/// </summary>
	/// <param name="checkBox">The Spice.CheckBox</param>
	public static implicit operator AndroidCheckBox(CheckBox checkBox) => checkBox.NativeView;

	static AndroidCheckBox Create(Context context) => new(context);

	/// <summary>
	/// A checkbox control for boolean selection.
	/// Android -> Android.Widget.CheckBox
	/// iOS -> UIKit.UIButton (with checkmark styling)
	/// </summary>
	public CheckBox() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public CheckBox(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected CheckBox(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected CheckBox(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		// Always subscribe to keep IsChecked in sync
		NativeView.CheckedChange += OnCheckedChange;
	}

	/// <summary>
	/// The underlying Android.Widget.CheckBox
	/// </summary>
	public new AndroidCheckBox NativeView => (AndroidCheckBox)_nativeView.Value;

	bool _updatingChecked;

	partial void OnIsCheckedChanged(bool value)
	{
		if (_updatingChecked)
			return;

		_updatingChecked = true;
		NativeView.Checked = value;
		_updatingChecked = false;
	}

	void OnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs e)
	{
		if (_updatingChecked)
			return;

		_updatingChecked = true;
		IsChecked = e.IsChecked;
		_updatingChecked = false;
		CheckedChanged?.Invoke(this);
	}

	partial void OnCheckedChangedChanged(Action<CheckBox>? value)
	{
		// Event subscription is handled in constructor
	}
}
