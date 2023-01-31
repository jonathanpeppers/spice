namespace Spice;

public partial class Button : Label
{
	[ObservableProperty]
	Action<Button>? _clicked;
}