using OpenQA.Selenium;

namespace Spice.UITests;

public class EntryTests : BaseTest
{
    public EntryTests(SharedDriverFixture fixture) : base(fixture) { }

    [Fact]
    public void Entry_Should_DisplayTextAndPassword() => RunTest(() =>
    {
        var entryButton = FindButtonByText("Entry");
        entryButton.Click();

        var editTexts = Driver.FindElements(By.ClassName("android.widget.EditText"));
        Assert.True(editTexts.Count >= 2, "Should have at least 2 Entry controls");

        var emailEntry = editTexts[0];
        Assert.Contains("jonathan.peppers@gmail.com", emailEntry.Text);

        var passwordEntry = editTexts[1];
        Assert.NotNull(passwordEntry);
    });
}
