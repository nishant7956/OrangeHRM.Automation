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

        // Wait for the login attempt to settle. Two outcomes are possible:
        //   • Valid credentials   → browser redirects to /dashboard/ URL
        //   • Invalid credentials → browser stays on login page and shows an alert
        // Waiting for only one outcome causes the other path to block for the
        // full timeout (40 s). This condition exits as soon as either fires.
        Waiter.Until(driver =>
            driver.Url.Contains("/dashboard/") ||
            driver.FindElements(AlertMessage).Any(e => e.Displayed));

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
