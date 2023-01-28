namespace Spice;

public class App : Application
{
    public App ()
    {
        int count = 0;

        var label = new Label {
            Text = "Hello, Spice! 🌶",
            Align = Align.Center,
        };

        var button = new Button {
            Text = "Click Me",
            Clicked = _ => label.Text = $"Times: {++count}"
        };

        Main = new View {
            label,
            button,
        };
    }
}
