using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;



namespace Memory_Game
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            DataContext = new StartViewModel();
           
        }

    }
}
