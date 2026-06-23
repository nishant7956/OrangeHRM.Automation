using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Driver;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Framework.Support;

namespace OrangeHRM.Tests.Hooks;

public abstract class BaseUiTest
{
    protected IWebDriver Driver { get; private set; } = null!;

    protected TestSettings Settings { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Settings = TestSettings.FromEnvironment();
        Driver = DriverFactory.Create(Settings);
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshot = ScreenshotHelper.SaveFailureScreenshot(
                    Driver,
                    Settings.ArtifactsDirectory,
                    TestContext.CurrentContext.Test.Name);

                if (screenshot is not null)
                {
                    TestContext.AddTestAttachment(screenshot, "Failure screenshot");
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
