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
                   CustomMessageViewModel.ShowSelectedCategoryMessage(selectedCategory);
                    categoryWindow.Close();
                }
                else
                {
                    CustomMessageViewModel.ShowSelectedCategoryErrorMessage();
                }
            };

   
            viewModel.CancelAction = () =>
            {
                CustomMessageViewModel.ShowSelectedCategoryErrorMessage();
                categoryWindow.Close();
            };

            categoryWindow.ShowDialog();
        }
        private void StartNewGame()
        {
            if (string.IsNullOrEmpty(selectedCategory) || string.IsNullOrEmpty(selectedGameMode))
            {
                CustomMessageViewModel.ShowStartNewGameErrorMessage();
                return;
            }

            var timeWindow = new TimeWindow();
            var timeViewModel = timeWindow.DataContext as TimeViewModel;
          
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
                CustomMessageViewModel.ShowCurrentSaveGameErrorMessage();
                return;
            }

            var state = currentGameViewModel.GetCurrentGameState();
            try
            {
                GameStateServices.SaveGame(state);
                CustomMessageViewModel.ShowSaveGameMessage();
            }
            catch (Exception ex)
            {
                CustomMessageViewModel.ShowSaveErrorMessage(ex);
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
            CustomMessageViewModel.ShowStandardGameMessage();
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
                CustomMessageViewModel.ShowCustomGameMessage(selectedColumns,selectedRows);
            }
            else
            {
                CustomMessageViewModel.ShowCustomErrorGameMessage();
                selectedGameMode = null;
            }
        }
        private void ShowAboutInfo()
        {
            CustomMessageViewModel.ShowAboutInfoMessage();
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
