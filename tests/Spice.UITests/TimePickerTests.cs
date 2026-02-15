using OpenQA.Selenium;

namespace Spice.UITests;

public class TimePickerTests : BaseTest
{
    [Fact]
    public void TimePicker_Should_DisplayWithDefaultValue()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to TimePicker scenario
            var timePickerButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='TimePicker']"));
            timePickerButton.Click();

            // Assert - Find the selected label showing time
            var selectedLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Selected:')]"));
            Assert.NotNull(selectedLabel);
            Assert.Contains("Selected:", selectedLabel.Text);
            Assert.Contains("9:30", selectedLabel.Text.Replace("09:30", "9:30"));
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
