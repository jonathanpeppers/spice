namespace Spice.UITests;

public class SwitchTests : BaseTest
{
    [Fact]
    public void Switch_Should_ToggleAndUpdateLabel() => RunTest(() =>
    {
        var switchButton = FindButtonByText("Switch");
        switchButton.Click();

        var stateLabel = FindTextViewContaining("Switch is OFF");
        Assert.NotNull(stateLabel);
    });

    [Fact]
    public void Switch_Should_ToggleProgrammatically() => RunTest(() =>
    {
        var switchButton = FindButtonByText("Switch");
        switchButton.Click();

        var toggleButton = FindButtonByText("Toggle First Switch Programmatically");
        toggleButton.Click();
        Thread.Sleep(500);

        var stateLabel = FindTextViewContaining("Switch is ON");
        Assert.NotNull(stateLabel);
    });
}
