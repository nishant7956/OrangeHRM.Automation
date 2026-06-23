using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class EmployeeDetailsPage : BasePage
{
    private static readonly By PersonalDetailsHeader = By.XPath("//h6[normalize-space()='Personal Details']");
    private static readonly By NicknameInput = By.XPath("//label[normalize-space()='Nickname']/ancestor::div[contains(@class,'oxd-input-group')]//input");
    private static readonly By SaveButton = By.XPath("(//button[@type='submit'])[1]");

    public EmployeeDetailsPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public bool IsLoaded()
    {
        return IsVisible(PersonalDetailsHeader);
    }

    public EmployeeDetailsPage UpdateNickname(string nickname)
    {
        Type(NicknameInput, nickname);
        Click(SaveButton);
        return this;
    }
}
