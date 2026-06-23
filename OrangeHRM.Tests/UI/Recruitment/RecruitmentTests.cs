using Allure.NUnit;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Recruitment;

[TestFixture]
[AllureNUnit]
[Category("UI")]
[Category("Regression")]
[Category("Recruitment")]
public sealed class RecruitmentTests : BaseUiTest
{
    [Test]
    public void CandidateList_ShouldExposeSearchFilters()
    {
        LoginAsAdmin();

        var recruitmentPage = new RecruitmentPage(Driver, Settings).Open();

        recruitmentPage.HasCandidateSearchFilters().Should().BeTrue();
    }
}
