using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Spice.UITests;

public abstract class BaseTest : IDisposable
{
    protected AndroidDriver Driver { get; private set; } = null!;

    public string PackageName { get; } = "com.companyname.spice.scenarios";
    public string ActivityName { get; } = "com.companyname.spice.scenarios.MainActivity";

    private static readonly string ArtifactsPath = GetArtifactsPath();

    private static string GetArtifactsPath()
    {
        // Use ARTIFACTS_DIR env var if set (CI), otherwise fall back to repo-root test-artifacts/
        var envPath = Environment.GetEnvironmentVariable("ARTIFACTS_DIR");
        if (!string.IsNullOrEmpty(envPath))
            return envPath;

        string? assemblyLocation = Assembly.GetExecutingAssembly().Location;
        if (string.IsNullOrEmpty(assemblyLocation))
            return Path.Combine(Environment.CurrentDirectory, "test-artifacts");

        string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(assemblyDirectory))
            return Path.Combine(Environment.CurrentDirectory, "test-artifacts");

        // Navigate up from bin/Debug/net10.0/ to repo root
        string artifactsPath = Path.Combine(assemblyDirectory, "..", "..", "..", "..", "test-artifacts");
        return Path.GetFullPath(artifactsPath);
    }

    static BaseTest()
    {
        // Ensure artifacts directory exists
        Directory.CreateDirectory(ArtifactsPath);
        Console.WriteLine($"Created artifacts directory: {ArtifactsPath}");
    }

    protected void InitializeAndroidDriver()
    {
        var options = new AppiumOptions();

        // Basic Android capabilities
        options.AddAdditionalAppiumOption("platformName", "Android");
        options.AutomationName = "UiAutomator2";
        options.AddAdditionalAppiumOption("appPackage", PackageName);
        options.AddAdditionalAppiumOption("appActivity", ActivityName);
        options.AddAdditionalAppiumOption("appium:newCommandTimeout", 300);
        options.AddAdditionalAppiumOption("appium:connectHardwareKeyboard", true);
        options.AddAdditionalAppiumOption("appium:autoGrantPermissions", true);

        // Create driver with default Appium server URL, retrying on failure
        var serverUri = new Uri("http://127.0.0.1:4723");
        const int maxRetries = 3;
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Driver = new AndroidDriver(serverUri, options);
                break;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                Console.WriteLine($"Driver init attempt {attempt} failed: {ex.Message}. Retrying...");
                Thread.Sleep(3000);
            }
        }

        // Configure implicit wait timeout - tells the driver to poll the DOM for up to 10 seconds
        // when trying to find elements. This helps handle elements that may not be immediately available.
        // Reference: http://appium.io/docs/en/latest/quickstart/test-dotnet/
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// Finds a button by text, scrolling if needed. Android's default button style
    /// transforms text to uppercase, so this matches using the uppercased text.
    /// </summary>
    protected AppiumElement FindButtonByText(string text)
    {
        var upper = text.ToUpperInvariant();
        var uiSelector = $"new UiSelector().className(\"android.widget.Button\").text(\"{upper}\")";
        try
        {
            return Driver.FindElement(MobileBy.AndroidUIAutomator(uiSelector));
        }
        catch
        {
            // If not immediately visible, try scrolling into view
            return Driver.FindElement(MobileBy.AndroidUIAutomator(
                $"new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView({uiSelector})"));
        }
    }

    /// <summary>
    /// Finds a TextView by partial text match.
    /// </summary>
    protected AppiumElement FindTextViewContaining(string text)
    {
        return Driver.FindElement(MobileBy.AndroidUIAutomator(
            $"new UiSelector().className(\"android.widget.TextView\").textContains(\"{text}\")"));
    }

    /// <summary>
    /// Runs a test body with automatic driver initialization and failure diagnostics.
    /// Retries up to 2 additional times on failure to handle emulator flakiness.
    /// </summary>
    protected void RunTest(Action testBody, [CallerMemberName] string testName = "")
    {
        const int maxAttempts = 3;
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                if (attempt > 1)
                {
                    Console.WriteLine($"Test {testName}: retry attempt {attempt}/{maxAttempts}");
                    try { Driver?.Quit(); } catch { }
                    Thread.Sleep(2000);
                }

                InitializeAndroidDriver();
                testBody();
                return; // Test passed
            }
            catch (Exception ex)
            {
                lastException = ex;
                if (attempt == maxAttempts)
                {
                    CaptureTestFailureDiagnostics(ex, testName);
                    throw;
                }
                Console.WriteLine($"Test {testName} attempt {attempt} failed: {ex.Message}");
            }
        }
    }

    void CaptureTestFailureDiagnostics(Exception testException, [CallerMemberName] string testName = "")
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var testArtifactDir = Path.Combine(ArtifactsPath, $"{testName}-{timestamp}");
        Directory.CreateDirectory(testArtifactDir);

        try
        {
            // Log the exception that caused the failure
            var exceptionPath = Path.Combine(testArtifactDir, $"{testName}-exception.txt");
            File.WriteAllText(exceptionPath, testException.ToString());
            Console.WriteLine($"Test {testName} failed: {testException.Message}");

            CaptureScreenshot(testArtifactDir, testName);
            CaptureLogcat(testArtifactDir, testName);

            try
            {
                var source = Driver.PageSource;
                File.WriteAllText(Path.Combine(testArtifactDir, $"{testName}-page-source.xml"), source);
                Console.WriteLine($"Page source for {testName}:\n{source}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture page source: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to capture diagnostics for test {testName}: {ex.Message}");
        }
    }

    private void CaptureScreenshot(string artifactDir, string testName)
    {
        try
        {
            if (Driver?.SessionId != null)
            {
                var screenshot = Driver.GetScreenshot();
                var screenshotPath = Path.Combine(artifactDir, $"{testName}-screenshot.png");
                screenshot.SaveAsFile(screenshotPath);
                Console.WriteLine($"Screenshot saved: {screenshotPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
        }
    }

    private void CaptureLogcat(string artifactDir, string testName)
    {
        try
        {
            var logcatPath = Path.Combine(artifactDir, $"{testName}-logcat.txt");

            // Run adb logcat command to capture recent logs
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "adb",
                Arguments = "logcat -d -v time",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process != null)
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    File.WriteAllText(logcatPath, output);
                    Console.WriteLine($"Logcat saved: {logcatPath}");
                }
                else
                {
                    Console.WriteLine($"adb logcat failed with exit code {process.ExitCode}: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to capture logcat: {ex.Message}");
        }
    }

    public void Dispose()
    {
        Driver?.Quit();
    }
}
