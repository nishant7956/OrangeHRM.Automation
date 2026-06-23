using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRM.Framework.Pages;

public abstract class BasePage
{
    protected BasePage(IWebDriver driver, TestSettings settings)
    {
        Driver = driver;
        Settings = settings;
        Waiter = new Waiter(driver, TimeSpan.FromSeconds(settings.TimeoutSeconds));
    }

    protected IWebDriver Driver { get; }

    protected TestSettings Settings { get; }

    protected Waiter Waiter { get; }

    protected void NavigateTo(string relativePath)
    {
        var baseUri = new Uri(Settings.OrangeHrmBaseUrl.TrimEnd('/') + "/");
        Driver.Navigate().GoToUrl(new Uri(baseUri, relativePath.TrimStart('/')));
    }

    protected void Click(By locator)
    {
        Waiter.Clickable(locator).Click();
    }

    protected void Type(By locator, string value, bool clear = true)
    {
        var element = Waiter.Visible(locator);
        if (clear)
        {
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Backspace);
        }

        element.SendKeys(value);
    }

    protected string TextOf(By locator)
    {
        return Waiter.Visible(locator).Text.Trim();
    }

    protected bool IsVisible(By locator)
    {
        try
        {
            return Waiter.Visible(locator).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    protected void SelectAutocompleteOption(string inputText)
    {
        var option = By.XPath($"//div[@role='option']//span[contains(normalize-space(), {XPathLiteral(inputText)})]");
        Waiter.Clickable(option).Click();
    }

    /// <summary>Waits until the browser reports document.readyState == 'complete'.</summary>
    protected void WaitForPageLoad(TimeSpan? timeout = null)
    {
        var wait = timeout.HasValue
            ? new WebDriverWait(Driver, timeout.Value)
            : new WebDriverWait(Driver, TimeSpan.FromSeconds(Settings.TimeoutSeconds));

        wait.Until(driver =>
            ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState") as string == "complete");
    }

    /// <summary>Scrolls the element matching <paramref name="locator"/> into the viewport.</summary>
    protected void ScrollIntoView(By locator)
    {
        var element = Driver.FindElement(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'})", element);
    }

    /// <summary>Selects an option in a native HTML &lt;select&gt; by visible text.</summary>
    protected void SelectDropdown(By locator, string visibleText)
    {
        var select = new SelectElement(Waiter.Visible(locator));
        select.SelectByText(visibleText);
    }

    public static string XPathLiteral(string value)
    {
        if (!value.Contains('\''))
        {
            return $"'{value}'";
        }

        if (!value.Contains('"'))
        {
            return $"\"{value}\"";
        }

        return "concat('" + value.Replace("'", "',\"'\",'") + "')";
    }
}
