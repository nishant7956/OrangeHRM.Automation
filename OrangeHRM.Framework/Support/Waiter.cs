using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRM.Framework.Support;

public sealed class Waiter
{
    private readonly WebDriverWait _wait;

    public Waiter(IWebDriver driver, TimeSpan timeout)
    {
        _wait = new WebDriverWait(driver, timeout);
        _wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
    }

    public IWebElement Visible(By locator)
    {
        return _wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed ? element : null;
        })!;
    }

    public IWebElement Clickable(By locator)
    {
        return _wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed && element.Enabled ? element : null;
        })!;
    }

    public IReadOnlyCollection<IWebElement> AllVisible(By locator)
    {
        return _wait.Until(driver =>
        {
            var elements = driver.FindElements(locator).Where(element => element.Displayed).ToList();
            return elements.Count > 0 ? elements : null;
        })!;
    }

    public bool InvisibilityOf(By locator)
    {
        return _wait.Until(driver =>
        {
            var elements = driver.FindElements(locator);
            return elements.Count == 0 || elements.All(element => !element.Displayed);
        });
    }

    public T Until<T>(Func<IWebDriver, T> condition)
    {
        return _wait.Until(condition);
    }
}
