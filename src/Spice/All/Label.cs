namespace Spice;

public partial class Label : View
{
	[ObservableProperty]
	string _text = "";

	[ObservableProperty]
	Color? _textColor;
}