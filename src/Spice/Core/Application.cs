namespace Spice;

/// <summary>
/// The root "view" of a Spice application. Set Main to a single view.
/// </summary>
public partial class Application : View
{
	/// <summary>
	/// The single, main "view" of this application
	/// </summary>
	[ObservableProperty]
	View? _main;
}
