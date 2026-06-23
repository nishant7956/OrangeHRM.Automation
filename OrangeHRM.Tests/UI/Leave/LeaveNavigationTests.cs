using Allure.NUnit;
using Allure.NUnit.Attributes;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Framework.Support;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Leave;

[TestFixture]
[AllureNUnit]
[AllureFeature("Leave - Module Navigation")]
[Category("UI")]
[Category("Regression")]
[Category("Leave")]
public sealed class LeaveNavigationTests : BaseUiTest
{
    [Test]
    [AllureDescription("Navigating to the Leave module via the sidebar should display the Leave List page header.")]
    public void NavigateToLeave_ShouldShowLeaveListHeader()
    {
        LoginAsAdmin();

        TestLogger.Step("Opening the Leave module via sidebar");
        var leavePage = new LeavePage(Driver, Settings).Open();

        TestLogger.Step("Verifying the Leave List header is visible");
        leavePage.IsLoaded().Should().BeTrue(
            because: "clicking Leave in the sidebar should load the Leave List view");
    }

    [Test]
    [AllureDescription("The Leave List page should show a Search button indicating filter controls are present.")]
    public void LeaveList_ShouldHaveSearchFilters()
    {
        LoginAsAdmin();

        TestLogger.Step("Opening the Leave module");
        var leavePage = new LeavePage(Driver, Settings).Open();

        TestLogger.Step("Verifying the Search filter panel is present");
        leavePage.HasSearchFilters().Should().BeTrue(
            because: "the Leave List page must include search filter controls");
    }
}
