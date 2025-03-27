
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Memory_Game
{
    public partial class SavedGameWindow : Window
    {
        public class SavedGameItem
        {
            public string Filename { get; set; }
            public string FilePath { get; set; }
        }

        public List<SavedGameItem> SavedGames { get; set; }

        private string _username;
        public SavedGameWindow(string username)
        {
            InitializeComponent();
            _username = username;
            LoadSavedGames();
            SavedGamesList.ItemsSource = SavedGames;
        }

        private void LoadSavedGames()
        {
            SavedGames = GameStateStorage.GetSavedGameFiles(_username)
                           .Select(f => new SavedGameItem { Filename = Path.GetFileName(f), FilePath = f })
                           .ToList();
        }

        private void SavedGamesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SavedGamesList.SelectedItem is SavedGameItem selectedItem)
            {
                try
                {
                    GameState state = GameStateStorage.LoadGame(selectedItem.FilePath);
                    MemoryGameWindow gameWindow = new MemoryGameWindow(state);
                    gameWindow.Show();
                    this.Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Eroare la încărcarea jocului: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
