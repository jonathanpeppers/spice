using OpenQA.Selenium.Appium;

namespace Spice.UITests;

public class SwitchTests : BaseTest
{
    [Fact]
    public void Switch_Should_ToggleAndUpdateLabel() => RunTest(() =>
    {
        var switchButton = FindButtonByText("Switch");
        switchButton.Click();

        var stateLabel = FindTextViewContaining("Switch is OFF");
        Assert.NotNull(stateLabel);

        // Toggle the first switch using checkable selector
        var switches = Driver.FindElements(MobileBy.AndroidUIAutomator("new UiSelector().checkable(true)"));
        Assert.True(switches.Count >= 1, $"Expected at least 1 checkable element, found {switches.Count}");
        switches[0].Click();
        Thread.Sleep(500);

        var updatedLabel = FindTextViewContaining("Switch is ON");
        Assert.NotNull(updatedLabel);
    });

    [Fact]
    public void Switch_Should_ToggleProgrammatically() => RunTest(() =>
    {
        var switchButton = FindButtonByText("Switch");
        switchButton.Click();

        // First switch starts OFF (unchecked)
        var switches = Driver.FindElements(MobileBy.AndroidUIAutomator("new UiSelector().checkable(true)"));
        Assert.True(switches.Count >= 1, $"Expected at least 1 checkable element, found {switches.Count}");
        Assert.Equal("false", switches[0].GetAttribute("checked"));

        var toggleButton = FindButtonByText("Toggle First Switch Programmatically");
        toggleButton.Click();
        Thread.Sleep(500);

        // After programmatic toggle, switch should be ON (checked)
        switches = Driver.FindElements(MobileBy.AndroidUIAutomator("new UiSelector().checkable(true)"));
        Assert.Equal("true", switches[0].GetAttribute("checked"));
    });
}
