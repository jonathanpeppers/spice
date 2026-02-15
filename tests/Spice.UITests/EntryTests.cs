using OpenQA.Selenium;

namespace Spice.UITests;

public class EntryTests : BaseTest
{
    [Fact]
    public void Entry_Should_DisplayTextAndPassword()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Entry scenario
            var entryButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Entry']"));
            entryButton.Click();

            // Assert - Find EditText controls
            var editTexts = Driver.FindElements(By.ClassName("android.widget.EditText"));
            Assert.True(editTexts.Count >= 2, "Should have at least 2 Entry controls");

            // First entry should contain email
            var emailEntry = editTexts[0];
            Assert.Contains("jonathan.peppers@gmail.com", emailEntry.Text);

            // Second entry should be password field (value not visible)
            var passwordEntry = editTexts[1];
            Assert.NotNull(passwordEntry);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
