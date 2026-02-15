using OpenQA.Selenium;

namespace Spice.UITests;

public class AppLaunchTests : BaseTest
{
    [Fact]
    public void App_Should_LaunchWithoutCrashing()
    {
        try
        {
            // Arrange & Act
            InitializeAndroidDriver();

            // Assert - If we get here without an exception, the app launched successfully
            Assert.NotNull(Driver);
            Assert.True(Driver.SessionId != null, "Driver should have a valid session");

            // Verify the driver is responsive and the current activity matches expected
            var currentActivity = Driver.CurrentActivity;
            Assert.Equal(ActivityName, currentActivity);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void MainMenu_Should_DisplayAllScenarioButtons()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Find all buttons on the main screen
            var buttons = Driver.FindElements(By.ClassName("android.widget.Button"));

            // Assert - Verify we have scenario buttons available
            Assert.True(buttons.Count > 0, "Should have at least one scenario button");

            // Verify some expected buttons exist
            var buttonTexts = buttons.Select(b => b.Text).ToList();
            Assert.Contains("Hello World", buttonTexts);
            Assert.Contains("ImageButton", buttonTexts);
            Assert.Contains("WebView", buttonTexts);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
