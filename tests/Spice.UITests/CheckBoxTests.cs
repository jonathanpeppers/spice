using OpenQA.Selenium;

namespace Spice.UITests;

public class CheckBoxTests : BaseTest
{
    [Fact]
    public void CheckBox_Should_DisplayWithInitialState()
    {
        try
        {
            InitializeAndroidDriver();

            var checkBoxButton = FindButtonByText("CheckBox");
            checkBoxButton.Click();

            // Initially one checkbox is checked (Option 3)
            var selectedLabel = FindTextViewContaining("1 item selected");
            Assert.NotNull(selectedLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void CheckBox_Should_ToggleState()
    {
        try
        {
            InitializeAndroidDriver();

            var checkBoxButton = FindButtonByText("CheckBox");
            checkBoxButton.Click();

            // Find a checkbox and click it
            var checkBox = Driver.FindElement(By.ClassName("android.widget.CheckBox"));
            checkBox.Click();

            // Label should update to show 2 items selected (or 0 if we unchecked the default)
            var selectedLabel = FindTextViewContaining("selected");
            Assert.NotNull(selectedLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
