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

        public MemoryGameWindow(int timeLimitInSeconds,string selectedCategory, int selectedRows, int selectedColumns)
        {
            InitializeComponent();
            DataContext = new MemoryGameViewModel(timeLimitInSeconds, selectedCategory, selectedRows, selectedColumns); 
        }
    }
}
    

