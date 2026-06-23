using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class LoginPage : BasePage
{
    private static readonly By UsernameInput = By.Name("username");
    private static readonly By PasswordInput = By.Name("password");
    private static readonly By LoginButton = By.CssSelector("button[type='submit']");
    private static readonly By AlertMessage = By.CssSelector(".oxd-alert-content-text");

    public LoginPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public LoginPage Open()
    {
        NavigateTo("auth/login");
        Waiter.Visible(UsernameInput);
        return this;
    }

    public DashboardPage LoginAs(string username, string password)
    {
        Type(UsernameInput, username);
        Type(PasswordInput, password);
        Click(LoginButton);
        return new DashboardPage(Driver, Settings);
    }

    public string ErrorMessage()
    {
        return TextOf(AlertMessage);
    }

    public bool IsLoaded()
    {
        return IsVisible(UsernameInput) && IsVisible(LoginButton);
    }
}
