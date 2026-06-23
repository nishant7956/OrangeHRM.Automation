using OrangeHRM.Framework.Components;
using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class PimPage : BasePage
{
    private static readonly By AddEmployeeTab = By.XPath("//a[normalize-space()='Add Employee']");
    private static readonly By EmployeeListHeader = By.XPath("//h5[normalize-space()='Employee Information']");

    public PimPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public PimPage Open()
    {
        new SidebarMenu(Driver, Settings).OpenModule("PIM");
        Waiter.Visible(EmployeeListHeader);
        return this;
    }

    public AddEmployeePage GoToAddEmployee()
    {
        Click(AddEmployeeTab);
        return new AddEmployeePage(Driver, Settings);
    }
}
