namespace Spice;

/// <summary>
/// A parent view for laying out child controls in a row.
/// Defaults to Orientation=Vertical.
/// </summary>
public partial class StackView : View
{
	/// <summary>
	/// Set to specify if layout is vertical or horizontal. Defaults to vertical.
	/// </summary>
	[ObservableProperty]
	Orientation _orientation;
}