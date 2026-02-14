namespace Spice;

/// <summary>
/// Defines row-specific properties that apply to Grid elements.
/// </summary>
public partial class RowDefinition : ObservableObject
{
	/// <summary>
	/// Gets or sets the height of a RowDefinition.
	/// </summary>
	[ObservableProperty]
	GridLength _height = GridLength.Star;

	/// <summary>
	/// Initializes a new instance of RowDefinition with a Star height.
	/// </summary>
	public RowDefinition()
	{
	}

	/// <summary>
	/// Initializes a new instance of RowDefinition with the specified height.
	/// </summary>
	public RowDefinition(GridLength height)
	{
		Height = height;
	}
}
