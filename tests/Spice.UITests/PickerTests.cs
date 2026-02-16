namespace Spice.UITests;

public class PickerTests : BaseTest
{
    public PickerTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void Picker_Should_DisplayWithDefaultState() => RunTest(() =>
    {
        var pickerButton = FindButtonByText("Picker");
        pickerButton.Click();

        var resultLabel = FindTextViewContaining("No selection");
        Assert.NotNull(resultLabel);
        Assert.Contains("No selection", resultLabel.Text);
    });
}
