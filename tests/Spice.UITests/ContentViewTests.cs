using OpenQA.Selenium;

namespace Spice.UITests;

public class ContentViewTests : BaseTest
{
    [Fact]
    public void ContentView_Should_DisplayNestedContent()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ContentView scenario
            var contentViewButton = FindButtonByText("ContentView");
            contentViewButton.Click();

            // Assert - Find labels within ContentView
            var exampleLabel = FindTextViewContaining("ContentView Example");
            Assert.NotNull(exampleLabel);

            var simpleLabel = FindTextViewContaining("Simple ContentView");
            Assert.NotNull(simpleLabel);

            var nestedLabel = FindTextViewContaining("Nested ContentView");
            Assert.NotNull(nestedLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
