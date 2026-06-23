# OrangeHRM Selenium C# Automation Framework

[![CI](https://github.com/nishant7956/OrangeHRM.Automation/actions/workflows/tests.yml/badge.svg?branch=main)](https://github.com/nishant7956/OrangeHRM.Automation/actions/workflows/tests.yml)
[![Allure Report](https://img.shields.io/badge/Allure-Report-brightgreen?logo=testcafe)](https://nishant7956.github.io/OrangeHRM.Automation/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![NUnit](https://img.shields.io/badge/NUnit-4.x-009B00?logo=nunit)](https://nunit.org/)
[![Selenium](https://img.shields.io/badge/Selenium-WebDriver-43B02A?logo=selenium)](https://www.selenium.dev/)

> **📊 [Live Allure Report →](https://nishant7956.github.io/OrangeHRM.Automation/)**
>
> Tests run automatically every day at 10:30 UTC.

A production-quality C# test automation framework built as a portfolio proof-of-concept. It demonstrates how to design, implement, and operate a maintainable Selenium WebDriver + API automation suite with a full CI/CD pipeline and daily HTML reports.

---

## Why This Framework (Upwork Clients: Read This)

> If you landed here from an Upwork proposal, this section is for you.

| Capability | What This Project Shows |
|---|---|
| **Page Object Model** | 10+ page classes — zero Selenium code in tests. Locators, waits, and actions live only in the framework. |
| **Fluent API design** | Chainable methods: `new LoginPage(...).Open().LoginAs(u, p)` — tests read like English sentences. |
| **Reusable components** | `DataTableComponent`, `SearchFilterPanel`, `SidebarMenu` — shared across all page tests, not copy-pasted. |
| **Data-driven testing** | `TestDataGenerator` using [Bogus](https://github.com/bchavez/Bogus) — unique, realistic test data every run. |
| **API automation** | Full REST CRUD flow with `RestSharp` + negative cases + query-parameter filter tests. |
| **Framework utilities** | `Waiter` (explicit waits), `RetryHelper` (flake resilience), `TestLogger` (structured step output), `ScreenshotHelper` (auto-capture on failure). |
| **CI/CD** | GitHub Actions with **parallel matrix jobs** (smoke/regression/API), **daily schedule**, and **Allure HTML report** published to GitHub Pages automatically. |
| **Reporting** | [Allure](https://allurereport.org/) with suites, features, steps, and failure screenshots. TRX for Azure DevOps / Visual Studio. |
| **Zero secrets in code** | All credentials read from environment variables. CI uses GitHub Secrets / Actions env. |
| **Documentation** | This README, architecture diagram, run instructions, and a public live report URL. |

This framework is intentionally built to be **reusable across projects**. Plugging in a new application means adding a page object namespace and new test categories — the driver, waits, retry, screenshot, reporting, and CI layers are already done.

---

## Test Targets

### OrangeHRM UI

OrangeHRM is the UI automation target because it behaves like a real enterprise HR application. It includes authentication, dashboard navigation, side menus, forms, tables, search filters, validation messages, and HR workflows: PIM, Admin, Leave, and Recruitment.

```text
https://opensource-demo.orangehrmlive.com/
```

Credentials (public demo):

```text
Username: Admin
Password: admin123
```

### Restful Booker API

Restful Booker is the API automation target. It provides a clean public API contract for authentication, CRUD-style booking operations, and query-parameter filtering.

```text
https://restful-booker.herokuapp.com/
```

OrangeHRM is intentionally not used as the primary API target — its backend endpoints are internal UI calls, not a stable public API contract.

---

## Test Coverage

| Module | Category | Tests |
|---|---|---|
| Login | Smoke | Valid login, invalid login, logout |
| Admin — Users | Regression | Search by username, role filter (Admin/ESS), validation messages |
| PIM — Employees | Regression | Add employee, search by name, search by ID, no-results state, reset filter |
| Leave | Regression | Module navigation, search filter presence |
| Booking API | API | Full CRUD, GET all, name filter, negative filter, compound filter |

**Total: 20+ tests** across UI (headless Chrome) and API.

---

## Architecture

```text
OrangeHRM.Framework/
  Api/
    Models/          — Records for request/response serialization
    RestfulBookerClient.cs — Typed HTTP client (HttpClient, System.Text.Json)
  Components/
    DataTableComponent.cs   — Row assertions, count, action buttons
    SearchFilterPanel.cs    — Search / Reset button interactions
    SidebarMenu.cs          — Module navigation
    ToastMessage.cs         — Success/error toast capture
  Config/
    TestSettings.cs  — Environment-variable config with typed defaults
  Driver/
    DriverFactory.cs — Creates Chrome/Firefox with headless/headed toggle
    BrowserType.cs
  Pages/             — Page Object Model classes (BasePage + 9 page classes)
  Support/
    RetryHelper.cs   — Retry-with-delay for flaky operations
    ScreenshotHelper.cs — Auto-save screenshots on failure
    TestLogger.cs    — Structured timestamped step logging
    Waiter.cs        — Explicit WebDriverWait wrappers

OrangeHRM.Tests/
  Api/
    BookingApiTests.cs        — Full CRUD flow
    BookingFilterApiTests.cs  — GET list + name filter tests
  Hooks/
    BaseUiTest.cs    — SetUp/TearDown: driver init, screenshot, Allure suite
  UI/
    Admin/           — AdminUserTests, AdminUserRoleTests
    Leave/           — LeaveNavigationTests
    Pim/             — PimEmployeeTests, PimSearchFilterTests
    Recruitment/     — RecruitmentTests
    Smoke/           — LoginTests
```

---

## Page Object Model

Page classes expose business actions. Tests never contain Selenium locators:

```csharp
// Test code — reads like a requirement
new LoginPage(Driver, Settings)
    .Open()
    .LoginAs(username, password);

// Searching with no results
new EmployeeListPage(Driver, Settings)
    .Open()
    .SearchByEmployeeName_NoAutocomplete("zzz_nonexistent")
    .NoResultsVisible()
    .Should().BeTrue();
```

Chaining is achieved by returning `this` or the next logical page from every action method.

---

## Framework Utilities

### RetryHelper

Wraps flaky operations with configurable retry count and delay:

```csharp
RetryHelper.Execute(
    () => Click(SubmitButton),
    maxAttempts: 3,
    delay: TimeSpan.FromMilliseconds(500),
    description: "Submit form");
```

### TestLogger

Structured step logging that surfaces in TRX and Allure:

```csharp
TestLogger.Step("Searching for employee by ID");
TestLogger.Warn("Retry triggered — possible loading delay");
TestLogger.Error("Screenshot saved: path/to/screenshot.png");
```

### Waiter

Explicit WebDriverWait wrappers — no `Thread.Sleep`:

```csharp
Waiter.Visible(locator);       // Wait for element to appear
Waiter.Clickable(locator);     // Wait until enabled and visible
Waiter.InvisibilityOf(locator); // Wait for element to disappear
Waiter.AllVisible(locator);    // Wait for a collection of visible elements
```

---

## Configuration

Configuration is read from environment variables with sensible defaults.

| Variable | Default |
|---|---|
| `ORANGEHRM_BASE_URL` | `https://opensource-demo.orangehrmlive.com/web/index.php` |
| `ORANGEHRM_USERNAME` | `Admin` |
| `ORANGEHRM_PASSWORD` | `admin123` |
| `BOOKER_BASE_URL` | `https://restful-booker.herokuapp.com` |
| `BROWSER` | `Chrome` |
| `HEADLESS` | `false` locally, `true` in CI |
| `TIMEOUT_SECONDS` | `20` |
| `TEST_ARTIFACTS_DIR` | `TestResults` |
| `SAVE_ALL_SCREENSHOTS` | `false` (set `true` for demo/debug runs) |

---

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

Run UI smoke tests — headed (you'll see the browser):

```powershell
$env:HEADLESS = "false"
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=Smoke"
```

Run UI smoke tests — headless:

```powershell
$env:HEADLESS = "true"
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=Smoke"
```

Run regression tests:

```powershell
dotnet test OrangeHRM.Automation.sln --filter "TestCategory=Regression"
```

Run everything:

```powershell
dotnet test OrangeHRM.Automation.sln
```

---

## GitHub Actions CI/CD

The workflow in [`.github/workflows/tests.yml`](.github/workflows/tests.yml) runs on:

- **Push / PR to `main`** — smoke + API tests
- **Daily schedule** — every day at 10:30 UTC (6:30 AM Toronto ET during DST) — regression + API
- **Manual trigger** — choose `smoke`, `api`, `regression`, or `all`

### Pipeline Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                   GitHub Actions Workflow                    │
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ smoke tests  │  │  regression  │  │   api tests      │  │
│  │  (parallel)  │  │  (parallel)  │  │   (parallel)     │  │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────────┘  │
│         │                 │                  │              │
│         └─────────────────┴──────────────────┘             │
│                           │                                 │
│                  ┌────────▼────────┐                        │
│                  │  Allure Report  │                        │
│                  │  → GitHub Pages │                        │
│                  │  → Job Summary  │                        │
│                  └─────────────────┘                        │
└─────────────────────────────────────────────────────────────┘
```

Key features:
- **Parallel matrix** — smoke, regression, and API run simultaneously (saves ~40% CI time)
- **fail-fast: false** — all suites always complete, even if one fails
- **Allure HTML report** — published to GitHub Pages after every run
- **Job summary** — pass/fail counts per suite visible directly in the Actions UI
- **Hang timeout** — `--blame-hang-timeout 2m` kills any test stuck for over 2 minutes
- **Concurrency control** — in-progress runs cancelled when a new push arrives

---

## Known Limitations

- OrangeHRM demo is a shared public site — data may change or reset between runs.
- Tests create unique records using `TestDataGenerator` to reduce collisions with other users.
- Destructive cleanup flows are intentionally conservative until the shared demo state is confirmed stable.
- The Allure report link requires GitHub Pages to be enabled on your repository (`Settings → Pages → Source: gh-pages branch`).

---

## Adapting to a New Project

This framework is intentionally reusable. To target a new web application:

1. Add a new page object namespace under `OrangeHRM.Framework/Pages/`.
2. Reuse the existing `DriverFactory`, `Waiter`, `RetryHelper`, `ScreenshotHelper`, and `TestLogger`.
3. Add new test categories and test fixtures under `OrangeHRM.Tests/UI/`.
4. The CI pipeline, Allure reporting, and configuration layers require no changes.
