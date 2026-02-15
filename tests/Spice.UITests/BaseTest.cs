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

        // Create driver with default Appium server URL
        var serverUri = new Uri("http://127.0.0.1:4723");
        Driver = new AndroidDriver(serverUri, options);

        // Configure implicit wait timeout - tells the driver to poll the DOM for up to 10 seconds
        // when trying to find elements. This helps handle elements that may not be immediately available.
        // Reference: http://appium.io/docs/en/latest/quickstart/test-dotnet/
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    protected void CaptureTestFailureDiagnostics([CallerMemberName] string testName = "")
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var testArtifactDir = Path.Combine(ArtifactsPath, $"{testName}-{timestamp}");
        Directory.CreateDirectory(testArtifactDir);

        try
        {
            // Capture screenshot
            CaptureScreenshot(testArtifactDir, testName);

            // Capture logcat output
            CaptureLogcat(testArtifactDir, testName);
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
