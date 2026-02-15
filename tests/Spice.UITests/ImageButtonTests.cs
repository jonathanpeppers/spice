using OpenQA.Selenium;

namespace Spice.UITests;

public class ImageButtonTests : BaseTest
{
    [Fact]
    public void ImageButton_Should_BeClickable()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ImageButton scenario
            var imageButtonScenarioButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='ImageButton']"));
            imageButtonScenarioButton.Click();

            // Find the label
            var label = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Click the image')]"));
            Assert.Contains("Click the image button below!", label.Text);

            // Find the image button and click it
            var imageButtons = Driver.FindElements(By.ClassName("android.widget.ImageView"));
            Assert.True(imageButtons.Count >= 2, "Should have at least 2 ImageButton controls");
            
            var firstImageButton = imageButtons[0];
            firstImageButton.Click();

            // Assert - Label should update with click count
            label = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Clicked')]"));
            Assert.Contains("Clicked 1 time(s)!", label.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
