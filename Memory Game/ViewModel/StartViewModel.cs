using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Memory_Game.ViewModel;

namespace Memory_Game
{
    public class StartViewModel:BaseViewModel
    {
        public ImageBrush BackgroundBrush { get; }
        public ICommand StartGameCommand { get; }

        public StartViewModel()
        {
            StartGameCommand = new RelayCommand(StartGame);

   
            BackgroundModel backgroundModel = new BackgroundModel();
            BackgroundBrush = backgroundModel.GetBackgroundBrush();
        }

        private void StartGame()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows.OfType<StartWindow>().FirstOrDefault()?.Close();
        }
    }
}
