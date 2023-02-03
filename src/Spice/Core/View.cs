using System.Collections;
using System.Collections.ObjectModel;

namespace Spice;

/// <summary>
/// The base "View" type for all Spice views.
/// </summary>
public partial class View : ObservableObject, IEnumerable<View>
{
	/// <summary>
	/// Child views of this view
	/// </summary>
	public ObservableCollection<View> Children { get; set; } = new();

	/// <summary>
	/// Calls Children.Add(), collection initializer support
	/// </summary>
	public void Add(View item) => Children.Add(item);

	/// <summary>
	/// You can just foreach over this
	/// </summary>
	public IEnumerator<View> GetEnumerator() => Children.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

	/// <summary>
	/// The view's horizontal alignment. Defaults to center.
	/// </summary>
	[ObservableProperty]
	Align _horizontalAlign;

	/// <summary>
	/// The view's vertical alignment. Defaults to center.
	/// </summary>
	[ObservableProperty]
	Align _verticalAlign;

	/// <summary>
	/// Background color of the view
	/// </summary>
	[ObservableProperty]
	Color? _backgroundColor;
}