using System.Collections;
using System.Collections.ObjectModel;

namespace Spice;

public partial class View : ObservableObject, IEnumerable<View>
{
	public ObservableCollection<View> Children { get; set; } = new();

	public void Add(View item) => Children.Add(item);

	public IEnumerator<View> GetEnumerator() => Children.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

	[ObservableProperty]
	Align _horizontalAlign;

	[ObservableProperty]
	Align _verticalAlign;
}