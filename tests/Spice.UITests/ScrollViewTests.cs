namespace Spice.UITests;

public class ScrollViewTests : BaseTest
{
    [Fact]
    public void ScrollView_Should_DisplayScrollableContent() => RunTest(() =>
    {
        var scrollViewButton = FindButtonByText("ScrollView");
        scrollViewButton.Click();

        var titleLabel = FindTextViewContaining("ScrollView Demo");
        Assert.NotNull(titleLabel);
        Assert.Contains("ScrollView Demo", titleLabel.Text);

        var item1 = FindTextViewContaining("Item 1");
        Assert.NotNull(item1);
    });
}
