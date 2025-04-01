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
using Memory_Game.ViewModel;

namespace Memory_Game
{
   
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel();
        }
    }
}
