using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NBA_News.Models
{
    /// <summary>
    /// Enveloppe générique pour toutes les réponses de l'API balldontlie.
    /// 
    /// Le JSON retourné est toujours sous cette forme :
    /// {
    ///   "data": [ {...}, {...} ],
    ///   "meta": { "total_pages": 5, "current_page": 1, ... }
    /// }
    /// 
    /// Le T générique permet de réutiliser cette classe pour
    /// Player, Team et Game sans la réécrire 3 fois.
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new List<T>();

        [JsonPropertyName("meta")]
        public MetaInfo? Meta { get; set; }
    }

    /// <summary>
    /// Informations de pagination retournées par l'API.
    /// </summary>
    public class MetaInfo
    {
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("next_page")]
        public int? NextPage { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
    }
}
