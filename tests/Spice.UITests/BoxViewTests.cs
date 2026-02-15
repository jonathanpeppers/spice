using OpenQA.Selenium;

namespace Spice.UITests;

public class BoxViewTests : BaseTest
{
    [Fact]
    public void BoxView_Should_DisplayWithColor()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to BoxView scenario
            var boxViewButton = FindButtonByText("BoxView");
            boxViewButton.Click();

            // Assert - Find the BoxView Demo label
            var label = FindTextViewContaining("BoxView Demo");
            Assert.NotNull(label);

            // Find color change buttons
            var blueButton = FindButtonByText("Blue Color");
            Assert.NotNull(blueButton);

            // Click blue button to change color
            blueButton.Click();
            
            // BoxView should still be present (color changed)
            label = FindTextViewContaining("BoxView Demo");
            Assert.NotNull(label);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
