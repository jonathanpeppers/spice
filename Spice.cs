namespace spice;

public class App : Application
{
    public App () => Main = new View {
        Children = {
            new Label {
                Text = "Hello, Spice! 🌶",
                Align = Align.Center,
            }
        }
    };
}

public class Application
{
    public View? Main { get; set; }
}

public class View
{
    public IList<View> Children { get; set; } = new List<View>();
}

public class Label : View
{
    public string Text { get; set; } = "";

    public Align Align { get; set; }
}

public enum Align
{
    Start,
    Center,
    End,
}
