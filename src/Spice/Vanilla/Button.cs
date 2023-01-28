namespace Spice;

public class Button : Label
{
    public Action<Button> Clicked { get; set; } = delegate { };
}