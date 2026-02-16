namespace Spice.UITests;

public class DatePickerTests : BaseTest
{
    [Fact]
    public void DatePicker_Should_DisplayWithDefaultValue() => RunTest(() =>
    {
        var datePickerButton = FindButtonByText("DatePicker");
        datePickerButton.Click();

        var selectedLabel = FindTextViewContaining("Selected:");
        Assert.NotNull(selectedLabel);
        Assert.Contains("Selected:", selectedLabel.Text);
    });
}
