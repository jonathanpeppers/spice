namespace Spice.UITests;

public class SliderTests : BaseTest
{
    public SliderTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void Slider_Should_DisplayWithInitialValue() => RunTest(() =>
    {
        var sliderButton = FindButtonByText("Slider");
        sliderButton.Click();

        var valueLabel = FindTextViewContaining("Value: 50");
        Assert.NotNull(valueLabel);
    });
}
