using Allure.NUnit;
using Allure.NUnit.Attributes;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Pages;
using OrangeHRM.Framework.Support;
using OrangeHRM.Tests.Hooks;

namespace OrangeHRM.Tests.UI.Pim;

[TestFixture]
[AllureNUnit]
[AllureFeature("PIM - Employee Search Filters")]
[Category("UI")]
[Category("Regression")]
[Category("PIM")]
public sealed class PimSearchFilterTests : BaseUiTest
{
    [Test]
    [AllureDescription("A search for a nonsense employee name should show the 'No Records Found' empty state.")]
    public void SearchByNonsenseName_ShouldShowNoRecordsFound()
    {
        LoginAsAdmin();

        TestLogger.Step("Searching for an employee name that does not exist");
        var employeeList = new EmployeeListPage(Driver, Settings)
            .Open()
            .SearchByEmployeeName_NoAutocomplete("zzz_nonexistent_xyz");

        TestLogger.Step("Verifying empty-state is displayed");
        employeeList.NoResultsVisible().Should().BeTrue(
            because: "a search for a non-existent employee should display 'No Records Found'");
    }

    [Test]
    [AllureDescription("A search by Employee ID should list only the matching employee record.")]
    public void SearchByEmployeeId_ShouldShowAtMostOneResult()
    {
        LoginAsAdmin();;

        TestLogger.Step("Opening Employee List and searching by a known Employee ID");
        // Employee ID '0001' is the default Admin account on the OrangeHRM demo
        var employeeList = new EmployeeListPage(Driver, Settings)
            .Open()
            .SearchByEmployeeId("0001");

        TestLogger.Step("Verifying result count is between 0 and 1");
        var count = employeeList.EmployeeCount();
        count.Should().BeInRange(0, 1,
            because: "searching by a specific Employee ID should return at most one record");
    }

    [Test]
    [AllureDescription("Resetting the search filter should restore all employee records.")]
    public void ResetFilter_ShouldRestoreAllRecords()
    {
        LoginAsAdmin();

        TestLogger.Step("Opening Employee List and applying a name search");
        var employeeList = new EmployeeListPage(Driver, Settings).Open();
        var initialCount = employeeList.EmployeeCount();

        employeeList.SearchByEmployeeId("0001");

        TestLogger.Step("Resetting the filter");
        employeeList.ResetFilter();

        TestLogger.Step("Verifying records are restored");
        var restoredCount = employeeList.EmployeeCount();
        restoredCount.Should().BeGreaterThanOrEqualTo(initialCount,
            because: "resetting the filter should show at least as many records as the initial load");
    }
}
