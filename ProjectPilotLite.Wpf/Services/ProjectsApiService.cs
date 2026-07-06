using System.Net;
using System.Net.Http.Json;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Wpf.Services
{
    /// <summary>Résultat d'un appel API : succès + valeur, ou message d'erreur lisible.</summary>
    public class ApiResult<T>
    {
        public bool Succes { get; init; }
        public T? Valeur { get; init; }
        public string? MessageErreur { get; init; }

        public static ApiResult<T> Ok(T valeur) => new() { Succes = true, Valeur = valeur };
        public static ApiResult<T> Erreur(string message) => new() { Succes = false, MessageErreur = message };
    }

    /// <summary>
    /// Client HTTP pour la feature "Projets" (responsable : Ethan).
    /// NOTE INTEGRATION : le README du repo mentionne un unique "Services/ApiService.cs"
    /// partagé — si le shell WPF (Ceredine) définit un tel service commun, fusionner les
    /// méthodes ci-dessous dedans. En attendant, ce service est autonome et n'a besoin que
    /// d'un HttpClient dont la BaseAddress pointe vers http://localhost:5123/.
    /// Gestion des erreurs HTTP 400/404 -> message utilisateur lisible (rôle secondaire d'Ethan).
    /// </summary>
    public class ProjectsApiService
    {
        private const string Route = "api/projects";
        private readonly HttpClient _httpClient;

        public ProjectsApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<List<ProjetDto>>> ObtenirTousAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(Route);
                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<List<ProjetDto>>.Erreur(await MessageErreur(response));
                }

                var projets = await response.Content.ReadFromJsonAsync<List<ProjetDto>>() ?? new();
                return ApiResult<List<ProjetDto>>.Ok(projets);
            }
            catch (HttpRequestException ex)
            {
                return ApiResult<List<ProjetDto>>.Erreur($"Impossible de contacter l'API : {ex.Message}");
            }
        }

        public async Task<ApiResult<ProjetDetailDto>> ObtenirParIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Route}/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ApiResult<ProjetDetailDto>.Erreur($"Le projet #{id} n'existe pas (404).");
                }
                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<ProjetDetailDto>.Erreur(await MessageErreur(response));
                }

                var projet = await response.Content.ReadFromJsonAsync<ProjetDetailDto>();
                return projet is null
                    ? ApiResult<ProjetDetailDto>.Erreur("Réponse vide de l'API.")
                    : ApiResult<ProjetDetailDto>.Ok(projet);
            }
            catch (HttpRequestException ex)
            {
                return ApiResult<ProjetDetailDto>.Erreur($"Impossible de contacter l'API : {ex.Message}");
            }
        }

        public async Task<ApiResult<ProjetDto>> CreerAsync(ProjetCreateDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(Route, dto);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ApiResult<ProjetDto>.Erreur(await MessageErreur(response, prefixeDefaut:
                        "Requête invalide (400) : vérifie le nom, la description, le responsable et les dates."));
                }
                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<ProjetDto>.Erreur(await MessageErreur(response));
                }

                var projet = await response.Content.ReadFromJsonAsync<ProjetDto>();
                return projet is null
                    ? ApiResult<ProjetDto>.Erreur("Réponse vide de l'API.")
                    : ApiResult<ProjetDto>.Ok(projet);
            }
            catch (HttpRequestException ex)
            {
                return ApiResult<ProjetDto>.Erreur($"Impossible de contacter l'API : {ex.Message}");
            }
        }

        public async Task<ApiResult<ProjetDto>> ChangerStatutAsync(int id, StatutProjet nouveauStatut)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync(
                    $"{Route}/{id}/status", new ProjetStatutUpdateDto { Statut = nouveauStatut });

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ApiResult<ProjetDto>.Erreur($"Le projet #{id} n'existe pas (404).");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ApiResult<ProjetDto>.Erreur(await MessageErreur(response, prefixeDefaut: "Statut invalide (400)."));
                }
                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<ProjetDto>.Erreur(await MessageErreur(response));
                }

                var projet = await response.Content.ReadFromJsonAsync<ProjetDto>();
                return projet is null
                    ? ApiResult<ProjetDto>.Erreur("Réponse vide de l'API.")
                    : ApiResult<ProjetDto>.Ok(projet);
            }
            catch (HttpRequestException ex)
            {
                return ApiResult<ProjetDto>.Erreur($"Impossible de contacter l'API : {ex.Message}");
            }
        }

        private static async Task<string> MessageErreur(HttpResponseMessage response, string? prefixeDefaut = null)
        {
            var contenu = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(prefixeDefaut))
            {
                return string.IsNullOrWhiteSpace(contenu) ? prefixeDefaut : $"{prefixeDefaut} {contenu}";
            }
            return $"Erreur API ({(int)response.StatusCode} {response.StatusCode}) : {contenu}";
        }
    }
}
