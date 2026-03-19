using System;
using System.Text.Json.Serialization;

namespace NBA_News.Models
{
    /// <summary>
    /// Représente un match NBA avec les deux équipes et les scores.
    /// </summary>
    public class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Date au format "YYYY-MM-DD"
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        // Ex : "Final", "In Progress"
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        // Quart-temps actuel (4 = fin de match)
        [JsonPropertyName("period")]
        public int Period { get; set; }

        [JsonPropertyName("home_team_score")]
        public int HomeTeamScore { get; set; }

        [JsonPropertyName("visitor_team_score")]
        public int VisitorTeamScore { get; set; }

        [JsonPropertyName("home_team")]
        public Team? HomeTeam { get; set; }

        [JsonPropertyName("visitor_team")]
        public Team? VisitorTeam { get; set; }

        // --- Propriétés calculées ---

        /// <summary>Date en format français ex : "25/12/2024"</summary>
        public string FormattedDate
        {
            get
            {
                if (DateTime.TryParse(Date, out DateTime d))
                    return d.ToString("dd/MM/yyyy");
                return Date;
            }
        }

        public string HomeTeamName => HomeTeam?.FullName ?? "N/A";
        public string VisitorTeamName => VisitorTeam?.FullName ?? "N/A";

        /// <summary>Ex : "Lakers 110 - 98 Celtics"</summary>
        public string ScoreSummary =>
            $"{HomeTeamName} {HomeTeamScore} - {VisitorTeamScore} {VisitorTeamName}";
    }
}
