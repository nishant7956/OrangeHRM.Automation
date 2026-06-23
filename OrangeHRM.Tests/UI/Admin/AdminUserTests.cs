using Allure.NUnit;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Admin;

[TestFixture]
[AllureNUnit]
[Category("UI")]
[Category("Regression")]
[Category("Admin")]
public sealed class AdminUserTests : BaseUiTest
{
    [Test]
    public void SearchAdminUser_ShouldShowAdminInResults()
    {
        LoginAsAdmin();

        var adminUsers = new AdminUsersPage(Driver, Settings)
            .Open()
            .SearchByUsername("Admin");

        adminUsers.HasUser("Admin").Should().BeTrue();
    }

    [Test]
    public void SaveBlankUser_ShouldShowRequiredValidationMessages()
    {
        LoginAsAdmin();

        var adminUsers = new AdminUsersPage(Driver, Settings)
            .Open()
            .StartAddingUser()
            .SaveBlankUser();

        adminUsers.RequiredFieldMessageCount().Should().BeGreaterThan(0);
    }
}
