namespace Spice;

/// <summary>
/// A checkbox control for boolean selection.
/// Android -> Android.Widget.CheckBox
/// iOS -> UIKit.UIButton (with checkmark styling)
/// </summary>
public partial class CheckBox : View
{
	/// <summary>
	/// Whether the checkbox is checked
	/// </summary>
	[ObservableProperty]
	bool _isChecked;

	/// <summary>
	/// Action to run when the checkbox is checked or unchecked
	/// </summary>
	[ObservableProperty]
	Action<CheckBox>? _checkedChanged;
}
