using OpenQA.Selenium;

namespace Spice.UITests;

public class ImageTests : BaseTest
{
    [Fact]
    public void HelloWorld_Image_Should_BeDisplayed()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Hello World scenario
            var helloWorldButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Hello World']"));
            helloWorldButton.Click();

            // Assert - Find the image view (spice image)
            var image = Driver.FindElement(By.ClassName("android.widget.ImageView"));
            Assert.NotNull(image);
            Assert.True(image.Displayed);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
