using System.Windows;
using System.Windows.Input;
using Memory_Game.Model;

namespace Memory_Game.ViewModel
{
    public class TimeViewModel : BaseViewModel
    {
        private TimeModel _timeModel;

        public TimeModel TimeModel
        {
            get { return _timeModel; }
            set
            {
                _timeModel = value;
                OnPropertyChanged(nameof(TimeModel));
            }
        }

        public int TimeLimit
        {
            get { return _timeModel.TimeLimit; }
            set
            {
                _timeModel.TimeLimit = value;
                OnPropertyChanged(nameof(TimeLimit));
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public Action ConfirmAction { get; set; }
        public Action CancelAction { get; set; }

        public TimeViewModel()
        {
            _timeModel = new TimeModel { TimeLimit = 60 };
            ConfirmCommand = new RelayCommand(ConfirmSelection);
            CancelCommand = new RelayCommand(CancelSelection);
        }

        private void ConfirmSelection()
        {
            if (TimeLimit > 0)
            {
                MessageBox.Show($"Game will start with {TimeLimit} seconds.", "Time Set", MessageBoxButton.OK, MessageBoxImage.Information);
                ConfirmAction?.Invoke();
            }
            else
            {
                MessageBox.Show("The entered time is not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelSelection()
        {
            MessageBox.Show("Time limit selection canceled.", "Cancel", MessageBoxButton.OK, MessageBoxImage.Information);
            CancelAction?.Invoke();
        }
    }
}
