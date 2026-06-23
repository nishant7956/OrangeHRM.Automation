using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Support;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Components;

public sealed class SearchFilterPanel
{
    private readonly Waiter _waiter;

    public SearchFilterPanel(IWebDriver driver, TestSettings settings)
    {
        _waiter = new Waiter(driver, TimeSpan.FromSeconds(settings.TimeoutSeconds));
    }

    public void Search()
    {
        _waiter.Clickable(Button("Search")).Click();
    }

    public void Reset()
    {
        _waiter.Clickable(Button("Reset")).Click();
    }

    public bool HasSearchButton()
    {
        try
        {
            return _waiter.Visible(Button("Search")).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    private static By Button(string text)
    {
        return By.XPath($"//button[normalize-space()='{text}']");
    }
}
