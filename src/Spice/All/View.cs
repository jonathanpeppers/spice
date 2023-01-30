using System.Collections;

namespace Spice;

public partial class View : ObservableObject, IEnumerable<View>
{
	public View this[int index]
	{
		get => Children[index];
		set => Children[index] = value;
	}

	public IList<View> Children { get; set; } = new List<View>();

	public
#if !VANILLA
		partial
#endif
		void Add(View item)
#if VANILLA
		=> Children.Add(item)
#endif
	;

	public IEnumerator<View> GetEnumerator() => Children.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

	[ObservableProperty]
	Align _horizontalAlign;

	[ObservableProperty]
	Align _verticalAlign;
}