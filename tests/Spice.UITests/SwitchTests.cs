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
            var switchButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Switch']"));
            switchButton.Click();

            // Find the first switch control
            var switches = Driver.FindElements(By.ClassName("android.widget.Switch"));
            Assert.True(switches.Count >= 2, "Should have at least 2 Switch controls");

            var firstSwitch = switches[0];
            
            // Find the label that shows switch state
            var stateLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Switch is')]"));
            
            // Initially should be OFF
            Assert.Contains("Switch is OFF", stateLabel.Text);

            // Toggle the switch
            firstSwitch.Click();

            // Assert - Label should update to ON
            stateLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Switch is')]"));
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
            var switchButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Switch']"));
            switchButton.Click();

            // Find the toggle button
            var toggleButton = Driver.FindElement(By.XPath("//android.widget.Button[contains(@text, 'Toggle First Switch')]"));
            
            // Find the label that shows switch state
            var stateLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Switch is')]"));
            
            // Initially should be OFF
            Assert.Contains("Switch is OFF", stateLabel.Text);

            // Click the toggle button
            toggleButton.Click();

            // Assert - Label should update to ON
            stateLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Switch is')]"));
            Assert.Contains("Switch is ON", stateLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
