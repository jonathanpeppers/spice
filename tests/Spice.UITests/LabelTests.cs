namespace Spice.UITests;

public class LabelTests : BaseTest
{
    [Fact]
    public void HelloWorld_Label_Should_DisplayText() => RunTest(() =>
    {
        var helloWorldButton = FindButtonByText("Hello World");
        helloWorldButton.Click();

        var label = FindTextViewContaining("Hello, Spice");
        Assert.NotNull(label);
        Assert.Contains("Hello, Spice", label.Text);
    });
}
