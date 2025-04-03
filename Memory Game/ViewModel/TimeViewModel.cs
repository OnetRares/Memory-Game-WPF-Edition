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
                CustomMessageViewModel.ShowTimeMessage(TimeLimit);
                ConfirmAction?.Invoke();
            }
            else
            {
                CustomMessageViewModel.ShowTimeEroorMessage();
            }
        }

        private void CancelSelection()
        {
            CustomMessageViewModel.ShowTimeCancelMessage(); 
            CancelAction?.Invoke();
        }
    }
}
