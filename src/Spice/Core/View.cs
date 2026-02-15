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

	/// <summary>
	/// Whether the view is visible. Defaults to true.
	/// </summary>
	[ObservableProperty]
	bool _isVisible = true;

	/// <summary>
	/// Whether the view is enabled (can respond to user interaction). Defaults to true.
	/// </summary>
	[ObservableProperty]
	bool _isEnabled = true;

	/// <summary>
	/// Space around the view. Supports uniform (10), horizontal/vertical (10,20), or individual sides (10,20,30,40).
	/// Aligns with Microsoft.Maui Margin property.
	/// </summary>
	[ObservableProperty]
	Thickness _margin;
}