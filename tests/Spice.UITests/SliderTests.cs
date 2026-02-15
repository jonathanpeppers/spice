using OpenQA.Selenium;

namespace Spice.UITests;

public class SliderTests : BaseTest
{
    [Fact]
    public void Slider_Should_DisplayWithInitialValue()
    {
        try
        {
            InitializeAndroidDriver();

            var sliderButton = FindButtonByText("Slider");
            sliderButton.Click();

            // Verify the value label shows the initial value
            var valueLabel = FindTextViewContaining("Value: 50");
            Assert.NotNull(valueLabel);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
