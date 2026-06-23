using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Driver;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Framework.Support;

namespace OrangeHRM.Tests.Hooks;

[AllureNUnit]
[AllureSuite("OrangeHRM UI")]
public abstract class BaseUiTest
{
    protected IWebDriver Driver { get; private set; } = null!;

    protected TestSettings Settings { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Settings = TestSettings.FromEnvironment();
        Driver = DriverFactory.Create(Settings);
        TestLogger.Step($"Test starting: {TestContext.CurrentContext.Test.FullName}");
    }

    [TearDown]
    public void TearDown()
    {
        var result = TestContext.CurrentContext.Result;
        TestLogger.Step($"Test finished: {result.Outcome.Status} — {TestContext.CurrentContext.Test.Name}");

        try
        {
            var failed = result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed;
            var saveAll = bool.TryParse(
                Environment.GetEnvironmentVariable("SAVE_ALL_SCREENSHOTS"), out var b) && b;

            if (failed || saveAll)
            {
                var screenshot = ScreenshotHelper.SaveFailureScreenshot(
                    Driver,
                    Settings.ArtifactsDirectory,
                    TestContext.CurrentContext.Test.Name);

                if (screenshot is not null)
                {
                    TestContext.AddTestAttachment(screenshot, failed ? "Failure screenshot" : "Test screenshot");
                    if (failed) TestLogger.Error($"Screenshot saved: {screenshot}");
                }
            }
        }
        finally
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }

    protected DashboardPage LoginAsAdmin()
    {
        return new LoginPage(Driver, Settings)
            .Open()
            .LoginAs(Settings.OrangeHrmUsername, Settings.OrangeHrmPassword);
    }
}
