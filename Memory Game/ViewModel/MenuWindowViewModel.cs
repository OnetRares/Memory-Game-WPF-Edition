using Memory_Game.Model;
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
            var categoryWindow = new CategoryWindow();
            var viewModel = categoryWindow.DataContext as CategoryViewModel;

          
            viewModel.ConfirmAction = () =>
            {
                if (viewModel.SelectedCategory != null)
                {
                    selectedCategory = viewModel.SelectedCategory.Name;
                    MessageBox.Show($"Selected category: {selectedCategory}", "Category", MessageBoxButton.OK, MessageBoxImage.Information);
                    categoryWindow.Close();
                }
                else
                {
                    MessageBox.Show("You must select a category.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

   
            viewModel.CancelAction = () =>
            {
                MessageBox.Show("You must select a category.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                categoryWindow.Close();
            };

            categoryWindow.ShowDialog();
        }
        private void StartNewGame()
        {
            if (string.IsNullOrEmpty(selectedCategory) || string.IsNullOrEmpty(selectedGameMode))
            {
                MessageBox.Show("Please select an image category and game mode (Standard or Custom) before starting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var timeWindow = new TimeWindow();
            var timeViewModel = timeWindow.DataContext as TimeViewModel;

            if (timeViewModel == null)
            {
                MessageBox.Show("Error loading time window.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

          
            timeViewModel.ConfirmAction = () =>
            {
                int timeLimit = timeViewModel.TimeLimit;

            
                if (timeLimit > 0)
                {
                    
                   
                    currentGameViewModel = new MemoryGameViewModel(username, timeLimit, selectedCategory, selectedColumns, selectedRows);
                    MemoryGameWindow memoryGameWindow = new MemoryGameWindow(currentGameViewModel);

                    memoryGameWindow.Closed += (s, e) => currentGameViewModel.StopTimer();
                    memoryGameWindow.Show();
                }
                else
                {
                    MessageBox.Show("Invalid time limit entered. Defaulting to 60 seconds.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                timeWindow.Close();
            };

            timeViewModel.CancelAction = () =>
            {
                timeWindow.Close();
            };

            timeWindow.ShowDialog();
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

           
            BoardDimensionWindow boardWindow = new BoardDimensionWindow();
            if (boardWindow.ShowDialog() == true)
            {
               
                var viewModel = boardWindow.DataContext as BoardDimensionsViewModel;
                selectedRows = viewModel.SelectedRow;
                selectedColumns = viewModel.SelectedColumn;
                MessageBox.Show($"Custom mode selected: The board will be {selectedColumns}x{selectedRows}.", "Custom Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Dimensiunile tablei nu au fost setate.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
