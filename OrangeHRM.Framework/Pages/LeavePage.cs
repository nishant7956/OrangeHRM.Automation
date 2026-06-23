using OrangeHRM.Framework.Components;
using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class LeavePage : BasePage
{
    private static readonly By LeaveListHeader = By.XPath("//h5[normalize-space()='Leave List']");

    public LeavePage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public LeavePage Open()
    {
        new SidebarMenu(Driver, Settings).OpenModule("Leave");
        Waiter.Visible(LeaveListHeader);
        return this;
    }

    public bool HasSearchFilters()
    {
        return new SearchFilterPanel(Driver, Settings).HasSearchButton();
    }
}
