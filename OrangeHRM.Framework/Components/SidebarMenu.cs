using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Components;

public sealed class SidebarMenu
{
    private readonly IWebDriver _driver;
    private readonly TestSettings _settings;

    public SidebarMenu(IWebDriver driver, TestSettings settings)
    {
        _driver = driver;
        _settings = settings;
    }

    public void OpenModule(string moduleName)
    {
        var locator = By.XPath($"//aside//a[.//span[normalize-space()={Pages.BasePage.XPathLiteral(moduleName)}]]");
        new Support.Waiter(_driver, TimeSpan.FromSeconds(_settings.TimeoutSeconds)).Clickable(locator).Click();
    }
}
