using OpenQA.Selenium;

namespace Spice.UITests;

public class CheckBoxTests : BaseTest
{
    [Fact]
    public void CheckBox_Should_DisplayWithInitialState()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to CheckBox scenario
            var checkBoxButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='CheckBox']"));
            checkBoxButton.Click();

            // Assert - Find checkboxes
            var checkBoxes = Driver.FindElements(By.ClassName("android.widget.CheckBox"));
            Assert.True(checkBoxes.Count >= 3, "Should have at least 3 CheckBox controls");

            // Find the label showing selected count
            var selectedLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'selected')]"));
            Assert.NotNull(selectedLabel);
            // Initially one checkbox is checked (Option 3)
            Assert.Contains("1 item selected", selectedLabel.Text);
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
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to CheckBox scenario
            var checkBoxButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='CheckBox']"));
            checkBoxButton.Click();

            // Find the first checkbox and click it
            var checkBoxes = Driver.FindElements(By.ClassName("android.widget.CheckBox"));
            var firstCheckBox = checkBoxes[0];
            firstCheckBox.Click();

            // Assert - Label should update to show 2 items selected
            var selectedLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'selected')]"));
            Assert.Contains("2 items selected", selectedLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
