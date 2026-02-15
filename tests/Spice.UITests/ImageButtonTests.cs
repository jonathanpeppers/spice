using OpenQA.Selenium;

namespace Spice.UITests;

public class ImageButtonTests : BaseTest
{
    [Fact]
    public void ImageButton_Should_BeClickable() => RunTest(() =>
    {
        var imageButtonScenarioButton = FindButtonByText("ImageButton");
        imageButtonScenarioButton.Click();

        var label = FindTextViewContaining("Click the image");
        Assert.Contains("Click the image button below!", label.Text);

        var imageButton = Driver.FindElement(By.ClassName("android.widget.ImageButton"));
        imageButton.Click();

        label = FindTextViewContaining("Clicked");
        Assert.Contains("Clicked 1 time(s)!", label.Text);
    });
}
