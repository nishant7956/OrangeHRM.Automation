using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class DashboardPage : BasePage
{
    private static readonly By DashboardHeader = By.XPath("//h6[normalize-space()='Dashboard']");
    private static readonly By UserDropdown = By.CssSelector(".oxd-userdropdown-tab");
    private static readonly By LogoutLink = By.XPath("//a[normalize-space()='Logout']");

    public DashboardPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public bool IsLoaded()
    {
        return IsVisible(DashboardHeader);
    }

    public LoginPage Logout()
    {
        Click(UserDropdown);
        Click(LogoutLink);
        return new LoginPage(Driver, Settings);
    }
}
