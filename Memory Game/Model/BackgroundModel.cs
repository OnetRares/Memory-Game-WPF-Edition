using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Memory_Game.ViewModel;

namespace Memory_Game
{
    public class BackgroundModel:BaseViewModel
    {
        public string BackgroundImagePath { get; }

        public BackgroundModel()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            BackgroundImagePath = Path.Combine(baseDir, "..", "..", "..", "Resources", "background.png");
            BackgroundImagePath = Path.GetFullPath(BackgroundImagePath);
        }

        public ImageBrush GetBackgroundBrush()
        {
            if (File.Exists(BackgroundImagePath))
            {
                return new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(BackgroundImagePath, UriKind.Absolute)),
                    Stretch = Stretch.Fill
                };
            }

            return new ImageBrush(null); 
        }
    }
}
