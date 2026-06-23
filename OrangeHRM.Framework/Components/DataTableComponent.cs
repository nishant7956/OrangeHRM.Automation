using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Support;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Components;

public sealed class DataTableComponent
{
    private readonly IWebDriver _driver;
    private readonly Waiter _waiter;

    private static readonly By TableRows = By.CssSelector(".oxd-table-body .oxd-table-row");

    public DataTableComponent(IWebDriver driver, TestSettings settings)
    {
        _driver = driver;
        _waiter = new Waiter(driver, TimeSpan.FromSeconds(settings.TimeoutSeconds));
    }

    public bool ContainsText(string expectedText)
    {
        try
        {
            return _waiter.Until(driver =>
                driver.FindElements(TableRows).Any(row =>
                    row.Displayed && row.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase)));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public IReadOnlyCollection<string> RowTexts()
    {
        return _driver.FindElements(TableRows)
            .Where(row => row.Displayed)
            .Select(row => row.Text.Trim())
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .ToList();
    }
}
