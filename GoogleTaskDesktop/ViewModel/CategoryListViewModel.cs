using GalaSoft.MvvmLight;
using GoogleTaskDesktop.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace GoogleTaskDesktop.ViewModel
{
    public class CategoryListViewModel : ViewModelBase
    {
        private bool _isOpen;
        private CategoryViewModel _currentCategory;

        /// <summary>
        /// 카테고리 리스트
        /// </summary>
        public ICategoryList Categories { get; }

        /// <summary>
        /// 카테고리 리스트(뷰 모델) 
        /// </summary>
        public ObservableCollection<CategoryViewModel> CategoryViewModels { get; }

        /// <summary>
        /// 선택된 카테고리
        /// </summary>
        public CategoryViewModel CurrentCategory
        {
            get => _currentCategory;
            set
            {
                Set(ref _currentCategory, value);

                IsOpen = false;
            }
        }

        /// <summary>
        /// 현재 카테고리 리스트가 뷰에 보여지는지 여부
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set => Set(ref _isOpen, value);
        }

        public CategoryListViewModel(ICategoryList categories)
        {
            Categories = categories;
            CategoryViewModels = new ObservableCollection<CategoryViewModel>(
                Categories.GetCategories().Select(category => new CategoryViewModel(category)));
        }

        /// <summary>
        /// 카테고리 팝업 호출
        /// </summary>
        public void ShowPopup()
        {
            IsOpen = true;
        }
    }
}