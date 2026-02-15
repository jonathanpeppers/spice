using OpenQA.Selenium;

namespace Spice.UITests;

public class ButtonTests : BaseTest
{
    [Fact]
    public void HelloWorld_Button_Should_UpdateLabel()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Hello World scenario
            var helloWorldButton = FindButtonByText("Hello World");
            helloWorldButton.Click();

            // Find the "Click Me" button
            var clickMeButton = FindButtonByText("Click Me");
            
            // Find the label - it should initially say "Hello, Spice 🌶"
            var label = FindTextViewContaining("Hello, Spice");
            Assert.Contains("Hello, Spice", label.Text);

            // Click the button
            clickMeButton.Click();

            // Assert - Label should update to show click count
            label = FindTextViewContaining("Times:");
            Assert.Contains("Times: 1", label.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
