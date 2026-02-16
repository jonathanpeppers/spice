using OpenQA.Selenium;

namespace Spice.UITests;

public class ImageTests : BaseTest
{
    public ImageTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void HelloWorld_Image_Should_BeDisplayed() => RunTest(() =>
    {
        var helloWorldButton = FindButtonByText("Hello World");
        helloWorldButton.Click();

        var image = Driver.FindElement(By.ClassName("android.widget.ImageView"));
        Assert.NotNull(image);
        Assert.True(image.Displayed);
    });
}
