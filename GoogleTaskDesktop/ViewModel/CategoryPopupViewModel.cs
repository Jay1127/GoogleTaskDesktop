using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GoogleTaskDesktop.ViewModel
{
    public class CategoryPopupViewModel : ViewModelBase
    {
        private bool _isShown;

        public bool IsShown
        {
            get => _isShown;
            set => Set(ref _isShown, value);
        }

        public string Title { get; set; }

        public RelayCommand AddCategoryCommand { get; }
        public RelayCommand CancelCommand { get; }

        public delegate Task CategoryUpdatedHandler(string titleToUpdated);

        public event CategoryUpdatedHandler CategoryUpdated;

        public CategoryPopupViewModel()
        {
            AddCategoryCommand = new RelayCommand(AddCategory);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void ShowPopup()
        {
            IsShown = true;
        }

        private void AddCategory()
        {
            CategoryUpdated?.Invoke(Title);
            IsShown = false;
        }

        private void Cancel()
        {
            IsShown = false;
        }
    }
}