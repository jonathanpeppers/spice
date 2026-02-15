using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Spice.UITests;

public class WebViewTests : BaseTest
{
    [Fact]
    public void WebView_Should_LoadWebsite()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to WebView scenario
            var webViewButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='WebView']"));
            webViewButton.Click();

            // Wait for WebView to load
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var webView = wait.Until(driver => driver.FindElement(By.ClassName("android.webkit.WebView")));

            // Assert - Find WebView control
            Assert.NotNull(webView);
            Assert.True(webView.Displayed);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
