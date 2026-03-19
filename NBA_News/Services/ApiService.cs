using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using NBA_News.Models;

namespace NBA_News.Services
{
    /// <summary>
    /// Centralise tous les appels HTTP vers l'API balldontlie.
    /// 
    /// API : https://www.balldontlie.io/
    /// Inscription gratuite pour obtenir une clé API.
    /// 
    /// Toutes les méthodes sont async pour ne pas bloquer l'interface WPF.
    /// </summary>
    public class ApiService
    {
        // URL de base de l'API
        private const string BASE_URL = "https://api.balldontlie.io/v1";

        // ⚠️ Remplace cette valeur par ta vraie clé API (inscription gratuite sur balldontlie.io)
        private const string API_KEY = "ddf91c66-957a-445f-b60e-fb49a2437b47";

        // HttpClient réutilisable (bonne pratique : 1 seule instance par application)
        private readonly HttpClient _httpClient;

        // Options pour la désérialisation JSON
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService()
        {
            _httpClient = new HttpClient();

            // La clé API est envoyée dans le header Authorization à chaque requête
            _httpClient.DefaultRequestHeaders.Add("Authorization", API_KEY);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Récupère une liste de joueurs NBA.
        /// Paramètre search : filtre optionnel par nom (ex : "james" → LeBron James...)
        /// URL appelée : /players?per_page=25[&search=xxx]
        /// </summary>
        public async Task<List<Player>> GetPlayersAsync(string search = "")
        {
            try
            {
                string url = $"{BASE_URL}/players?per_page=25";

                if (!string.IsNullOrWhiteSpace(search))
                    url += $"&search={Uri.EscapeDataString(search)}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<Player>>(json, _jsonOptions);
                return result?.Data ?? new List<Player>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erreur réseau (joueurs) : {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erreur JSON (joueurs) : {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur inattendue (joueurs) : {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Récupère les 30 équipes NBA.
        /// URL appelée : /teams?per_page=100
        /// </summary>
        public async Task<List<Team>> GetTeamsAsync()
        {
            try
            {
                string url = $"{BASE_URL}/teams?per_page=100";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<Team>>(json, _jsonOptions);
                return result?.Data ?? new List<Team>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erreur réseau (équipes) : {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erreur JSON (équipes) : {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur inattendue (équipes) : {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Récupère les matchs de la saison NBA en cours.
        /// URL appelée : /games?per_page=25&seasons[]=2024
        /// 
        /// La saison NBA commence en octobre :
        ///   - Avant octobre → saison de l'année précédente
        ///   - Après octobre → saison de l'année en cours
        /// </summary>
        public async Task<List<Game>> GetGamesAsync()
        {
            // Ajoute cette ligne avant le GetAsync dans GetGamesAsync()
            await Task.Delay(1000); // attend 1 seconde
            try
            {
                int currentSeason = DateTime.Now.Month >= 10
                    ? DateTime.Now.Year
                    : DateTime.Now.Year - 1;

                string url = $"{BASE_URL}/games?per_page=25&seasons[]={currentSeason}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<Game>>(json, _jsonOptions);
                return result?.Data ?? new List<Game>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erreur réseau (matchs) : {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Erreur JSON (matchs) : {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur inattendue (matchs) : {ex.Message}", ex);
            }
        }
    }
}
