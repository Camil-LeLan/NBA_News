using NBA_News.Models;
using NBA_News.Services;
// Ajouts pour la logique NBA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NBA_News
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// Ce fichier gère toute la logique de l'interface :
    ///   - Clics sur les boutons
    ///   - Appels au service API
    ///   - Remplissage des DataGrids
    ///   - Recherche et filtrage
    ///   - Affichage des erreurs
    /// </summary>
    public partial class MainWindow : Window
    {
        // -------------------------------------------------------
        // CHAMPS PRIVÉS
        // -------------------------------------------------------

        // Service qui fait les appels à l'API NBA
        private readonly ApiService _apiService;

        // Listes gardées en mémoire pour le filtrage local
        private List<Player> _allPlayers = new List<Player>();
        private List<Team> _allTeams = new List<Team>();
        private List<Game> _allGames = new List<Game>();

        // Vue actuellement affichée : "players", "teams", "games" ou ""
        private string _currentView = "";

        // -------------------------------------------------------
        // CONSTRUCTEUR
        // -------------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();

            // Création du service API (une seule instance pour toute l'app)
            _apiService = new ApiService();

            SetStatus("Application démarrée. Cliquez sur un bouton pour charger les données.");
        }

        // -------------------------------------------------------
        // CLICS SUR LES BOUTONS
        // -------------------------------------------------------

        /// <summary>Clic sur "👤 Joueurs"</summary>
        private async void BtnLoadPlayers_Click(object sender, RoutedEventArgs e)
        {
            if (_allPlayers.Any())
            {
                PlayersGrid.ItemsSource = _allPlayers;
                PlayersGrid.Visibility = Visibility.Visible;
                _currentView = "players";
                SectionTitle.Text = $"👤 Joueurs NBA — {_allPlayers.Count} résultat(s)";
                SetStatus($"✅ {_allPlayers.Count} joueurs affichés (cache).");
                HideAllGrids();
                PlayersGrid.Visibility = Visibility.Visible;
                return;
            }
            await LoadPlayersAsync();
        }

        /// <summary>Clic sur "🏆 Équipes"</summary>
        private async void BtnLoadTeams_Click(object sender, RoutedEventArgs e)
        {
            if (_allTeams.Any())
            {
                HideAllGrids();
                TeamsGrid.ItemsSource = _allTeams;
                TeamsGrid.Visibility = Visibility.Visible;
                _currentView = "teams";
                SectionTitle.Text = $"🏆 Équipes NBA — {_allTeams.Count} équipes";
                SetStatus($"✅ {_allTeams.Count} équipes affichées (cache).");
                return;
            }
            await LoadTeamsAsync();
        }

        /// <summary>Clic sur "📅 Matchs"</summary>
        private async void BtnLoadGames_Click(object sender, RoutedEventArgs e)
        {
            if (_allGames.Any())
            {
                GamesGrid.ItemsSource = _allGames;
                GamesGrid.Visibility = Visibility.Visible;
                return;
            }
            await LoadGamesAsync();
        }

        /// <summary>Clic sur "Rechercher"</summary>
        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBox.Text.Trim();

            // La recherche via API ne fonctionne que pour les joueurs
            if (_currentView == "players" || !string.IsNullOrEmpty(searchTerm))
            {
                await LoadPlayersAsync(searchTerm);
            }
        }

        /// <summary>
        /// Déclenché à chaque frappe dans la barre de recherche.
        /// Gère le placeholder ET le filtrage local instantané.
        /// </summary>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Affiche ou cache le texte grisé "Rechercher un joueur..."
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Filtrage local (sans appel API) si des joueurs sont déjà chargés
            if (_currentView == "players" && _allPlayers.Any())
            {
                string terme = SearchBox.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(terme))
                {
                    // Aucun filtre → reaffiche tout
                    PlayersGrid.ItemsSource = _allPlayers;
                    SetStatus($"{_allPlayers.Count} joueurs affichés.");
                }
                else
                {
                    // Filtre sur le nom complet (insensible à la casse)
                    var filtres = _allPlayers
                        .Where(p => p.FullName.ToLower().Contains(terme))
                        .ToList();

                    PlayersGrid.ItemsSource = filtres;
                    SetStatus($"{filtres.Count} joueur(s) trouvé(s) pour \"{SearchBox.Text}\".");
                }
            }
        }

        // -------------------------------------------------------
        // CHARGEMENT DES DONNÉES
        // -------------------------------------------------------

        /// <summary>
        /// Appelle l'API pour récupérer les joueurs et les affiche.
        /// Le paramètre search permet de filtrer par nom.
        /// </summary>
        private async Task LoadPlayersAsync(string search = "")
        {
            ShowLoading(true);
            _currentView = "players";
            HideAllGrids();

            string label = string.IsNullOrEmpty(search) ? "" : $" pour \"{search}\"";
            SectionTitle.Text = $"👤 Joueurs NBA{label}";
            SetStatus("Chargement des joueurs...");

            try
            {
                // ← Appel asynchrone : l'interface reste réactive pendant ce temps
                _allPlayers = await _apiService.GetPlayersAsync(search);

                // On donne la liste à la DataGrid pour qu'elle l'affiche
                PlayersGrid.ItemsSource = _allPlayers;
                PlayersGrid.Visibility = Visibility.Visible;

                SectionTitle.Text = $"👤 Joueurs NBA — {_allPlayers.Count} résultat(s){label}";
                SetStatus($"✅ {_allPlayers.Count} joueurs chargés.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                SetStatus("❌ Erreur lors du chargement des joueurs.");
            }
            finally
            {
                // Ce bloc s'exécute TOUJOURS (succès ou erreur)
                ShowLoading(false);
            }
        }

        /// <summary>
        /// Appelle l'API pour récupérer les 30 équipes NBA et les affiche.
        /// </summary>
        private async Task LoadTeamsAsync()
        {
            ShowLoading(true);
            _currentView = "teams";
            HideAllGrids();
            SectionTitle.Text = "🏆 Équipes NBA";
            SetStatus("Chargement des équipes...");

            try
            {
                _allTeams = await _apiService.GetTeamsAsync();

                TeamsGrid.ItemsSource = _allTeams;
                TeamsGrid.Visibility = Visibility.Visible;

                SectionTitle.Text = $"🏆 Équipes NBA — {_allTeams.Count} équipes";
                SetStatus($"✅ {_allTeams.Count} équipes chargées.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                SetStatus("❌ Erreur lors du chargement des équipes.");
            }
            finally
            {
                ShowLoading(false);
            }
        }

        /// <summary>
        /// Appelle l'API pour récupérer les matchs récents et les affiche.
        /// </summary>
        private async Task LoadGamesAsync()
        {
            ShowLoading(true);
            _currentView = "games";
            HideAllGrids();
            SectionTitle.Text = "📅 Matchs NBA récents";
            SetStatus("Chargement des matchs...");

            try
            {
                _allGames = await _apiService.GetGamesAsync();

                GamesGrid.ItemsSource = _allGames;
                GamesGrid.Visibility = Visibility.Visible;

                SectionTitle.Text = $"📅 Matchs NBA — {_allGames.Count} matchs";
                SetStatus($"✅ {_allGames.Count} matchs chargés.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                SetStatus("Erreur lors du chargement des matchs.");
            }
            finally
            {
                ShowLoading(false);
            }
        }

        // -------------------------------------------------------
        // MÉTHODES UTILITAIRES (helpers interface)
        // -------------------------------------------------------

        /// <summary>
        /// Affiche ou masque l'indicateur de chargement.
        /// Désactive aussi les boutons pour éviter les double-clics.
        /// </summary>
        private void ShowLoading(bool isLoading)
        {
            LoadingPanel.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            BtnLoadPlayers.IsEnabled = !isLoading;
            BtnLoadTeams.IsEnabled = !isLoading;
            BtnLoadGames.IsEnabled = !isLoading;
            BtnSearch.IsEnabled = !isLoading;
        }

        /// <summary>
        /// Cache toutes les DataGrids et le panneau d'erreur.
        /// Appelée avant d'en afficher une nouvelle.
        /// </summary>
        private void HideAllGrids()
        {
            PlayersGrid.Visibility = Visibility.Collapsed;
            TeamsGrid.Visibility = Visibility.Collapsed;
            GamesGrid.Visibility = Visibility.Collapsed;
            ErrorPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Affiche le panneau rouge d'erreur avec le message passé en paramètre.
        /// </summary>
        private void ShowError(string message)
        {
            HideAllGrids();
            ErrorMessage.Text = message;
            ErrorPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Met à jour le texte dans la barre de statut en bas de la fenêtre.
        /// Ajoute automatiquement l'heure courante.
        /// </summary>
        private void SetStatus(string message)
        {
            StatusBar.Text = $"[{DateTime.Now:HH:mm:ss}] {message}";
        }
    }
}
