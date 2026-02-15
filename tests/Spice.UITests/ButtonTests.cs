using OpenQA.Selenium;

namespace Spice.UITests;

public class ButtonTests : BaseTest
{
    [Fact]
    public void HelloWorld_Button_Should_UpdateLabel() => RunTest(() =>
    {
        var helloWorldButton = FindButtonByText("Hello World");
        helloWorldButton.Click();

        var clickMeButton = FindButtonByText("Click Me");
        
        var label = FindTextViewContaining("Hello, Spice");
        Assert.Contains("Hello, Spice", label.Text);

        clickMeButton.Click();

        label = FindTextViewContaining("Times:");
        Assert.Contains("Times: 1", label.Text);
    });
}
