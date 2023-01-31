namespace Spice;

public partial class Button : View
{
	[ObservableProperty]
	string _text = "";

	[ObservableProperty]
	Color? _textColor;

	[ObservableProperty]
	Action<Button>? _clicked;
}