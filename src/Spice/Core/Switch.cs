namespace Spice;

/// <summary>
/// A toggle switch control for boolean on/off input.
/// Android -> AndroidX.AppCompat.Widget.SwitchCompat
/// iOS -> UIKit.UISwitch
/// </summary>
public partial class Switch : View
{
	/// <summary>
	/// Whether the switch is in the on position
	/// </summary>
	[ObservableProperty]
	bool _isOn;

	/// <summary>
	/// Action to run when the switch is toggled
	/// </summary>
	[ObservableProperty]
	Action<Switch>? _toggled;
}
