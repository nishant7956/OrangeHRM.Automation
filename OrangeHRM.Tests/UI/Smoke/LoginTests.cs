using Allure.NUnit;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Smoke;

[TestFixture]
[AllureNUnit]
[Category("UI")]
[Category("Smoke")]
public sealed class LoginTests : BaseUiTest
{
    [Test]
    public void ValidLogin_ShouldOpenDashboard()
    {
        var dashboard = LoginAsAdmin();

        dashboard.IsLoaded().Should().BeTrue();
    }

    [Test]
    public void InvalidLogin_ShouldShowValidationMessage()
    {
        var loginPage = new LoginPage(Driver, Settings).Open();

        loginPage.LoginAs("invalid-user", "invalid-password");

        loginPage.ErrorMessage().Should().Be("Invalid credentials");
    }

    [Test]
    public void Logout_ShouldReturnToLoginPage()
    {
        var dashboard = LoginAsAdmin();

        var loginPage = dashboard.Logout();

        loginPage.IsLoaded().Should().BeTrue();
    }
}
