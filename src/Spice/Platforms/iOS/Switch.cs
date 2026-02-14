namespace Spice;

public partial class Switch
{
	/// <summary>
	/// Returns switchControl.NativeView
	/// </summary>
	/// <param name="switchControl">The Spice.Switch</param>
	public static implicit operator UISwitch(Switch switchControl) => switchControl.NativeView;

	/// <summary>
	/// A toggle switch control for boolean on/off input.
	/// Android -> AndroidX.AppCompat.Widget.SwitchCompat
	/// iOS -> UIKit.UISwitch
	/// </summary>
	public Switch() : base(_ => new UISwitch { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Switch(CGRect frame) : base(_ => new UISwitch(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Switch(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIKit.UISwitch
	/// </summary>
	public new UISwitch NativeView => (UISwitch)_nativeView.Value;

	partial void OnIsOnChanged(bool value) => NativeView.On = value;

	EventHandler? _valueChanged;

	partial void OnToggledChanged(Action<Switch>? value)
	{
		if (value == null)
		{
			if (_valueChanged != null)
			{
				NativeView.ValueChanged -= _valueChanged;
				_valueChanged = null;
			}
		}
		else
		{
			NativeView.ValueChanged += _valueChanged = (sender, e) =>
			{
				IsOn = NativeView.On;
				Toggled?.Invoke(this);
			};
		}
	}
}
