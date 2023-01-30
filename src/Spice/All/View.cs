using System.Collections;

namespace Spice;

public partial class View : IEnumerable<View>
{
	public View this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public Align Align { get; set; }

	public IList<View> Children { get; set; } = new List<View>();

	public void Add(View item) => Children.Add(item);

	public IEnumerator<View> GetEnumerator() => Children.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();
}