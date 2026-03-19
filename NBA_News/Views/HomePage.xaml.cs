using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NBA_News.Views
{
    /// <summary>
    /// Page d'accueil avec hero, stats et cartes de navigation.
    /// </summary>
    public partial class HomePage : Page
    {
        // Référence à la fenêtre principale pour la navigation
        private readonly MainWindow _mainWindow;

        public HomePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        // Bouton hero "Voir les joueurs"
        private void BtnHeroPlayers_Click(object sender, RoutedEventArgs e)
            => _mainWindow.GoTo("players");

        // Bouton hero "Matchs en direct"
        private void BtnHeroGames_Click(object sender, RoutedEventArgs e)
            => _mainWindow.GoTo("games");

        // Clic sur la carte Joueurs
        private void CardPlayers_Click(object sender, MouseButtonEventArgs e)
            => _mainWindow.GoTo("players");

        // Clic sur la carte Équipes
        private void CardTeams_Click(object sender, MouseButtonEventArgs e)
            => _mainWindow.GoTo("teams");

        // Clic sur la carte Matchs
        private void CardGames_Click(object sender, MouseButtonEventArgs e)
            => _mainWindow.GoTo("games");
    }
}


