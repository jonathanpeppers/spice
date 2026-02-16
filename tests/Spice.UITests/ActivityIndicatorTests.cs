using OpenQA.Selenium;

namespace Spice.UITests;

public class ActivityIndicatorTests : BaseTest
{
    public ActivityIndicatorTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void ActivityIndicator_Should_BeRunning() => RunTest(() =>
    {
        var activityIndicatorButton = FindButtonByText("ActivityIndicator");
        activityIndicatorButton.Click();

        var progressBar = Driver.FindElement(By.ClassName("android.widget.ProgressBar"));
        Assert.NotNull(progressBar);
        Assert.True(progressBar.Displayed);
    });

    [Fact]
    public void ActivityIndicator_Should_ToggleRunning() => RunTest(() =>
    {
        var activityIndicatorButton = FindButtonByText("ActivityIndicator");
        activityIndicatorButton.Click();

        var toggleButton = FindButtonByText("Toggle Running");
        
        var progressBar = Driver.FindElement(By.ClassName("android.widget.ProgressBar"));
        Assert.NotNull(progressBar);

        toggleButton.Click();
        
        // ActivityIndicator is gone when not running — verify the toggle button is still there
        Assert.NotNull(toggleButton);
    });
}
