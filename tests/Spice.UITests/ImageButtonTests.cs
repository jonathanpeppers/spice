using OpenQA.Selenium;

namespace Spice.UITests;

public class ImageButtonTests : BaseTest
{
    [Fact]
    public void ImageButton_Should_BeClickable()
    {
        try
        {
            InitializeAndroidDriver();

            var imageButtonScenarioButton = FindButtonByText("ImageButton");
            imageButtonScenarioButton.Click();

            // Find the label
            var label = FindTextViewContaining("Click the image");
            Assert.Contains("Click the image button below!", label.Text);

            // Find an image button and click it
            var imageButton = Driver.FindElement(By.ClassName("android.widget.ImageButton"));
            imageButton.Click();

            // Label should update with click count
            label = FindTextViewContaining("Clicked");
            Assert.Contains("Clicked 1 time(s)!", label.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
