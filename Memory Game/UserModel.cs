using System.ComponentModel;

namespace Memory_Game
{
    public class UserModel : BaseViewModel
    {
        private string _name;
        private string _imagePath;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
    }
}
