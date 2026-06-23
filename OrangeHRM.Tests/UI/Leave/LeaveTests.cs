using Allure.NUnit;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Leave;

[TestFixture]
[AllureNUnit]
[Category("UI")]
[Category("Regression")]
[Category("Leave")]
public sealed class LeaveTests : BaseUiTest
{
    [Test]
    public void LeaveList_ShouldExposeSearchFilters()
    {
        LoginAsAdmin();

        var leavePage = new LeavePage(Driver, Settings).Open();

        leavePage.HasSearchFilters().Should().BeTrue();
    }
}
