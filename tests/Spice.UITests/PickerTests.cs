using OpenQA.Selenium;

namespace Spice.UITests;

public class PickerTests : BaseTest
{
    [Fact]
    public void Picker_Should_DisplayWithDefaultState()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Picker scenario
            var pickerButton = FindButtonByText("Picker");
            pickerButton.Click();

            // Assert - Find the result label
            var resultLabel = FindTextViewContaining("No selection");
            Assert.NotNull(resultLabel);
            Assert.Contains("No selection", resultLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
