using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Memory_Game.Model;

namespace Memory_Game.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryModel> Categories { get; set; }
        public CategoryModel SelectedCategory { get; set; }

        public ICommand ConfirmSelectionCommand { get; }
        public ICommand CancelCommand { get; }

     
        public Action ConfirmAction { get; set; }
        public Action CancelAction { get; set; }

        public CategoryViewModel()
        {
            Categories = new ObservableCollection<CategoryModel>
            {
                new CategoryModel { Name = "Animals" },
                new CategoryModel { Name = "Cars" },
                new CategoryModel { Name = "Cartoons" },
                new CategoryModel { Name = "Pony" }
            };

            ConfirmSelectionCommand = new RelayCommand(ConfirmSelection);
            CancelCommand = new RelayCommand(CancelSelection);
        }

        private void ConfirmSelection()
        {
            if (SelectedCategory != null)
            {
               
                ConfirmAction?.Invoke();
            }
            else
            {
                CustomMessageViewModel.ShowSelectedCategoryErrorMessage();
            }
        }

        private void CancelSelection()
        {
           
            CancelAction?.Invoke();
        }
    }
}
