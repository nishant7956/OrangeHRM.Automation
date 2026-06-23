using Allure.NUnit;
using Allure.NUnit.Attributes;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Api;
using OrangeHRM.Framework.Api.Models;
using OrangeHRM.Framework.Config;
using OrangeHRM.Framework.Support;

namespace OrangeHRM.Tests.Api;

/// <summary>
/// Tests covering the GET /booking list and filter endpoints of Restful Booker.
/// These complement the CRUD tests in <see cref="BookingApiTests"/> by validating
/// query-parameter-based filtering — a common pattern in real enterprise APIs.
/// </summary>
[TestFixture]
[AllureNUnit]
[AllureFeature("Booking API - Filters")]
[Category("API")]
public sealed class BookingFilterApiTests
{
    private RestfulBookerClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _client = new RestfulBookerClient(TestSettings.FromEnvironment().RestfulBookerBaseUrl);
        TestLogger.Step("API client initialised");
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    [AllureDescription("GET /booking should return a non-empty list of booking IDs.")]
    public async Task GetAllBookings_ShouldReturnNonEmptyList()
    {
        TestLogger.Step("Calling GET /booking");
        var ids = await _client.GetAllBookingIdsAsync();

        TestLogger.Step($"Received {ids.Count} booking IDs");
        ids.Should().NotBeEmpty(because: "the Restful Booker demo always has pre-seeded bookings");
        ids.Should().OnlyContain(id => id > 0, because: "all booking IDs should be positive integers");
    }

    [Test]
    [AllureDescription("GET /booking?firstname=X should return only bookings with that first name.")]
    public async Task GetBookingsByFirstName_ShouldReturnFilteredResults()
    {
        // Seed a booking with a unique first name so we can filter for it precisely.
        var uniqueFirstName = "FilterTest" + Guid.NewGuid().ToString("N")[..8];
        TestLogger.Step($"Creating a booking with unique first name: {uniqueFirstName}");

        var token = await _client.CreateTokenAsync();
        var created = await _client.CreateBookingAsync(new Booking(
            uniqueFirstName, "FilterLast", 100, false,
            new BookingDates("2026-08-01", "2026-08-05"), "None"));

        try
        {
            TestLogger.Step($"Filtering bookings by firstname={uniqueFirstName}");
            var filtered = await _client.GetBookingsByNameAsync(firstName: uniqueFirstName);

            filtered.Should().Contain(created.BookingId,
                because: "the newly created booking should appear when filtering by its first name");
        }
        finally
        {
            await _client.DeleteBookingAsync(created.BookingId, token);
            TestLogger.Step($"Cleaned up booking {created.BookingId}");
        }
    }

    [Test]
    [AllureDescription("GET /booking?firstname=Unknown should return an empty or minimal list for a nonsense name.")]
    public async Task GetBookingsByNonsenseName_ShouldReturnNoResults()
    {
        var nonsenseName = $"zzz_no_such_guest_{Guid.NewGuid():N}";
        TestLogger.Step($"Filtering by non-existent name: {nonsenseName}");

        var results = await _client.GetBookingsByNameAsync(firstName: nonsenseName);

        TestLogger.Step($"Received {results.Count} result(s)");
        results.Should().BeEmpty(
            because: "a filter for a name that has never been booked should return no results");
    }

    [Test]
    [AllureDescription("GET /booking?firstname=X&lastname=Y should narrow results further than firstname alone.")]
    public async Task GetBookingsByFullName_ShouldBeMoreSpecificThanFirstNameAlone()
    {
        TestLogger.Step("Calling GET /booking with only firstname");
        var byFirstName = await _client.GetBookingsByNameAsync(firstName: "Test");

        TestLogger.Step("Calling GET /booking with firstname AND lastname");
        var byFullName = await _client.GetBookingsByNameAsync(firstName: "Test", lastName: "Test");

        TestLogger.Step($"firstname={byFirstName.Count} | firstname+lastname={byFullName.Count}");
        byFullName.Count.Should().BeLessThanOrEqualTo(byFirstName.Count,
            because: "adding a last-name filter should return the same or fewer results");
    }
}
