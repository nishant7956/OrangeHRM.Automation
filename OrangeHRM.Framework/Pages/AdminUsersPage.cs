using OrangeHRM.Framework.Components;
using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class AdminUsersPage : BasePage
{
    private static readonly By SystemUsersHeader = By.XPath("//h5[normalize-space()='System Users']");
    private static readonly By UsernameInput = By.XPath("//label[normalize-space()='Username']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private static readonly By AddButton = By.XPath("//button[normalize-space()='Add']");
    private static readonly By SaveButton = By.CssSelector("button[type='submit']");
    private static readonly By RequiredMessages = By.XPath("//span[normalize-space()='Required']");

    public AdminUsersPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public AdminUsersPage Open()
    {
        new SidebarMenu(Driver, Settings).OpenModule("Admin");
        Waiter.Visible(SystemUsersHeader);
        return this;
    }

    public AdminUsersPage SearchByUsername(string username)
    {
        Type(UsernameInput, username);
        new SearchFilterPanel(Driver, Settings).Search();
        return this;
    }

    public bool HasUser(string username)
    {
        return new DataTableComponent(Driver, Settings).ContainsText(username);
    }

    public AdminUsersPage StartAddingUser()
    {
        Click(AddButton);
        return this;
    }

    public AdminUsersPage SaveBlankUser()
    {
        Click(SaveButton);
        return this;
    }

    public int RequiredFieldMessageCount()
    {
        return Waiter.AllVisible(RequiredMessages).Count;
    }
}
