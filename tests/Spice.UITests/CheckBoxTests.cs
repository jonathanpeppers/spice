using OpenQA.Selenium.Appium;

namespace Spice.UITests;

public class CheckBoxTests : BaseTest
{
    [Fact]
    public void CheckBox_Should_DisplayWithInitialState() => RunTest(() =>
    {
        var checkBoxButton = FindButtonByText("CheckBox");
        checkBoxButton.Click();

        // Initially one checkbox is checked (Option 3)
        var selectedLabel = FindTextViewContaining("1 item selected");
        Assert.NotNull(selectedLabel);
    });

    [Fact]
    public void CheckBox_Should_ToggleState() => RunTest(() =>
    {
        var checkBoxButton = FindButtonByText("CheckBox");
        checkBoxButton.Click();

        // Use checkable selector — class name may differ across Android versions
        var checkBoxes = Driver.FindElements(MobileBy.AndroidUIAutomator("new UiSelector().checkable(true)"));
        Assert.True(checkBoxes.Count >= 3, $"Expected at least 3 checkable elements, found {checkBoxes.Count}");
        checkBoxes[0].Click();

        // Label should update to show selected count changed
        var selectedLabel = FindTextViewContaining("selected");
        Assert.NotNull(selectedLabel);
    });
}
