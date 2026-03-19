using System.Windows;
using System.Windows.Controls;
using NBA_News.Views;

namespace NBA_News
{
    /// <summary>
    /// Gère la navigation entre les pages via le Frame principal.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Pages créées une seule fois et réutilisées (cache)
        private HomePage? _homePage;
        private PlayersPage? _playersPage;
        private TeamsPage? _teamsPage;
        private GamesPage? _gamesPage;

        public MainWindow()
        {
            InitializeComponent();

            // Affiche la page d'accueil au démarrage
            NavigateTo("home");
        }

        // -------------------------------------------------------
        // NAVIGATION
        // -------------------------------------------------------

        private void BtnNavHome_Click(object sender, RoutedEventArgs e) => NavigateTo("home");
        private void BtnNavPlayers_Click(object sender, RoutedEventArgs e) => NavigateTo("players");
        private void BtnNavTeams_Click(object sender, RoutedEventArgs e) => NavigateTo("teams");
        private void BtnNavGames_Click(object sender, RoutedEventArgs e) => NavigateTo("games");

        /// <summary>
        /// Navigue vers la page demandée et met à jour les boutons actifs.
        /// </summary>
        private void NavigateTo(string page)
        {
            // Remet tous les boutons en style normal
            BtnNavHome.Style = (Style)Resources["NavButton"];
            BtnNavPlayers.Style = (Style)Resources["NavButton"];
            BtnNavTeams.Style = (Style)Resources["NavButton"];
            BtnNavGames.Style = (Style)Resources["NavButton"];

            switch (page)
            {
                case "home":
                    _homePage ??= new HomePage(this);
                    MainFrame.Navigate(_homePage);
                    BtnNavHome.Style = (Style)Resources["NavButtonActive"];
                    break;

                case "players":
                    _playersPage ??= new PlayersPage();
                    MainFrame.Navigate(_playersPage);
                    BtnNavPlayers.Style = (Style)Resources["NavButtonActive"];
                    break;

                case "teams":
                    _teamsPage ??= new TeamsPage();
                    MainFrame.Navigate(_teamsPage);
                    BtnNavTeams.Style = (Style)Resources["NavButtonActive"];
                    break;

                case "games":
                    _gamesPage ??= new GamesPage();
                    MainFrame.Navigate(_gamesPage);
                    BtnNavGames.Style = (Style)Resources["NavButtonActive"];
                    break;
            }
        }

        /// <summary>
        /// Méthode publique appelée depuis HomePage pour naviguer.
        /// </summary>
        public void GoTo(string page) => NavigateTo(page);
    }
}
