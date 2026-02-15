using OpenQA.Selenium;

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

            // Give WebView some time to load
            Thread.Sleep(3000);

            // Assert - Find WebView control
            var webView = Driver.FindElement(By.ClassName("android.webkit.WebView"));
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
