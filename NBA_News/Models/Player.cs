using NBA_News.Models;
using System.Text.Json.Serialization;

namespace NBA_News.Models
{
    /// <summary>
    /// Représente un joueur NBA récupéré depuis l'API balldontlie.
    /// Les [JsonPropertyName] font le lien entre le JSON et les propriétés C#.
    /// </summary>
    public class Player
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        // Poste : G = Guard, F = Forward, C = Center
        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("height_feet")]
        public int? HeightFeet { get; set; }

        [JsonPropertyName("height_inches")]
        public int? HeightInches { get; set; }

        [JsonPropertyName("weight_pounds")]
        public int? WeightPounds { get; set; }

        // Objet Team imbriqué dans le JSON joueur
        [JsonPropertyName("team")]
        public Team? Team { get; set; }

        [JsonPropertyName("jersey_number")]
        public string? JerseyNumber { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("draft_year")]
        public int? DraftYear { get; set; }

        [JsonPropertyName("draft_round")]
        public int? DraftRound { get; set; }

        [JsonPropertyName("draft_number")]
        public int? DraftNumber { get; set; }

        // --- Propriétés calculées (non issues de l'API) ---

        /// <summary>Prénom + Nom, ex : "LeBron James"</summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>Nom de l'équipe ou "Inconnu" si absent</summary>
        public string TeamName => Team?.FullName ?? "Inconnu";

        /// <summary>Taille formatée ex : "6'3\"" ou "N/A"</summary>
        public string Height => (HeightFeet.HasValue && HeightInches.HasValue)
            ? $"{HeightFeet}'{HeightInches}\""
            : "N/A";

        /// <summary>Poids formaté ex : "215 lbs" ou "N/A"</summary>
        public string Weight => WeightPounds.HasValue
            ? $"{WeightPounds} lbs"
            : "N/A";
    }
}
