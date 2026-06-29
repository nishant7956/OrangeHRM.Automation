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
        //   • Valid credentials   → Dashboard header h6 becomes visible
        //   • Invalid credentials → browser stays on login page and shows an alert
        // Waiting for the DOM element (not just the URL) ensures the Vue app
        // has fully hydrated the sidebar before the next test step executes.
        Waiter.Until(driver =>
            driver.FindElements(By.XPath("//h6[normalize-space()='Dashboard']")).Any(e => e.Displayed) ||
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
