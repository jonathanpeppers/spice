using System.Collections;

namespace Spice;

public partial class View : ObservableObject, IEnumerable<View>
{
	public View this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public IList<View> Children { get; set; } = new List<View>();

	public void Add(View item) => Children.Add(item);

	public IEnumerator<View> GetEnumerator() => Children.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

	[ObservableProperty]
	Align _horizontalAlign;

	[ObservableProperty]
	Align _verticalAlign;
}