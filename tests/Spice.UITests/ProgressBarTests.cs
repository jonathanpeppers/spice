namespace Spice.UITests;

public class ProgressBarTests : BaseTest
{
    [Fact]
    public void ProgressBar_Should_DisplayWithInitialValue() => RunTest(() =>
    {
        var progressBarButton = FindButtonByText("ProgressBar");
        progressBarButton.Click();

        var progressLabel = FindTextViewContaining("Progress:");
        Assert.NotNull(progressLabel);
        Assert.Contains("Progress: 0%", progressLabel.Text);
    });

    [Fact]
    public void ProgressBar_Should_IncrementProgress() => RunTest(() =>
    {
        var progressBarButton = FindButtonByText("ProgressBar");
        progressBarButton.Click();

        var incrementButton = FindButtonByText("Increment Progress (+10%)");
        incrementButton.Click();

        var progressLabel = FindTextViewContaining("Progress:");
        Assert.Contains("Progress: 10%", progressLabel.Text);
    });

    [Fact]
    public void ProgressBar_Should_SetToHalf() => RunTest(() =>
    {
        var progressBarButton = FindButtonByText("ProgressBar");
        progressBarButton.Click();

        var setHalfButton = FindButtonByText("Set to 50%");
        setHalfButton.Click();

        var progressLabel = FindTextViewContaining("Progress:");
        Assert.Contains("Progress: 50%", progressLabel.Text);
    });
}
