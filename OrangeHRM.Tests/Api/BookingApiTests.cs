using Allure.NUnit;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using OrangeHRM.Framework.Api;
using OrangeHRM.Framework.Api.Models;
using OrangeHRM.Framework.Config;

namespace OrangeHRM.Tests.Api;

[TestFixture]
[AllureNUnit]
[Category("API")]
public sealed class BookingApiTests
{
    private RestfulBookerClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _client = new RestfulBookerClient(TestSettings.FromEnvironment().RestfulBookerBaseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public async Task BookingCrud_ShouldCreateReadUpdatePatchAndDeleteBooking()
    {
        var token = await _client.CreateTokenAsync();
        var booking = NewBooking("Proof", "Concept", 275, "Breakfast");

        var createResponse = await _client.CreateBookingAsync(booking);
        createResponse.BookingId.Should().BeGreaterThan(0);
        createResponse.Booking.FirstName.Should().Be("Proof");

        var savedBooking = await _client.GetBookingAsync(createResponse.BookingId);
        savedBooking.LastName.Should().Be("Concept");

        var updatedBooking = NewBooking("Framework", "Demo", 325, "Late checkout");
        var updateResponse = await _client.UpdateBookingAsync(createResponse.BookingId, updatedBooking, token);
        updateResponse.FirstName.Should().Be("Framework");
        updateResponse.TotalPrice.Should().Be(325);

        var patchResponse = await _client.PartialUpdateBookingAsync(
            createResponse.BookingId,
            new { firstname = "Portfolio", additionalneeds = "Quiet room" },
            token);
        patchResponse.FirstName.Should().Be("Portfolio");
        patchResponse.AdditionalNeeds.Should().Be("Quiet room");

        var deleteStatus = await _client.DeleteBookingAsync(createResponse.BookingId, token);
        deleteStatus.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task GetMissingBooking_ShouldReturnNotFound()
    {
        var status = await _client.GetBookingStatusAsync(99999999);

        status.Should().Be(HttpStatusCode.NotFound);
    }

    private static Booking NewBooking(string firstName, string lastName, int price, string needs)
    {
        return new Booking(
            firstName,
            lastName,
            price,
            true,
            new BookingDates("2026-07-01", "2026-07-08"),
            needs);
    }
}
