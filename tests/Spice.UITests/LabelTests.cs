using OpenQA.Selenium;

namespace Spice.UITests;

public class LabelTests : BaseTest
{
    [Fact]
    public void HelloWorld_Label_Should_DisplayText()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Hello World scenario
            var helloWorldButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Hello World']"));
            helloWorldButton.Click();

            // Assert - Find and verify the label text
            var label = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Hello, Spice')]"));
            Assert.NotNull(label);
            Assert.Contains("Hello, Spice", label.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
