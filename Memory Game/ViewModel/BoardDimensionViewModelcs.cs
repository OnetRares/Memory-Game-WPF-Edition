using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Memory_Game.Model;

namespace Memory_Game.ViewModel
{
    public class BoardDimensionsViewModel : BaseViewModel
    {
  
        public ObservableCollection<int> RowOptions { get; }
        public ObservableCollection<int> ColumnOptions { get; }

        private int selectedRow;
        public int SelectedRow
        {
            get => selectedRow;
            set
            {
                selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private int selectedColumn;
        public int SelectedColumn
        {
            get => selectedColumn;
            set
            {
                selectedColumn = value;
                OnPropertyChanged(nameof(SelectedColumn));
            }
        }

        
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

       
        public Action ConfirmAction { get; set; }
        public Action CancelAction { get; set; }

        public BoardDimensionsViewModel()
        {
         
            RowOptions = new ObservableCollection<int> { 2, 3, 4, 5, 6 };
            ColumnOptions = new ObservableCollection<int> { 2, 3, 4, 5, 6 };

           
            SelectedRow = 4;
            SelectedColumn = 4;

            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Confirm()
        {
            
            var boardModel = new BoardDimensionsModel(SelectedRow, SelectedColumn);
            if (!boardModel.IsValid())
            {
                MessageBox.Show("The product of rows and columns must be even.", "Invalid dimensions", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ConfirmAction?.Invoke();
        }

        private void Cancel()
        {
            CancelAction?.Invoke();
        }
    }
}
