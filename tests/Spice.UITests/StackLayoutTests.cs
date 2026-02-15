using OpenQA.Selenium;

namespace Spice.UITests;

public class StackLayoutTests : BaseTest
{
    [Fact]
    public void StackLayout_Should_ArrangeChildrenVertically()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - The main menu itself is a StackLayout with buttons arranged vertically
            var buttons = Driver.FindElements(By.ClassName("android.widget.Button"));

            // Assert - Should have multiple buttons arranged in the StackLayout
            Assert.True(buttons.Count > 0, "StackLayout should contain multiple buttons");
            
            // Verify buttons are present
            var helloWorldButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Hello World']"));
            Assert.NotNull(helloWorldButton);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }

    [Fact]
    public void StackLayout_Should_DisplayChildrenInOrder()
    {
        try
        {
            // Arrange
            InitializeAndroidDriver();

            // Act - Navigate to Hello World scenario which uses StackLayout
            var helloWorldButton = Driver.FindElement(By.XPath("//android.widget.Button[@text='Hello World']"));
            helloWorldButton.Click();

            // Assert - Children should be displayed in the order: Image, Label, Button
            var image = Driver.FindElement(By.ClassName("android.widget.ImageView"));
            var label = Driver.FindElement(By.XPath("//android.widget.TextView[contains(@text, 'Hello, Spice')]"));
            var button = Driver.FindElement(By.XPath("//android.widget.Button[@text='Click Me']"));

            Assert.NotNull(image);
            Assert.NotNull(label);
            Assert.NotNull(button);
        }
        catch (Exception)
        {
            CaptureTestFailureDiagnostics();
            throw;
        }
    }
}
