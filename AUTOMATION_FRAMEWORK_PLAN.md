# OrangeHRM Selenium C# Automation Framework Plan

## Summary

Build a GitHub-ready proof-of-concept test automation framework using C#, Selenium WebDriver, and NUnit. The framework uses OrangeHRM as the main UI automation target and Restful Booker as the API testing target.

The goal is not only to test one demo site, but to show that the framework can be reused for future sample projects from testing/demo websites. This makes it suitable as portfolio proof for GitHub, interviews, freelance proposals, or user-testing style project demos.

## Key Architecture

- Use Page Object Model for UI automation:
  - `BasePage`
  - `LoginPage`
  - `DashboardPage`
  - `PimPage`
  - `AdminUsersPage`
  - `LeavePage`
  - `RecruitmentPage`
- Use reusable UI components:
  - Sidebar/menu component
  - Data table component
  - Toast/notification component
  - Search/filter component
- Keep Selenium logic inside framework/page classes, not inside test methods.
- Add a separate API layer for Restful Booker:
  - API client
  - Request/response models
  - Auth helper
- Design the framework so another demo site can be added later without rewriting core driver, config, wait, reporting, or CI logic.

## TODO Tasks

### Repository Setup

- [x] Remove current `WebApplication1` starter project.
- [x] Create solution: `OrangeHRM.Automation.sln`.
- [x] Create projects:
  - `OrangeHRM.Framework`
  - `OrangeHRM.Tests`
- [x] Add packages:
  - Selenium WebDriver
  - Selenium Support
  - NUnit
  - NUnit3TestAdapter
  - Microsoft.NET.Test.Sdk
  - FluentAssertions
  - Bogus
  - RestSharp
  - Allure.NUnit

### Framework Foundation

- [x] Create configuration system for base URL, browser, headless mode, timeout, and credentials.
- [x] Create `DriverFactory`.
- [x] Create driver lifecycle setup/teardown.
- [x] Create wait helpers.
- [x] Create screenshot-on-failure support.
- [x] Create test data generator for unique employee/candidate names.
- [x] Add category support: `Smoke`, `Regression`, `UI`, `API`.

### OrangeHRM UI Tests

- [x] Login with valid credentials.
- [x] Validate invalid login message.
- [x] Verify dashboard loads.
- [x] Logout successfully.
- [x] Add employee in PIM.
- [x] Search employee.
- [x] Validate Admin user search.
- [x] Validate Leave page filters.
- [x] Validate Recruitment candidate search filters.
- [ ] Add candidate workflow after confirming it remains stable on the shared public demo site.
- [ ] Add employee delete cleanup after confirming the shared demo state is stable enough for cleanup.

### Restful Booker API Tests

- [x] Create auth token.
- [x] Create booking.
- [x] Get booking.
- [x] Update booking.
- [x] Partial update booking.
- [x] Delete booking.
- [x] Add negative API test.

### GitHub Actions

- [x] Add workflow for automatic test execution.
- [x] Run tests on:
  - Push to `main`
  - Pull request to `main`
  - Manual trigger
  - Scheduled weekday run
- [x] Upload test results, screenshots, logs, and reports as artifacts.
- [ ] Add CI badge after the repository is created on GitHub and the final workflow badge URL is known.

### Documentation

- [x] Create `AUTOMATION_FRAMEWORK_PLAN.md` with this plan.
- [x] Create `README.md` explaining:
  - Purpose of the framework
  - Why OrangeHRM was selected
  - Why Restful Booker is used for API tests
  - Architecture
  - POM structure
  - How to run tests locally
  - How GitHub Actions runs tests automatically
  - How this can be adapted to future demo/client projects

## Test Plan

- [x] Run `dotnet restore`.
- [x] Run `dotnet build`.
- [x] Run API tests.
- [x] Run UI smoke tests locally in headed mode.
- [x] Run UI smoke tests locally in headless mode.
- [x] Run full regression suite.
- [ ] Push to GitHub and confirm Actions run automatically.
- [ ] Trigger GitHub Actions manually.
- [x] Confirm scheduled workflow is configured using GitHub Actions UTC cron.

## Assumptions

- Use Selenium + C# because it clearly demonstrates framework design.
- Use NUnit as the test runner.
- Use OrangeHRM for UI automation.
- Use Restful Booker for API automation.
- Use Page Object Model as the main UI design pattern.
- Keep this as a portfolio/PoC framework that can later support more demo websites.
- Do not initialize Git or commit from Codex; the user will control repository setup.
