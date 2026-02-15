using OpenQA.Selenium;

namespace Spice.UITests;

public class SwitchTests : BaseTest
{
    [Fact]
    public void Switch_Should_ToggleAndUpdateLabel()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Switch scenario
            var switchButton = FindButtonByText("Switch");
            switchButton.Click();

            // Find the first switch control
            var switches = Driver.FindElements(By.ClassName("androidx.appcompat.widget.SwitchCompat"));
            Assert.True(switches.Count >= 2, "Should have at least 2 Switch controls");

            var firstSwitch = switches[0];
            
            // Find the label that shows switch state
            var stateLabel = FindTextViewContaining("Switch is");
            
            // Initially should be OFF
            Assert.Contains("Switch is OFF", stateLabel.Text);

            // Toggle the switch
            firstSwitch.Click();

            // Assert - Label should update to ON
            stateLabel = FindTextViewContaining("Switch is");
            Assert.Contains("Switch is ON", stateLabel.Text);
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
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Switch scenario
            var switchButton = FindButtonByText("Switch");
            switchButton.Click();

            // Find the toggle button
            var toggleButton = FindButtonByText("Toggle First Switch Programmatically");
            
            // Find the label that shows switch state
            var stateLabel = FindTextViewContaining("Switch is");
            
            // Initially should be OFF
            Assert.Contains("Switch is OFF", stateLabel.Text);

            // Click the toggle button
            toggleButton.Click();

            // Wait briefly for the programmatic toggle to propagate
            Thread.Sleep(500);

            // Assert - Label should update to ON
            stateLabel = FindTextViewContaining("Switch is ON");
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
