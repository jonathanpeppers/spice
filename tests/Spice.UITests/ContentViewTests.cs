namespace Spice.UITests;

public class ContentViewTests : BaseTest
{
    [Fact]
    public void ContentView_Should_DisplayNestedContent() => RunTest(() =>
    {
        var contentViewButton = FindButtonByText("ContentView");
        contentViewButton.Click();

        var exampleLabel = FindTextViewContaining("ContentView Example");
        Assert.NotNull(exampleLabel);

        var simpleLabel = FindTextViewContaining("Simple ContentView");
        Assert.NotNull(simpleLabel);

        var nestedLabel = FindTextViewContaining("Nested ContentView");
        Assert.NotNull(nestedLabel);
    });
}
