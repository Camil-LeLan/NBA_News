using NBA_News.Models;
using NBA_News.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace NBA_News.Views
{
    public partial class PlayersPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Player> _allPlayers = new List<Player>();

        public PlayersPage()
        {
            InitializeComponent();
        }

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
            => await LoadPlayersAsync();

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
            => await LoadPlayersAsync(SearchBox.Text.Trim());

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible : Visibility.Collapsed;

            if (_allPlayers.Any())
            {
                string terme = SearchBox.Text.Trim().ToLower();
                PlayersGrid.ItemsSource = string.IsNullOrEmpty(terme)
                    ? _allPlayers
                    : _allPlayers.Where(p => p.FullName.ToLower().Contains(terme)).ToList();
            }
        }

        private async Task LoadPlayersAsync(string search = "")
        {
            // Cache : si déjà chargé sans filtre, ne rappelle pas l'API
            if (_allPlayers.Any() && string.IsNullOrEmpty(search))
            {
                PlayersGrid.ItemsSource = _allPlayers;
                return;
            }

            ShowLoading(true);
            ErrorPanel.Visibility = Visibility.Collapsed;

            try
            {
                _allPlayers = await _apiService.GetPlayersAsync(search);
                PlayersGrid.ItemsSource = _allPlayers;

                string label = string.IsNullOrEmpty(search) ? "" : $" pour \"{search}\"";
                SubTitle.Text = $"{_allPlayers.Count} joueur(s) trouvé(s){label}";
                StatusBar.Text = $"[{DateTime.Now:HH:mm:ss}] ✅ {_allPlayers.Count} joueurs chargés.";
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
                ErrorPanel.Visibility = Visibility.Visible;
                StatusBar.Text = $"[{DateTime.Now:HH:mm:ss}] ❌ Erreur.";
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void ShowLoading(bool isLoading)
        {
            LoadingPanel.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            BtnLoad.IsEnabled = !isLoading;
            BtnSearch.IsEnabled = !isLoading;
        }
    }
}
