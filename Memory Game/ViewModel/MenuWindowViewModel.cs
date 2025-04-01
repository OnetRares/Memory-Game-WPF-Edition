﻿using Memory_Game.Model;
using System.Windows;
using System.Windows.Input;

namespace Memory_Game.ViewModel
{
    public class MenuWindowViewModel:BaseViewModel
    {
        private string selectedCategory;
        private string selectedGameMode;
        private int selectedRows;
        private int selectedColumns;
        private string username;

        public string WelcomeMessage => $"Hello {username}";

        private MemoryGameViewModel currentGameViewModel;

        public ICommand CategoryCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand StatisticsCommand { get; }
        public ICommand StandardGameCommand { get; }
        public ICommand CustomGameCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand BackToLoginCommand {  get; }
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
            BackToLoginCommand = new RelayCommand(BackToLogin);
            ExitCommand = new RelayCommand(ExitApplication);
            this.username = username;

            OnPropertyChanged(nameof(WelcomeMessage));
        }


        private void OpenCategorySelection()
        {
            
            string[] categories = { "Animals", "Cars", "Cartoons", "Pony"};

           
            string categoryPrompt = "Choose from: " + string.Join(", ", categories);


            string input = Microsoft.VisualBasic.Interaction.InputBox(categoryPrompt, "Select Category", "");

        
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

            MessageBox.Show($"The game will start with:\nCategory: '{selectedCategory}'\nMode: '{selectedGameMode}'\nBoard size: {selectedColumns}x{selectedRows}\nTime limit: {timeLimit} seconds.",
                "Starting Game", MessageBoxButton.OK, MessageBoxImage.Information);

            currentGameViewModel = new MemoryGameViewModel(username, timeLimit, selectedCategory, selectedColumns, selectedRows);
            MemoryGameWindow memoryGameWindow = new MemoryGameWindow(currentGameViewModel);

            memoryGameWindow.Closed += (s, e) => currentGameViewModel.StopTimer();

            memoryGameWindow.Show();
        }


        private void OpenSavedGame()
        {
            var savedGameViewModel = new SavedGameViewModel(username);
            SavedGameWindow savedGamesWindow = new SavedGameWindow(username)
            {
                DataContext = savedGameViewModel
            };

            savedGameViewModel.OnGameLoaded += (sender, filePath) =>
            {
                savedGamesWindow.DialogResult = true;
                savedGamesWindow.Close();
            };

            if (savedGamesWindow.ShowDialog() == true && !string.IsNullOrEmpty(savedGameViewModel.SelectedFile))
            {
                GameStateModel loadedState = GameStateServices.LoadGame(savedGameViewModel.SelectedFile);
                currentGameViewModel = new MemoryGameViewModel(loadedState);

                MemoryGameWindow memoryGameWindow = new MemoryGameWindow(currentGameViewModel);
                memoryGameWindow.Closed += (s, e) => currentGameViewModel.StopTimer();
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
                GameStateServices.SaveGame(state);
                MessageBox.Show("Game saved successfully.", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ShowStatistics()
        {
           StatisticsWindow statisticsWindow = new StatisticsWindow();
            statisticsWindow.Show();
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
            string inputCols = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of cols (between 2 and 6):", "Board Dimensions", "4");
            string inputRows = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of rows (between 2 and 6):", "Board Dimensions", "4");

            if (int.TryParse(inputRows, out int rows) &&
                int.TryParse(inputCols, out int cols) &&
                rows >= 2 && rows <= 6 &&
                cols >= 2 && cols <= 6 &&
                rows * cols % 2 == 0)
            {
                selectedRows = rows;
                selectedColumns = cols;
                MessageBox.Show($"Custom mode selected: The board will be {cols}x{rows}.", "Custom Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid dimensions entered. Please ensure that the number of rows and columns is between 2 and 6 and that their product is even.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                selectedGameMode = null;
            }
        }
        private void ShowAboutInfo()
        {
            MessageBox.Show("Memory Game\nDeveloper: Onet Rares-Nicolae\nEmail: rares.onet@student.com\nGroup: 10LF233\nSpecialization: Computer Science", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackToLogin()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows.OfType<MenuWindow>().FirstOrDefault()?.Close();
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
        
    }
}
