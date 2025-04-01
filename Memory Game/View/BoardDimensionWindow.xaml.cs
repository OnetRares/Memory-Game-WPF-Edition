using Memory_Game.ViewModel;
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
    public partial class BoardDimensionWindow : Window
    {
        public BoardDimensionWindow()
        {
            InitializeComponent();
            
            var viewModel = new BoardDimensionsViewModel();
            this.DataContext = viewModel;

          
            viewModel.ConfirmAction = () =>
            {
                this.DialogResult = true;
                this.Close();
            };

            viewModel.CancelAction = () =>
            {
                this.DialogResult = false;
                this.Close();
            };
        }
    }
}
