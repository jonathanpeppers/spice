using OpenQA.Selenium;

namespace Spice.UITests;

public class AppLaunchTests : BaseTest
{
    [Fact]
    public void App_Should_LaunchWithoutCrashing() => RunTest(() =>
    {
        Assert.NotNull(Driver);
        Assert.True(Driver.SessionId != null, "Driver should have a valid session");

        var currentActivity = Driver.CurrentActivity;
        Assert.False(string.IsNullOrEmpty(currentActivity), "Should have an active activity");
    });

    [Fact]
    public void MainMenu_Should_DisplayAllScenarioButtons() => RunTest(() =>
    {
        var buttons = Driver.FindElements(By.ClassName("android.widget.Button"));
        Assert.True(buttons.Count > 0, "Should have at least one scenario button");

        var buttonTexts = buttons.Select(b => b.Text).ToList();
        Assert.Contains("HELLO WORLD", buttonTexts);
        Assert.Contains("IMAGEBUTTON", buttonTexts);
        Assert.Contains("WEBVIEW", buttonTexts);
    });
}
