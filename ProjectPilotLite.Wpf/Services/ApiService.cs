using System.Net.Http;
using System.Net.Http.Json;

namespace ProjectPilotLite.Wpf.Services;
public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(string baseUrl = "http://localhost:5123")
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }


    public async Task<List<T>> GetListAsync<T>(string route)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<T>>(route);
            return result ?? new List<T>();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return new List<T>();
        }
    }

    public async Task<T?> GetAsync<T>(string route)
    {
        try
        {
            return await _http.GetFromJsonAsync<T>(route);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return default;
        }
    }

    public async Task<T?> PostAsync<T>(string route, object body)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(route, body);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return default;
        }
    }

    public async Task PatchAsync(string route, object body)
    {
        try
        {
            var response = await _http.PatchAsJsonAsync(route, body);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
        }
    }
}
