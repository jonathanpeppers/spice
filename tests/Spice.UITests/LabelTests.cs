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
            var helloWorldButton = FindButtonByText("Hello World");
            helloWorldButton.Click();

            // Assert - Find and verify the label text
            var label = FindTextViewContaining("Hello, Spice");
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
