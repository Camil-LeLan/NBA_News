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
    public partial class TeamsPage : Page
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Team> _allTeams = new List<Team>();

        public TeamsPage() => InitializeComponent();

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            // Cache : ne rappelle pas l'API si déjà chargé
            if (_allTeams.Count > 0)
            {
                TeamsGrid.ItemsSource = _allTeams;
                return;
            }
            await LoadTeamsAsync();
        }

        private async Task LoadTeamsAsync()
        {
            ShowLoading(true);
            ErrorPanel.Visibility = Visibility.Collapsed;

            try
            {
                _allTeams = await _apiService.GetTeamsAsync();
                TeamsGrid.ItemsSource = _allTeams;
                SubTitle.Text = $"{_allTeams.Count} équipes NBA";
                StatusBar.Text = $"[{DateTime.Now:HH:mm:ss}] ✅ {_allTeams.Count} équipes chargées.";
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
