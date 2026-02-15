namespace Spice;

/// <summary>
/// Defines column-specific properties that apply to Grid elements.
/// </summary>
public partial class ColumnDefinition : ObservableObject
{
	/// <summary>
	/// Gets or sets the width of a ColumnDefinition.
	/// </summary>
	[ObservableProperty]
	GridLength _width = GridLength.Star;

	/// <summary>
	/// Initializes a new instance of ColumnDefinition with a Star width.
	/// </summary>
	public ColumnDefinition()
	{
	}

	/// <summary>
	/// Initializes a new instance of ColumnDefinition with the specified width.
	/// </summary>
	public ColumnDefinition(GridLength width)
	{
		Width = width;
	}
}
