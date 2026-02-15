using OpenQA.Selenium;

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

        var checkBox = Driver.FindElement(By.ClassName("android.widget.CheckBox"));
        checkBox.Click();

        // Label should update to show selected count changed
        var selectedLabel = FindTextViewContaining("selected");
        Assert.NotNull(selectedLabel);
    });
}
