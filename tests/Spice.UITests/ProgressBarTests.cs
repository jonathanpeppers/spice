using OpenQA.Selenium;

namespace Spice.UITests;

public class ProgressBarTests : BaseTest
{
    [Fact]
    public void ProgressBar_Should_DisplayWithInitialValue()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ProgressBar scenario
            var progressBarButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='ProgressBar']"));
            progressBarButton.Click();

            // Assert - Find the progress label
            var progressLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Progress:')]"));
            Assert.NotNull(progressLabel);
            Assert.Contains("Progress: 0%", progressLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void ProgressBar_Should_IncrementProgress()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ProgressBar scenario
            var progressBarButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='ProgressBar']"));
            progressBarButton.Click();

            // Find the increment button
            var incrementButton = Driver.FindElement(By.XPath("//android.widget.Button[contains(@text, 'Increment Progress')]"));
            
            // Click increment button
            incrementButton.Click();

            // Assert - Progress should increase to 10%
            var progressLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Progress:')]"));
            Assert.Contains("Progress: 10%", progressLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void ProgressBar_Should_SetToHalf()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ProgressBar scenario
            var progressBarButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='ProgressBar']"));
            progressBarButton.Click();

            // Find the set to 50% button
            var setHalfButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Set to 50%']"));
            
            // Click button
            setHalfButton.Click();

            // Assert - Progress should be 50%
            var progressLabel = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Progress:')]"));
            Assert.Contains("Progress: 50%", progressLabel.Text);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
