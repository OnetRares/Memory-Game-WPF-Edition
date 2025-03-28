
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Memory_Game.ViewModel;
namespace Memory_Game
{
    public partial class SavedGameWindow : Window
    {

        public SavedGameWindow(string username)
        {
            InitializeComponent();
            DataContext = new SavedGameViewModel(username);
        }


    }
}
