using System.Windows;
using System.Windows.Input;

namespace Memory_Game
{
    public class MenuWindowViewModel:BaseViewModel
    {
        private string selectedCategory;
        private string selectedGameMode;
        private int selectedRows;
        private int selectedColumns;
        private string username;

        private MemoryGameViewModel currentGameViewModel;

        public ICommand CategoryCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand StatisticsCommand { get; }
        public ICommand StandardGameCommand { get; }
        public ICommand CustomGameCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }

        public MenuWindowViewModel(string username)
        {
            CategoryCommand = new RelayCommand(OpenCategorySelection);
            NewGameCommand = new RelayCommand(StartNewGame);
            OpenGameCommand = new RelayCommand(OpenSavedGame);
            SaveGameCommand = new RelayCommand(SaveCurrentGame);
            StatisticsCommand = new RelayCommand(ShowStatistics);
            StandardGameCommand = new RelayCommand(SelectStandardGame);
            CustomGameCommand = new RelayCommand(SelectCustomGame);
            AboutCommand = new RelayCommand(ShowAboutInfo);
            ExitCommand = new RelayCommand(ExitApplication);
            this.username = username;
        }

        private void OpenCategorySelection()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the image category name:", "Select Category", "Default");
            if (!string.IsNullOrWhiteSpace(input))
            {
                selectedCategory = input;
                MessageBox.Show($"Selected category: {selectedCategory}", "Category", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("You must select a category.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectStandardGame()
        {
            selectedGameMode = "Standard";
            selectedRows = 4;
            selectedColumns = 4;
            MessageBox.Show("Standard mode selected: The board will be 4x4.", "Standard Game", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SelectCustomGame()
        {
            selectedGameMode = "Custom";
            string inputRows = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of rows (between 2 and 6):", "Board Dimensions", "4");
            string inputCols = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of columns (between 2 and 6):", "Board Dimensions", "4");

            if (int.TryParse(inputRows, out int rows) &&
                int.TryParse(inputCols, out int cols) &&
                rows >= 2 && rows <= 6 &&
                cols >= 2 && cols <= 6 &&
                (rows * cols) % 2 == 0)
            {
                selectedRows = rows;
                selectedColumns = cols;
                MessageBox.Show($"Custom mode selected: The board will be {rows}x{cols}.", "Custom Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid dimensions entered. Please ensure that the number of rows and columns is between 2 and 6 and that their product is even.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                selectedGameMode = null;
            }
        }

        private void StartNewGame()
        {
            if (string.IsNullOrEmpty(selectedCategory) || string.IsNullOrEmpty(selectedGameMode))
            {
                MessageBox.Show("Please select an image category and game mode (Standard or Custom) before starting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int timeLimit = 60;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the game time limit (in seconds):", "Time Limit", "60");

            if (int.TryParse(input, out int parsedTimeLimit) && parsedTimeLimit > 0)
            {
                timeLimit = parsedTimeLimit;
            }
            else
            {
                MessageBox.Show("The entered time is not valid. The game will start with the default time limit of 60 seconds.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show($"The game will start with:\nCategory: '{selectedCategory}'\nMode: '{selectedGameMode}'\nBoard size: {selectedRows}x{selectedColumns}\nTime limit: {timeLimit} seconds.",
                "Starting Game", MessageBoxButton.OK, MessageBoxImage.Information);

            currentGameViewModel = new MemoryGameViewModel(username, timeLimit, selectedCategory, selectedRows, selectedColumns);

            MemoryGameWindow memoryGameWindow = new MemoryGameWindow(currentGameViewModel);
            memoryGameWindow.Show();

            MessageBox.Show($"Hello {username}");
        }

        private void OpenSavedGame()
        {
            SavedGameWindow savedGamesWindow = new SavedGameWindow(username);
            if (savedGamesWindow.ShowDialog() == true && !string.IsNullOrEmpty(savedGamesWindow.SelectedFile))
            {
                GameState loadedState = GameStateStorage.LoadGame(savedGamesWindow.SelectedFile);
                currentGameViewModel = new MemoryGameViewModel(loadedState);
                MemoryGameWindow memoryGameWindow = new MemoryGameWindow(currentGameViewModel);
                memoryGameWindow.Show();
            }
        }
       

        private void SaveCurrentGame()
        {
            if (currentGameViewModel == null)
            {
                MessageBox.Show("There is no game in progress to save..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var state = currentGameViewModel.GetCurrentGameState();
            try
            {
                GameStateStorage.SaveGame(state);
                MessageBox.Show("Game saved successfully.", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ShowStatistics()
        {
            MessageBox.Show("Showing game statistics...", "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowAboutInfo()
        {
            MessageBox.Show("Memory Game\nDeveloper: Your Name\nEmail: your.email@domain.com\nGroup: XYZ\nSpecialization: ABC", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
