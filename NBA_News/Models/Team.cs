using System.Text.Json.Serialization;

namespace NBA_News.Models
{
    /// <summary>
    /// Représente une équipe NBA.
    /// Utilisée seule (liste d'équipes) ET en objet imbriqué dans Player et Game.
    /// </summary>
    public class Team
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Abréviation ex : "LAL"
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; } = string.Empty;

        // Ville ex : "Los Angeles"
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        // "East" ou "West"
        [JsonPropertyName("conference")]
        public string Conference { get; set; } = string.Empty;

        // Ex : "Pacific", "Atlantic"...
        [JsonPropertyName("division")]
        public string Division { get; set; } = string.Empty;

        // Nom complet ex : "Los Angeles Lakers"
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        // Nom court ex : "Lakers"
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        // --- Propriété calculée ---

        /// <summary>Ville + Nom ex : "Los Angeles Lakers"</summary>
        public string CityAndName => $"{City} {Name}";
    }
}
