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

    /// <summary>Returns the number of visible rows in the table body.</summary>
    public int RowCount()
    {
        try
        {
            // Wait until at least one row is visible, then count synchronously.
            _waiter.Until(driver =>
                driver.FindElements(TableRows).Any(r => r.Displayed));

            return _driver.FindElements(TableRows).Count(r => r.Displayed);
        }
        catch (WebDriverTimeoutException)
        {
            return 0;
        }
    }

    /// <summary>
    /// Returns true when the OrangeHRM "No Records Found" empty state is visible.
    /// Useful for asserting that a search returned zero results.
    /// </summary>
    public bool NoResultsVisible()
    {
        try
        {
            var emptyState = By.XPath("//span[contains(normalize-space(),'No Records Found')]");
            return _waiter.Visible(emptyState).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// Clicks a labelled action button (e.g., Edit, Delete) within the row whose text
    /// contains <paramref name="rowText"/>.
    /// </summary>
    public void ClickActionOnRow(string rowText, string actionLabel)
    {
        var rows = _driver.FindElements(TableRows)
            .Where(row => row.Displayed && row.Text.Contains(rowText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (rows.Count == 0)
            throw new NoSuchElementException($"No visible table row containing '{rowText}' was found.");

        var actionButton = rows[0].FindElement(
            By.XPath($".//button[@title='{actionLabel}' or normalize-space()='{actionLabel}']"));
        actionButton.Click();
    }
}
