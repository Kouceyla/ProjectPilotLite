using System.Net.Http;
using System.Net.Http.Json;

namespace ProjectPilotLite.Wpf.Services;

/// <summary>
/// Service d'appel HTTP à l'API, partagé par tous les écrans (isolé du code-behind,
/// conformément à la contrainte MVVM de l'énoncé). Base minimale pour rendre les
/// features Tâches / Livrables fonctionnelles dès maintenant ; Ceredine pourra
/// l'enrichir (gestion d'erreurs centralisée, configuration de l'URL, etc.) lors
/// de l'intégration du shell WPF.
/// </summary>
public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(string baseUrl = "http://localhost:5123")
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public async Task<List<T>> GetListAsync<T>(string route)
    {
        var result = await _http.GetFromJsonAsync<List<T>>(route);
        return result ?? new List<T>();
    }

    public async Task<T?> GetAsync<T>(string route)
        => await _http.GetFromJsonAsync<T>(route);

    public async Task<T?> PostAsync<T>(string route, object body)
    {
        var response = await _http.PostAsJsonAsync(route, body);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task PatchAsync(string route, object body)
    {
        var response = await _http.PatchAsJsonAsync(route, body);
        response.EnsureSuccessStatusCode();
    }
}
