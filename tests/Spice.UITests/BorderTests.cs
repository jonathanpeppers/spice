using OpenQA.Selenium;

namespace Spice.UITests;

public class BorderTests : BaseTest
{
    [Fact]
    public void Border_Should_DisplayWithContent()
    {
        try
        {
            InitializeAndroidDriver();

            var borderButton = FindButtonByText("Border");
            borderButton.Click();

            // Verify the title and at least one border label loaded
            var titleLabel = FindTextViewContaining("Border Examples");
            Assert.NotNull(titleLabel);

            var simpleBorderLabel = FindTextViewContaining("Simple Border");
            Assert.NotNull(simpleBorderLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
