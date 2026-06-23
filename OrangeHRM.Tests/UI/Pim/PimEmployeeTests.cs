using Allure.NUnit;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Framework.TestData;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Pim;

[TestFixture]
[AllureNUnit]
[Category("UI")]
[Category("Regression")]
[Category("PIM")]
public sealed class PimEmployeeTests : BaseUiTest
{
    [Test]
    public void AddEmployee_ShouldCreateEmployeeProfile()
    {
        var employee = TestDataGenerator.UniquePerson();
        LoginAsAdmin();

        var detailsPage = new PimPage(Driver, Settings)
            .Open()
            .GoToAddEmployee()
            .FillName(employee.FirstName, employee.MiddleName, employee.LastName)
            .Save();

        detailsPage.IsLoaded().Should().BeTrue();
    }

    [Test]
    public void SearchEmployee_ShouldFindCreatedEmployee()
    {
        var employee = TestDataGenerator.UniquePerson();
        LoginAsAdmin();

        new PimPage(Driver, Settings)
            .Open()
            .GoToAddEmployee()
            .FillName(employee.FirstName, employee.MiddleName, employee.LastName)
            .Save();

        var employeeList = new EmployeeListPage(Driver, Settings)
            .Open()
            .SearchByEmployeeName(employee.FirstName);

        employeeList.HasEmployee(employee.FirstName).Should().BeTrue();
    }
}
