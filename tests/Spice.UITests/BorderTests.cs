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
            var borderButton = FindButtonByText("Border");
            borderButton.Click();

            // Assert - Find the title
            var titleLabel = FindTextViewContaining("Border Examples");
            Assert.NotNull(titleLabel);

            // Find labels within borders
            var simpleBorderLabel = FindTextViewContaining("Simple Border");
            Assert.NotNull(simpleBorderLabel);

            var coloredBorderLabel = FindTextViewContaining("Border with Background");
            Assert.NotNull(coloredBorderLabel);

            var roundedBorderLabel = FindTextViewContaining("Rounded Border");
            Assert.NotNull(roundedBorderLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
