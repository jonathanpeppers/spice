namespace spice;

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
            Children = {
                label,
                button,
            }
        };
    }
}

public class Application
{
    public View? Main { get; set; }
}

public class View
{
    public Align Align { get; set; }

    public IList<View> Children { get; set; } = new List<View>();
}

public class Label : View
{
    public string Text { get; set; } = "";
}

public class Button : Label
{
    public Action<Button> Clicked { get; set; } = delegate { };
}

public enum Align
{
    Start,
    Center,
    End,
}
