using OrangeHRM.Framework.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace OrangeHRM.Framework.Driver;

public static class DriverFactory
{
    public static IWebDriver Create(TestSettings settings)
    {
        IWebDriver driver = settings.Browser switch
        {
            BrowserType.Edge => CreateEdge(settings),
            BrowserType.Firefox => CreateFirefox(settings),
            _ => CreateChrome(settings)
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);

        if (!settings.Headless)
        {
            driver.Manage().Window.Maximize();
        }

        return driver;
    }

    private static IWebDriver CreateChrome(TestSettings settings)
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        return new ChromeDriver(options);
    }

    private static IWebDriver CreateEdge(TestSettings settings)
    {
        var options = new EdgeOptions();
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--disable-gpu");

        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        return new EdgeDriver(options);
    }

    private static IWebDriver CreateFirefox(TestSettings settings)
    {
        var options = new FirefoxOptions();

        if (settings.Headless)
        {
            options.AddArgument("--headless");
        }

        return new FirefoxDriver(options);
    }
}
