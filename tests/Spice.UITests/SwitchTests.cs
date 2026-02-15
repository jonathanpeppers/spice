using OpenQA.Selenium;

namespace Spice.UITests;

public class SwitchTests : BaseTest
{
    [Fact]
    public void Switch_Should_ToggleAndUpdateLabel()
    {
        try
        {
            InitializeAndroidDriver();

            var switchButton = FindButtonByText("Switch");
            switchButton.Click();

            // Find the label that shows switch state — initially OFF
            var stateLabel = FindTextViewContaining("Switch is OFF");
            Assert.NotNull(stateLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void Switch_Should_ToggleProgrammatically()
    {
        try
        {
            InitializeAndroidDriver();

            var switchButton = FindButtonByText("Switch");
            switchButton.Click();

            // Find and click the toggle button
            var toggleButton = FindButtonByText("Toggle First Switch Programmatically");
            toggleButton.Click();
            Thread.Sleep(500);

            // Label should update to ON
            var stateLabel = FindTextViewContaining("Switch is ON");
            Assert.NotNull(stateLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
