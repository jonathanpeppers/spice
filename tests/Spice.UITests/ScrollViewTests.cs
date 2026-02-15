using OpenQA.Selenium;

namespace Spice.UITests;

public class ScrollViewTests : BaseTest
{
    [Fact]
    public void ScrollView_Should_DisplayScrollableContent()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ScrollView scenario
            var scrollViewButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='ScrollView']"));
            scrollViewButton.Click();

            // Assert - Find the title label
            var titleLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'ScrollView Demo')]"));
            Assert.NotNull(titleLabel);
            Assert.Contains("ScrollView Demo", titleLabel.Text);

            // Find Item 1 (should be visible)
            var item1 = Driver.FindElement(By.XPath("//android.widget.TextView[@text='Item 1']"));
            Assert.NotNull(item1);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
