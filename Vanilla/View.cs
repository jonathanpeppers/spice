namespace Spice;

public class View
{
    public Align Align { get; set; }

    public IList<View> Children { get; set; } = new List<View>();
}