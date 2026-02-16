using OpenQA.Selenium;

namespace Spice.UITests;

public class BorderTests : BaseTest
{
    public BorderTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void Border_Should_DisplayWithContent() => RunTest(() =>
    {
        var borderButton = FindButtonByText("Border");
        borderButton.Click();
        Thread.Sleep(1000);

        // Border wraps content in a FrameLayout — find the button inside the last border
        var buttonInBorder = FindButtonByText("Button in Border");
        Assert.NotNull(buttonInBorder);
    });
}
