using OpenQA.Selenium.Appium;

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

        // Use UiAutomator selector — class name may differ across Android versions
        var imageButtons = Driver.FindElements(MobileBy.AndroidUIAutomator(
            "new UiSelector().clickable(true).className(\"android.widget.ImageButton\")"));
        Assert.True(imageButtons.Count >= 1, $"Expected at least 1 ImageButton, found {imageButtons.Count}");
        imageButtons[0].Click();

        label = FindTextViewContaining("Clicked");
        Assert.Contains("Clicked 1 time(s)!", label.Text);
    });
}
