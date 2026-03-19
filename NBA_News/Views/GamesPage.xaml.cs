using NBA_News.Models;
using NBA_News.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace NBA_News.Views
{
    public partial class GamesPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Game> _allGames = new List<Game>();

        public GamesPage() => InitializeComponent();

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            // Cache : ne rappelle pas l'API si déjà chargé
            if (_allGames.Count > 0)
            {
                GamesGrid.ItemsSource = _allGames;
                return;
            }
            await LoadGamesAsync();
        }

        private async Task LoadGamesAsync()
        {
            ShowLoading(true);
            ErrorPanel.Visibility = Visibility.Collapsed;

            try
            {
                _allGames = await _apiService.GetGamesAsync();
                GamesGrid.ItemsSource = _allGames;
                SubTitle.Text = $"{_allGames.Count} matchs chargés";
                StatusBar.Text = $"[{DateTime.Now:HH:mm:ss}] ✅ {_allGames.Count} matchs chargés.";
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
                ErrorPanel.Visibility = Visibility.Visible;
                StatusBar.Text = $"[{DateTime.Now:HH:mm:ss}] ❌ Erreur.";
            }
            finally { ShowLoading(false); }
        }

        private void ShowLoading(bool isLoading)
        {
            LoadingPanel.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            BtnLoad.IsEnabled = !isLoading;
        }
    }
}
