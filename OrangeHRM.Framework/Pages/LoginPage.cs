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

        // Wait for the post-login redirect to complete before handing back
        // the DashboardPage. Without this, IsLoaded() races against the
        // navigation on slow CI runners and times out at 20 s.
        Waiter.Until(driver => driver.Url.Contains("/dashboard/"));

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
