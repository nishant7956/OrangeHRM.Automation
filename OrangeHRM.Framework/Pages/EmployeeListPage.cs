using OrangeHRM.Framework.Components;
using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class EmployeeListPage : BasePage
{
    private static readonly By EmployeeNameInput = By.XPath("//label[normalize-space()='Employee Name']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private static readonly By EmployeeListHeader = By.XPath("//h5[normalize-space()='Employee Information']");

    public EmployeeListPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public EmployeeListPage Open()
    {
        NavigateTo("pim/viewEmployeeList");
        Waiter.Visible(EmployeeListHeader);
        return this;
    }

    public EmployeeListPage SearchByEmployeeName(string employeeName)
    {
        Type(EmployeeNameInput, employeeName);
        SelectAutocompleteOption(employeeName);
        new SearchFilterPanel(Driver, Settings).Search();
        return this;
    }

    public bool HasEmployee(string employeeName)
    {
        return new DataTableComponent(Driver, Settings).ContainsText(employeeName);
    }
}
