using OpenQA.Selenium;

namespace Spice.UITests;

public class BoxViewTests : BaseTest
{
    [Fact]
    public void BoxView_Should_DisplayWithColor()
    {
        try
        {
            InitializeAndroidDriver();

            var boxViewButton = FindButtonByText("BoxView");
            boxViewButton.Click();

            // Verify the demo label loaded
            var label = FindTextViewContaining("BoxView Demo");
            Assert.NotNull(label);

            // Find and click a color change button
            var blueButton = FindButtonByText("Blue Color");
            blueButton.Click();
            
            // Page should still be present
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
