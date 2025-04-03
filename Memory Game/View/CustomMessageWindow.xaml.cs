using System;
using System.CodeDom.Compiler;
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

    public partial class CustomMessageWindow : Window
    {

        public ICommand OkCommand { get; }
        public CustomMessageWindow()
        {
            InitializeComponent();
            OkCommand = new RelayCommand(ExecuteOk);
            DataContext = this;
        }

      
        public string Message
        {
            get => MessageTextBlock.Text;
            set => MessageTextBlock.Text = value;
        }

        private void ExecuteOk()
        {
            this.DialogResult = true;
            Close();
        }
    }
}
