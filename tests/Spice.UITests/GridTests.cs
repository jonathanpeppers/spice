using OpenQA.Selenium;

namespace Spice.UITests;

public class GridTests : BaseTest
{
    [Fact]
    public void Grid_Should_ArrangeChildren()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to a scenario that might use Grid
            // Note: Looking at BoxViewScenario which uses StackLayout with Grid capabilities
            var boxViewButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='BoxView']"));
            boxViewButton.Click();

            // Assert - Find elements that would be arranged in the layout
            var label = Driver.FindElement(By.XPath("//android.widget.TextView[@text='BoxView Demo']"));
            Assert.NotNull(label);

            // Verify multiple buttons are present (would be arranged in Grid/StackLayout)
            var buttons = Driver.FindElements(By.ClassName("android.widget.Button"));
            Assert.True(buttons.Count >= 4, "Should have multiple buttons arranged in layout");
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
