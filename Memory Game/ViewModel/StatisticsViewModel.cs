using Memory_Game.Model;
using Memory_Game.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Memory_Game.ViewModel
{
    public class StatisticsViewModel :BaseViewModel
    {
        private ObservableCollection<PlayerStatisticsModel> _playerStatistics;
        public ICommand CloseWindowCommand { get; }
        public ObservableCollection<PlayerStatisticsModel> PlayerStatistics
        {
            get { return _playerStatistics; }
            set
            {
                _playerStatistics = value;
                OnPropertyChanged();
            }
        }

        public StatisticsViewModel()
        {
            var stats = PlayerStatisticsService.LoadStatistics();
            PlayerStatistics = new ObservableCollection<PlayerStatisticsModel>(stats.OrderBy(s => s.Username));
            CloseWindowCommand = new RelayCommand(CloseWindowExecute);
           
        }

        private void CloseWindowExecute()
        {
            Application.Current.Windows.OfType<StatisticsWindow>().FirstOrDefault()?.Close();
        }



    }
}
