
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

        public string SelectedFile { get; private set; }

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
                SelectedFile = selectedItem.FilePath;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
