using OpenQA.Selenium;

namespace Spice.UITests;

public class SliderTests : BaseTest
{
    [Fact]
    public void Slider_Should_DisplayWithInitialValue()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Slider scenario
            var sliderButton = FindButtonByText("Slider");
            sliderButton.Click();

            // Assert - Find SeekBar controls (Slider maps to SeekBar on Android)
            var seekBars = Driver.FindElements(By.ClassName("android.widget.SeekBar"));
            Assert.True(seekBars.Count >= 3, "Should have at least 3 Slider controls");

            // Find the value label
            var valueLabel = FindTextViewContaining("Value: 50");
            Assert.NotNull(valueLabel);
            Assert.Contains("Value: 50", valueLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
