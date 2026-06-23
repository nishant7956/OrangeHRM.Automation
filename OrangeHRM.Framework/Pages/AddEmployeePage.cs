using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class AddEmployeePage : BasePage
{
    private static readonly By FirstNameInput = By.Name("firstName");
    private static readonly By MiddleNameInput = By.Name("middleName");
    private static readonly By LastNameInput = By.Name("lastName");
    private static readonly By SaveButton = By.CssSelector("button[type='submit']");
    private static readonly By PersonalDetailsHeader = By.XPath("//h6[normalize-space()='Personal Details']");

    public AddEmployeePage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public AddEmployeePage FillName(string firstName, string middleName, string lastName)
    {
        Type(FirstNameInput, firstName);
        Type(MiddleNameInput, middleName);
        Type(LastNameInput, lastName);
        return this;
    }

    public EmployeeDetailsPage Save()
    {
        Click(SaveButton);
        Waiter.Visible(PersonalDetailsHeader);
        return new EmployeeDetailsPage(Driver, Settings);
    }
}
