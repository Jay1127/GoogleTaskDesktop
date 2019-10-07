using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoogleTaskDesktop.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleTaskDesktop.ViewModel
{
    public class CategoryListViewModel : ViewModelBase
    {
        private bool _isCategoriesShown;
        private CategoryViewModel _currentCategory;
        private CategoryPopupViewModel _categoryPopupViewModel;

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

                IsCategoriesShown = false;
            }
        }

        /// <summary>
        /// 카테고리 팝업
        /// </summary>
        public CategoryPopupViewModel CategoryPopupViewModel
        {
            get => _categoryPopupViewModel;
            set => Set(ref _categoryPopupViewModel, value);
        }

        /// <summary>
        /// 현재 카테고리 리스트가 뷰에 보여지는지 여부
        /// </summary>
        public bool IsCategoriesShown
        {
            get => _isCategoriesShown;
            set => Set(ref _isCategoriesShown, value);
        }

        /// <summary>
        /// 새로운 카테고리 생성 팝업 보여주기
        /// </summary>
        public RelayCommand ShowNewCategoryCommand { get; }

        public CategoryListViewModel(ICategoryList categories)
        {
            Categories = categories;
            CategoryViewModels = new ObservableCollection<CategoryViewModel>(
                Categories.GetCategories().Select(category => new CategoryViewModel(category)));

            ShowNewCategoryCommand = new RelayCommand(ShowNewCategory);
        }

        /// <summary>
        /// 카테고리 팝업 호출
        /// </summary>
        public void ShowPopup()
        {
            IsCategoriesShown = true;            
        }

        /// <summary>
        /// 새로운 카테고리 만들기 팝업 호출
        /// </summary>
        private void ShowNewCategory()
        {
            IsCategoriesShown = false;

            if (CategoryPopupViewModel != null)
            {
                CategoryPopupViewModel.CategoryUpdated -= CategoryPopupViewModel_CategoryUpdatedAsync;
            }

            CategoryPopupViewModel = new CategoryPopupViewModel();
            CategoryPopupViewModel.CategoryUpdated += CategoryPopupViewModel_CategoryUpdatedAsync;

            CategoryPopupViewModel.ShowPopup();
        }

        private async Task CategoryPopupViewModel_CategoryUpdatedAsync(string titleToUpdated)
        {
            // 처음 생성하는 경우 ID를 부여안함.
            // ID는 서버에서 자동으로 부여함.(미리 부여하면 에러발생함.)
            await Categories.AddCategoryAsync(new Category(titleToUpdated));
            CategoryViewModels.Add(new CategoryViewModel(Categories.GetCategories().Last()));

            CurrentCategory = CategoryViewModels.Last();
        }
    }
}