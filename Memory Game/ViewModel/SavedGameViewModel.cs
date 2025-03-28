using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Memory_Game.Model;
namespace Memory_Game.ViewModel
{
    public class SavedGameViewModel : BaseViewModel
    {
        private string _username;
        private SavedGameItem _selectedGame;

        public ObservableCollection<SavedGameItem> SavedGames { get; set; }
        public ICommand LoadGameCommand { get; }
        public string SelectedFile { get; private set; }

        public SavedGameViewModel(string username)
        {
            _username = username;
            SavedGames = [.. GameStateServices.GetSavedGameFiles(username)
                    .Select(f => new SavedGameItem { Filename = Path.GetFileName(f), FilePath = f })];
            LoadGameCommand = new RelayCommand(LoadSelectedGame, CanLoadGame);
        }

        public SavedGameItem SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void LoadSelectedGame()
        {
            if (SelectedGame != null)
            {
                SelectedFile = SelectedGame.FilePath;
                OnGameLoaded?.Invoke(this, SelectedFile);
            }
        }

        private bool CanLoadGame()
        {
            return SelectedGame != null;
        }
        public event EventHandler<string> OnGameLoaded;
    }

}
