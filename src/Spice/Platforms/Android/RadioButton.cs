using Android.Content;
using Android.Widget;
using AndroidRadioButton = Android.Widget.RadioButton;

namespace Spice;

public partial class RadioButton
{
	/// <summary>
	/// Returns radioButton.NativeView
	/// </summary>
	/// <param name="radioButton">The Spice.RadioButton</param>
	public static implicit operator AndroidRadioButton(RadioButton radioButton) => radioButton.NativeView;

	static AndroidRadioButton Create(Context context) => new(context);

	/// <summary>
	/// A radio button control for single selection from a group of options.
	/// Android -> Android.Widget.RadioButton
	/// iOS -> UIKit.UIButton (with circle styling)
	/// </summary>
	public RadioButton() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public RadioButton(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected RadioButton(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected RadioButton(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		// Always subscribe to keep IsChecked in sync
		NativeView.CheckedChange += OnCheckedChange;
	}

	/// <summary>
	/// The underlying Android.Widget.RadioButton
	/// </summary>
	public new AndroidRadioButton NativeView => (AndroidRadioButton)_nativeView.Value;

	bool _updatingChecked;

	partial void OnIsCheckedChangedPartial(bool value)
	{
		if (_updatingChecked)
			return;

		_updatingChecked = true;
		// Guard against disposed Java peer (GC can collect during group operations)
		if (NativeView.Handle != IntPtr.Zero)
			NativeView.Checked = value;
		_updatingChecked = false;
	}

	partial void OnContentChanged(string? value)
	{
		NativeView.Text = value;
	}

	void OnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs e)
	{
		if (_updatingChecked)
			return;

		// Prevent unchecking via user tap - radio buttons can only be unchecked by another button in the group
		if (!e.IsChecked && IsChecked)
		{
			_updatingChecked = true;
			NativeView.Checked = true;
			_updatingChecked = false;
			return;
		}

		// Only fire CheckedChanged if the state actually changes
		bool stateChanged = IsChecked != e.IsChecked;

		_updatingChecked = true;
		IsChecked = e.IsChecked;
		_updatingChecked = false;

		// Fire CheckedChanged only when state changes (not when tapping already-checked button)
		if (stateChanged)
		{
			CheckedChanged?.Invoke(this);
		}
	}

	partial void OnCheckedChangedChanged(Action<RadioButton>? value)
	{
		// Event subscription is handled in constructor
	}
}
