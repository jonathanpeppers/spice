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
            var datePickerButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='DatePicker']"));
            datePickerButton.Click();

            // Assert - Find the selected label showing date
            var selectedLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Selected:')]"));
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
