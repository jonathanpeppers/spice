namespace Spice.UITests;

public class BorderTests : BaseTest
{
    [Fact]
    public void Border_Should_DisplayWithContent() => RunTest(() =>
    {
        var borderButton = FindButtonByText("Border");
        borderButton.Click();
        Thread.Sleep(1000);

        var simpleBorderLabel = FindTextViewContaining("Simple Border");
        Assert.NotNull(simpleBorderLabel);
    });
}
