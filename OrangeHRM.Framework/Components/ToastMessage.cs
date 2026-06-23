using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Support;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Components;

public sealed class ToastMessage
{
    private readonly Waiter _waiter;

    private static readonly By Toast = By.CssSelector(".oxd-toast");

    public ToastMessage(IWebDriver driver, TestSettings settings)
    {
        _waiter = new Waiter(driver, TimeSpan.FromSeconds(settings.TimeoutSeconds));
    }

    public string Text()
    {
        return _waiter.Visible(Toast).Text.Trim();
    }
}
