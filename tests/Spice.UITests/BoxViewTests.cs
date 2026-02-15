namespace Spice.UITests;

public class BoxViewTests : BaseTest
{
    [Fact]
    public void BoxView_Should_DisplayWithColor() => RunTest(() =>
    {
        var boxViewButton = FindButtonByText("BoxView");
        boxViewButton.Click();

        var label = FindTextViewContaining("BoxView Demo");
        Assert.NotNull(label);

        var blueButton = FindButtonByText("Blue Color");
        blueButton.Click();
        
        label = FindTextViewContaining("BoxView Demo");
        Assert.NotNull(label);
    });
}
