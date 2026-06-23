using System.Text.Json.Serialization;

namespace OrangeHRM.Framework.Api.Models;

public sealed record BookingDates(
    [property: JsonPropertyName("checkin")] string CheckIn,
    [property: JsonPropertyName("checkout")] string CheckOut);

public sealed record Booking(
    [property: JsonPropertyName("firstname")] string FirstName,
    [property: JsonPropertyName("lastname")] string LastName,
    [property: JsonPropertyName("totalprice")] int TotalPrice,
    [property: JsonPropertyName("depositpaid")] bool DepositPaid,
    [property: JsonPropertyName("bookingdates")] BookingDates BookingDates,
    [property: JsonPropertyName("additionalneeds")] string AdditionalNeeds);

public sealed record BookingCreateResponse(
    [property: JsonPropertyName("bookingid")] int BookingId,
    [property: JsonPropertyName("booking")] Booking Booking);
