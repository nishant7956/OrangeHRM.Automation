# OrangeHRM Selenium C# Automation Framework

Proof-of-concept test automation framework built with C#, Selenium WebDriver, NUnit, and GitHub Actions.

This project is designed as a GitHub portfolio framework. It demonstrates how to structure browser UI tests with Page Object Model, reusable framework utilities, API tests, CI execution, reporting artifacts, and documentation that can be adapted to future demo/client testing projects.

## Test Targets

### OrangeHRM UI

OrangeHRM is used as the main UI automation target because it behaves like a real enterprise HR application. It includes authentication, dashboard navigation, side menus, forms, tables, search filters, validation messages, and HR workflows such as PIM, Admin, Leave, and Recruitment.

Demo site:

```text
https://opensource-demo.orangehrmlive.com/
```

Default public demo credentials:

```text
Username: Admin
Password: admin123
```

### Restful Booker API

Restful Booker is used for API automation because it provides a clean public API contract for authentication and CRUD-style booking tests.

API site:

```text
https://restful-booker.herokuapp.com/
```

OrangeHRM is intentionally not used as the primary API target because its backend endpoints are internal UI endpoints and are not meant to be a stable public API contract.

## Architecture

```text
OrangeHRM.Framework/
  Api/
  Components/
  Config/
  Driver/
  Pages/
  Support/
  TestData/

OrangeHRM.Tests/
  Api/
  Hooks/
  UI/
    Admin/
    Leave/
    Pim/
    Recruitment/
    Smoke/
```

## Page Object Model

The UI layer follows Page Object Model.

Page classes expose business actions:

```csharp
new LoginPage(Driver, Settings)
    .Open()
    .LoginAs(username, password);
```

Tests stay readable and do not contain Selenium locator logic. Locators, waits, clicks, typing, and page-specific behavior live in the framework project.

Key page/component classes:

- `BasePage`
- `LoginPage`
- `DashboardPage`
- `PimPage`
- `AddEmployeePage`
- `EmployeeListPage`
- `AdminUsersPage`
- `LeavePage`
- `RecruitmentPage`
- `SidebarMenu`
- `DataTableComponent`
- `SearchFilterPanel`

## Configuration

Configuration is read from environment variables.

| Variable | Default |
| --- | --- |
| `ORANGEHRM_BASE_URL` | `https://opensource-demo.orangehrmlive.com/web/index.php` |
| `ORANGEHRM_USERNAME` | `Admin` |
| `ORANGEHRM_PASSWORD` | `admin123` |
| `BOOKER_BASE_URL` | `https://restful-booker.herokuapp.com` |
| `BROWSER` | `Chrome` |
| `HEADLESS` | `false` locally, `true` in CI |
| `TIMEOUT_SECONDS` | `20` |
| `TEST_ARTIFACTS_DIR` | `TestResults` |

## Run Locally

Restore and build:

```powershell
dotnet restore OrangeHRM.Automation.sln
dotnet build OrangeHRM.Automation.sln
```

Run API tests:

```powershell
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=API"
```

Run UI smoke tests in headed mode:

```powershell
$env:HEADLESS = "false"
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=Smoke"
```

Run UI smoke tests in headless mode:

```powershell
$env:HEADLESS = "true"
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=Smoke"
```

Run regression tests:

```powershell
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=Regression"
```

## GitHub Actions

The workflow in `.github/workflows/tests.yml` runs on:

- push to `main`
- pull request to `main`
- manual trigger
- weekday schedule at 10:30 UTC, which is 6:30 AM Toronto during daylight saving time

Default push/PR runs execute smoke UI tests plus API tests. Scheduled runs execute regression plus API tests. Manual runs allow selecting `smoke`, `api`, `regression`, or `all`.

Test artifacts are uploaded from `TestResults`, including TRX output and failure screenshots.

## Portfolio/PoC Usage

This framework is intentionally reusable. To adapt it for another demo or client-style project:

1. Add a new page object namespace for the new site.
2. Keep using the existing driver, wait, config, screenshot, and CI layers.
3. Add new test categories for the new project.
4. Keep test logic business-readable and keep selectors inside page objects/components.

That keeps the framework valuable beyond one demo website and makes it easier to show as proof of framework understanding.

## Known Limitations

- OrangeHRM demo is a shared public site, so data may change or reset.
- Tests create unique records to reduce collisions.
- Destructive cleanup flows are intentionally limited until the shared demo state is confirmed stable.
- Git has not been initialized or committed by Codex; repository setup is left to the user.
