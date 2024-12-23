using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MA.SlotService.Api.Contracts;

namespace MA.SlotService.IntegrationTests.Extensions;

public static class HttpClientExtensions
{
    public static async Task<SpinsBalanceResponse> TopUpBalance(this HttpClient httpClient, int userId, int amount)
    {
        var uri = new Uri($"http://localhost/api/balance/{amount}");
        var body = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = uri,
            Headers = {
                { "UserId", userId.ToString() }
            }
        };
        var response = await httpClient.SendAsync(body);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SpinsBalanceResponse>();
        result.Should().NotBeNull();
        
        return result!;
    }
    
    public static async Task<SpinsBalanceResponse> GetBalance(this HttpClient httpClient, int userId)
    {
        var uri = new Uri($"http://localhost/api/balance");
        var body = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri,
            Headers = {
                { "UserId", userId.ToString() }
            }
        };
        var response = await httpClient.SendAsync(body);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SpinsBalanceResponse>();
        result.Should().NotBeNull();
        
        return result!;
    }
    
    public static async Task<SpinResultResponse?> Spin(this HttpClient httpClient, int userId, HttpStatusCode expectedCode)
    {
        var response = await httpClient.Spin(userId);
        
        response!.StatusCode.Should().Be(expectedCode);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await response.Content.ReadFromJsonAsync<SpinResultResponse>();
            result.Should().NotBeNull();
        
            return result!;
        }

        return null;
    }
    
    public static async Task<HttpResponseMessage?> Spin(this HttpClient httpClient, int userId)
    {
        var uri = new Uri($"http://localhost/api/slot/spin");
        var body = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = uri,
            Headers = {
                { "UserId", userId.ToString() }
            }
        };
        var response = await httpClient.SendAsync(body);

        return response;
    }
}