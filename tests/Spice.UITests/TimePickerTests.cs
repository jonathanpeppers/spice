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
            var timePickerButton = FindButtonByText("TimePicker");
            timePickerButton.Click();

            // Assert - Find the selected label showing time (initially 9:30 AM)
            var selectedLabel = FindTextViewContaining("Selected:");
            Assert.NotNull(selectedLabel);
            Assert.Contains("Selected:", selectedLabel.Text);
            
            // Time can be formatted as "9:30 AM" or "09:30 AM" depending on locale
            var labelText = selectedLabel.Text;
            Assert.True(
                labelText.Contains("9:30") || labelText.Contains("09:30"),
                $"Expected time label to contain '9:30' or '09:30', but got: {labelText}");
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
