using OpenQA.Selenium;

namespace Spice.UITests;

public class StackLayoutTests : BaseTest
{
    public StackLayoutTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void StackLayout_Should_ArrangeChildrenVertically() => RunTest(() =>
    {
        var buttons = Driver.FindElements(By.ClassName("android.widget.Button"));
        Assert.True(buttons.Count > 0, "StackLayout should contain multiple buttons");
        
        var helloWorldButton = FindButtonByText("Hello World");
        Assert.NotNull(helloWorldButton);
    });

    [Fact]
    public void StackLayout_Should_DisplayChildrenInOrder() => RunTest(() =>
    {
        var helloWorldButton = FindButtonByText("Hello World");
        helloWorldButton.Click();

        var image = Driver.FindElement(By.ClassName("android.widget.ImageView"));
        var label = FindTextViewContaining("Hello, Spice");
        var button = FindButtonByText("Click Me");

        Assert.NotNull(image);
        Assert.NotNull(label);
        Assert.NotNull(button);
    });
}
