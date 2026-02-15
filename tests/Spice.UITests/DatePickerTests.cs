using OpenQA.Selenium;

namespace Spice.UITests;

public class DatePickerTests : BaseTest
{
    [Fact]
    public void DatePicker_Should_DisplayWithDefaultValue()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to DatePicker scenario
            var datePickerButton = FindButtonByText("DatePicker");
            datePickerButton.Click();

            // Assert - Find the selected label showing date
            var selectedLabel = FindTextViewContaining("Selected:");
            Assert.NotNull(selectedLabel);
            Assert.Contains("Selected:", selectedLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
