using Android.Content;
using AndroidX.AppCompat.Widget;

namespace Spice;

public partial class Switch
{
	/// <summary>
	/// Returns switchControl.NativeView
	/// </summary>
	/// <param name="switchControl">The Spice.Switch</param>
	public static implicit operator SwitchCompat(Switch switchControl) => switchControl.NativeView;

	static SwitchCompat Create(Context context) => new(context);

	/// <summary>
	/// A toggle switch control for boolean on/off input.
	/// Android -> AndroidX.AppCompat.Widget.SwitchCompat
	/// iOS -> UIKit.UISwitch
	/// </summary>
	public Switch() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Switch(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Switch(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Switch(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying AndroidX.AppCompat.Widget.SwitchCompat
	/// </summary>
	public new SwitchCompat NativeView => (SwitchCompat)_nativeView.Value;

	partial void OnIsOnChanged(bool value) => NativeView.Checked = value;

	EventHandler<CompoundButton.CheckedChangeEventArgs>? _checkedChange;

	partial void OnToggledChanged(Action<Switch>? value)
	{
		// Unsubscribe existing handler if any
		if (_checkedChange != null)
		{
			NativeView.CheckedChange -= _checkedChange;
			_checkedChange = null;
		}

		if (value != null)
		{
			NativeView.CheckedChange += _checkedChange = (sender, e) =>
			{
				IsOn = e.IsChecked;
				Toggled?.Invoke(this);
			};
		}
	}
}
