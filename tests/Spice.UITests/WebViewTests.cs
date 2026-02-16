using OpenQA.Selenium;

namespace Spice.UITests;

public class WebViewTests : BaseTest
{
    [Fact]
    public void WebView_Should_LoadWebsite() => RunTest(() =>
    {
        var webViewButton = FindButtonByText("WebView");
        webViewButton.Click();

        var webView = Driver.FindElement(By.ClassName("android.webkit.WebView"));
        Assert.NotNull(webView);
        Assert.True(webView.Displayed);
    });
}
