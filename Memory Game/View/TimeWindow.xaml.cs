using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    /// <summary>
    /// Interaction logic for TimeWindow.xaml
    /// </summary>
    public partial class TimeWindow : Window
    {
        public TimeWindow()
        {
            InitializeComponent();
            DataContext = new TimeViewModel();
        }
    }
}
