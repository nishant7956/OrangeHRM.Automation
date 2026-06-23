using OrangeHRM.Framework.Components;
using OrangeHRM.Framework.Config;
using OpenQA.Selenium;

namespace OrangeHRM.Framework.Pages;

public sealed class RecruitmentPage : BasePage
{
    private static readonly By CandidatesHeader = By.XPath("//h5[normalize-space()='Candidates']");

    public RecruitmentPage(IWebDriver driver, TestSettings settings) : base(driver, settings)
    {
    }

    public RecruitmentPage Open()
    {
        new SidebarMenu(Driver, Settings).OpenModule("Recruitment");
        Waiter.Visible(CandidatesHeader);
        return this;
    }

    public bool HasCandidateSearchFilters()
    {
        return new SearchFilterPanel(Driver, Settings).HasSearchButton();
    }
}
