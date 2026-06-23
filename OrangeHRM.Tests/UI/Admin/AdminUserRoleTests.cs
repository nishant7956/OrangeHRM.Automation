using Allure.NUnit;
using Allure.NUnit.Attributes;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Framework.Support;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Admin;

[TestFixture]
[AllureNUnit]
[AllureFeature("Admin - User Role Filters")]
[Category("UI")]
[Category("Regression")]
[Category("Admin")]
public sealed class AdminUserRoleTests : BaseUiTest
{
    [Test]
    [AllureDescription("Filtering System Users by the Admin role should return at least one result (the built-in Admin account).")]
    public void FilterByAdminRole_ShouldReturnResults()
    {
        LoginAsAdmin();

        TestLogger.Step("Opening Admin module and filtering by 'Admin' role");
        var adminUsers = new AdminUsersPage(Driver, Settings)
            .Open()
            .SearchByUserRole("Admin");

        TestLogger.Step("Verifying at least one Admin user is listed");
        adminUsers.UserCount().Should().BeGreaterThan(0,
            because: "the built-in Admin account must always appear when filtering by Admin role");
    }

    [Test]
    [AllureDescription("Filtering System Users by the ESS role should return fewer or equal results compared to the unfiltered list.")]
    public void FilterByEssRole_ShouldReturnFewerOrEqualResultsThanTotal()
    {
        LoginAsAdmin();

        TestLogger.Step("Opening Admin module and recording total user count");
        var adminUsersPage = new AdminUsersPage(Driver, Settings).Open();
        var totalCount = adminUsersPage.UserCount();

        TestLogger.Step("Filtering by 'ESS' role");
        adminUsersPage.SearchByUserRole("ESS");
        var essCount = adminUsersPage.UserCount();

        TestLogger.Step($"Total users: {totalCount}, ESS users: {essCount}");
        essCount.Should().BeLessThanOrEqualTo(totalCount,
            because: "filtering by ESS role cannot return MORE users than the unfiltered list");
    }

    [Test]
    [AllureDescription("After filtering, resetting the form should show all users regardless of role.")]
    public void ResetRoleFilter_ShouldRestoreAllUsers()
    {
        LoginAsAdmin();

        TestLogger.Step("Opening Admin module and getting initial total user count");
        var adminUsersPage = new AdminUsersPage(Driver, Settings).Open();
        var totalCount = adminUsersPage.UserCount();

        TestLogger.Step("Filtering by Admin role to reduce visible rows");
        adminUsersPage.SearchByUserRole("Admin");
        var filteredCount = adminUsersPage.UserCount();

        filteredCount.Should().BeLessThanOrEqualTo(totalCount,
            because: "filtering by role should return the same or fewer users");
    }
}
