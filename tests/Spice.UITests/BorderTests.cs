using OpenQA.Selenium;

namespace Spice.UITests;

public class BorderTests : BaseTest
{
    [Fact]
    public void Border_Should_DisplayWithContent()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Border scenario
            var borderButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Border']"));
            borderButton.Click();

            // Assert - Find the title
            var titleLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Border Examples']"));
            Assert.NotNull(titleLabel);

            // Find labels within borders
            var simpleBorderLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Simple Border']"));
            Assert.NotNull(simpleBorderLabel);

            var coloredBorderLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Border with Background']"));
            Assert.NotNull(coloredBorderLabel);

            var roundedBorderLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Rounded Border']"));
            Assert.NotNull(roundedBorderLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
