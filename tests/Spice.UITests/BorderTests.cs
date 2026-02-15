namespace Spice.UITests;

public class BorderTests : BaseTest
{
    [Fact]
    public void Border_Should_DisplayWithContent() => RunTest(() =>
    {
        var borderButton = FindButtonByText("Border");
        borderButton.Click();

        var titleLabel = FindTextViewContaining("Border Examples");
        Assert.NotNull(titleLabel);

        var simpleBorderLabel = FindTextViewContaining("Simple Border");
        Assert.NotNull(simpleBorderLabel);
    });
}
