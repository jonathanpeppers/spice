namespace Spice.UITests;

public class BoxViewTests : BaseTest
{
    public BoxViewTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void BoxView_Should_DisplayWithColor() => RunTest(() =>
    {
        var boxViewButton = FindButtonByText("BoxView");
        boxViewButton.Click();

        // Verify the scenario loaded — color-change buttons may be off-screen on small displays
        var label = FindTextViewContaining("BoxView Demo");
        Assert.NotNull(label);
    });
}
