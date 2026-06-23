using OrangeHRM.Framework.Components;
using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class EmployeeListPage : BasePage
{
    private static readonly By EmployeeNameInput = By.XPath("//label[normalize-space()='Employee Name']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private static readonly By EmployeeIdInput = By.XPath("//label[normalize-space()='Employee Id']/ancestor::div[contains(@class,'oxd-input-group')]//input");
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

    /// <summary>Searches the employee list by exact Employee ID string.</summary>
    public EmployeeListPage SearchByEmployeeId(string employeeId)
    {
        Type(EmployeeIdInput, employeeId);
        new SearchFilterPanel(Driver, Settings).Search();
        return this;
    }

    /// <summary>Returns true when the table shows the "No Records Found" state.</summary>
    public bool NoResultsVisible() => new DataTableComponent(Driver, Settings).NoResultsVisible();

    /// <summary>Returns the number of rows currently visible in the employee table.</summary>
    public int EmployeeCount() => new DataTableComponent(Driver, Settings).RowCount();

    /// <summary>
    /// Types a name into the Employee Name field and submits the search WITHOUT
    /// selecting an autocomplete option. Use this for negative/no-results tests.
    /// </summary>
    public EmployeeListPage SearchByEmployeeName_NoAutocomplete(string employeeName)
    {
        Type(EmployeeNameInput, employeeName);
        new SearchFilterPanel(Driver, Settings).Search();
        return this;
    }

    /// <summary>Clicks the Reset button to clear all active search filters.</summary>
    public EmployeeListPage ResetFilter()
    {
        new SearchFilterPanel(Driver, Settings).Reset();
        return this;
    }
}
