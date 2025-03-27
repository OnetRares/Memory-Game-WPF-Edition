using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory_Game
{

    public partial class MemoryGameWindow : Window
    {
        private MemoryGameViewModel viewModel;

        
        public MemoryGameWindow(string username, int timeLimit, string category, int rows, int columns)
        {
            InitializeComponent();
            viewModel = new MemoryGameViewModel(username, timeLimit, category, rows, columns);
            DataContext = viewModel;
        }

        
        public MemoryGameWindow(GameState state)
        {
            InitializeComponent();
            viewModel = new MemoryGameViewModel(state);
            DataContext = viewModel;
        }
        public MemoryGameWindow(MemoryGameViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
    

