using OpenQA.Selenium;

namespace OrangeHRM.Framework.Support;

public static class ScreenshotHelper
{
    public static string? SaveFailureScreenshot(IWebDriver driver, string artifactsDirectory, string testName)
    {
        if (driver is not ITakesScreenshot screenshotDriver)
        {
            return null;
        }

        Directory.CreateDirectory(artifactsDirectory);

        var safeName = string.Concat(testName.Select(character =>
            Path.GetInvalidFileNameChars().Contains(character) ? '_' : character));
        var fileName = $"{safeName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png";
        var path = Path.Combine(artifactsDirectory, fileName);

        screenshotDriver.GetScreenshot().SaveAsFile(path);
        return path;
    }
}
