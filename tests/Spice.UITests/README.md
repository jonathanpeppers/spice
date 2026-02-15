# Spice UI Tests

This directory contains UI tests for the Spice framework using Appium WebDriver for Android automation.

## Test Coverage

UI tests are provided for all implemented Spice controls:

- ActivityIndicator
- Border
- BoxView
- Button
- CheckBox
- ContentView
- DatePicker
- Entry

- Image
- ImageButton
- Label
- Picker
- ProgressBar
- ScrollView
- Slider
- StackLayout
- Switch
- TimePicker
- WebView (including BlazorWebView)

## Prerequisites

To run the UI tests locally, you need:

1. **.NET 10.0 SDK** with MAUI workloads
   ```bash
   dotnet workload install maui-android
   ```

2. **Android SDK** with emulator or physical device
   ```bash
   dotnet android sdk install
   ```

3. **Appium** and the UiAutomator2 driver
   ```bash
   npm install -g appium
   appium driver install uiautomator2
   ```

4. **Built Spice.Scenarios app**
   ```bash
   dotnet build samples/Spice.Scenarios/Spice.Scenarios.csproj -f net10.0-android
   ```

## Running Tests Locally

### 1. Start an Android Emulator or Connect a Device

Create and start an emulator:
```bash
dotnet android avd create --name TestEmulator --sdk "system-images;android-34;google_apis;x86_64"
dotnet android avd start --name TestEmulator
```

Or ensure a physical device is connected:
```bash
adb devices
```

### 2. Install the Spice.Scenarios App

```bash
dotnet android device install --package samples/Spice.Scenarios/bin/Debug/net10.0-android/com.companyname.spice.scenarios-Signed.apk
```

### 3. Start Appium Server

In a separate terminal:
```bash
appium server
```

### 4. Run the Tests

```bash
dotnet test tests/Spice.UITests/Spice.UITests.csproj
```

To run a specific test:
```bash
dotnet test tests/Spice.UITests/Spice.UITests.csproj --filter "FullyQualifiedName~ButtonTests"
```

## CI/CD

The UI tests run automatically in CI on every pull request using:
- Ubuntu runner (latest)
- Android SDK with system-images;android-34
- Appium with UiAutomator2 driver
- GitHub Actions workflow defined in `.github/workflows/spice.yml`

## Test Artifacts

When tests fail, diagnostic information is captured:
- Screenshots: `test-artifacts/{TestName}-{Timestamp}/{TestName}-screenshot.png`
- Logcat output: `test-artifacts/{TestName}-{Timestamp}/{TestName}-logcat.txt`

These artifacts are automatically uploaded in CI and can be downloaded from the workflow run.

## Architecture

### BaseTest.cs
Base class for all UI tests providing:
- Android driver initialization with Appium
- Automatic test failure diagnostics (screenshots + logcat)
- Package and activity configuration for Spice.Scenarios app

### Individual Test Classes
Each control has a dedicated test class (e.g., `ButtonTests.cs`, `SwitchTests.cs`) with tests verifying:
- Control rendering
- User interactions
- Property changes
- State management

Tests navigate through the Spice.Scenarios app menu to reach each control's scenario page.

## Writing New Tests

To add tests for a new control:

1. Create a scenario in `samples/Spice.Scenarios/Scenarios/`
2. Add the scenario to the menu in `samples/Spice.Scenarios/App.cs`
3. Create a test class in `tests/Spice.UITests/` (e.g., `MyControlTests.cs`)
4. Inherit from `BaseTest` and use `RunTest()` to wrap test logic
5. Use `FindButtonByText()`, `FindTextViewContaining()`, or class name selectors
6. Failure diagnostics (screenshots, logcat, page source) are captured automatically

Example:
```csharp
namespace Spice.UITests;

public class MyControlTests : BaseTest
{
    [Fact]
    public void MyControl_Should_DoSomething() => RunTest(() =>
    {
        var button = FindButtonByText("MyControl");
        button.Click();

        var element = FindTextViewContaining("Expected Text");
        Assert.NotNull(element);
    });
}
```

## Troubleshooting

### Appium connection fails
- Ensure Appium server is running on `http://127.0.0.1:4723`
- Check that the emulator/device is visible with `adb devices`

### App doesn't launch
- Verify the app is installed: `adb shell pm list packages | grep spice.scenarios`
- Check the activity name matches in `BaseTest.cs`

### Tests are flaky
- Increase wait times if elements aren't found immediately
- Use explicit waits with `WebDriverWait` for dynamic content
- Check logcat output for app crashes or errors
