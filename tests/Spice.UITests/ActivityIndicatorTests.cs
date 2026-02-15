using OpenQA.Selenium;

namespace Spice.UITests;

public class ActivityIndicatorTests : BaseTest
{
    [Fact]
    public void ActivityIndicator_Should_BeRunning()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ActivityIndicator scenario
            var activityIndicatorButton = FindButtonByText("ActivityIndicator");
            activityIndicatorButton.Click();

            // Assert - Find ProgressBar (ActivityIndicator maps to ProgressBar on Android)
            var progressBar = Driver.FindElement(By.ClassName("android.widget.ProgressBar"));
            Assert.NotNull(progressBar);
            Assert.True(progressBar.Displayed);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void ActivityIndicator_Should_ToggleRunning()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to ActivityIndicator scenario
            var activityIndicatorButton = FindButtonByText("ActivityIndicator");
            activityIndicatorButton.Click();

            // Find the toggle button
            var toggleButton = FindButtonByText("Toggle Running");
            
            // Assert - ProgressBar should exist
            var progressBar = Driver.FindElement(By.ClassName("android.widget.ProgressBar"));
            Assert.NotNull(progressBar);

            // Click toggle button
            toggleButton.Click();
            
            // Activity indicator should still be present (even if not running)
            progressBar = Driver.FindElement(By.ClassName("android.widget.ProgressBar"));
            Assert.NotNull(progressBar);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
