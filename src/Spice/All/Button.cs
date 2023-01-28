namespace Spice;

public partial class Button : Label
{
    public Action<Button> Clicked { get; set; } = delegate { };
}