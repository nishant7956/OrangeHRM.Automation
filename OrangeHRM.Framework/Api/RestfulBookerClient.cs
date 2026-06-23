using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using OrangeHRM.Framework.Api.Models;

namespace OrangeHRM.Framework.Api;

public sealed class RestfulBookerClient : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _client;

    public RestfulBookerClient(string baseUrl)
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/")
        };

        _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    }

    public async Task<string> CreateTokenAsync(string username = "admin", string password = "password123")
    {
        var response = await _client.PostAsJsonAsync("auth", new AuthRequest(username, password), JsonOptions);
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth?.Token ?? throw new InvalidOperationException("Restful Booker did not return an auth token.");
    }

    public async Task<BookingCreateResponse> CreateBookingAsync(Booking booking)
    {
        var response = await _client.PostAsJsonAsync("booking", booking, JsonOptions);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BookingCreateResponse>(JsonOptions)
            ?? throw new InvalidOperationException("Restful Booker did not return a booking create response.");
    }

    public async Task<Booking> GetBookingAsync(int bookingId)
    {
        var response = await _client.GetAsync($"booking/{bookingId}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Booking>(JsonOptions)
            ?? throw new InvalidOperationException($"Booking {bookingId} response was empty.");
    }

    public async Task<Booking> UpdateBookingAsync(int bookingId, Booking booking, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"booking/{bookingId}")
        {
            Content = JsonContent.Create(booking, options: JsonOptions)
        };
        request.Headers.Add("Cookie", $"token={token}");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Booking>(JsonOptions)
            ?? throw new InvalidOperationException($"Booking {bookingId} update response was empty.");
    }

    public async Task<Booking> PartialUpdateBookingAsync(int bookingId, object patch, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Patch, $"booking/{bookingId}")
        {
            Content = new StringContent(JsonSerializer.Serialize(patch, JsonOptions), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Cookie", $"token={token}");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Booking>(JsonOptions)
            ?? throw new InvalidOperationException($"Booking {bookingId} partial update response was empty.");
    }

    public async Task<HttpStatusCode> DeleteBookingAsync(int bookingId, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"booking/{bookingId}");
        request.Headers.Add("Cookie", $"token={token}");

        var response = await _client.SendAsync(request);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> GetBookingStatusAsync(int bookingId)
    {
        var response = await _client.GetAsync($"booking/{bookingId}");
        return response.StatusCode;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
