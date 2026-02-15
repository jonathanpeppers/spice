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
            var contentViewButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='ContentView']"));
            contentViewButton.Click();

            // Assert - Find labels within ContentView
            var exampleLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='ContentView Example']"));
            Assert.NotNull(exampleLabel);

            var simpleLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Simple ContentView']"));
            Assert.NotNull(simpleLabel);

            var nestedLabel = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Nested ContentView']"));
            Assert.NotNull(nestedLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
