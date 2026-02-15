namespace Spice;

/// <summary>
/// A radio button control for single selection from a group of options.
/// Android -> Android.Widget.RadioButton
/// iOS -> UIKit.UIButton (with circle styling)
/// </summary>
public partial class RadioButton : View
{
	/// <summary>
	/// Whether the radio button is checked
	/// </summary>
	[ObservableProperty]
	bool _isChecked;

	/// <summary>
	/// The group name for mutually exclusive selection
	/// </summary>
	[ObservableProperty]
	string? _groupName;

	/// <summary>
	/// The text content to display next to the radio button
	/// </summary>
	[ObservableProperty]
	string? _content;

	/// <summary>
	/// Action to run when the radio button is checked or unchecked
	/// </summary>
	[ObservableProperty]
	Action<RadioButton>? _checkedChanged;
}
