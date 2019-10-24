using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoogleTaskDesktop.Core;
using MahApps.Metro.Controls.Dialogs;
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
                Categories.GetCategories().Select(category =>
                {
                    var categoryViewModel = new CategoryViewModel(category);
                    RegisterCategoryEvent(categoryViewModel);

                    return categoryViewModel;
                }));

            CurrentCategory = CategoryViewModels.FirstOrDefault();

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

            var popup = ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            popup.Show("New Category", "Enter new category name");
            popup.Updated += CategoryUpdatedAsync;
        }

        private async Task CategoryUpdatedAsync(string titleToUpdated)
        {
            // 처음 생성하는 경우 ID를 부여안함.
            // ID는 서버에서 자동으로 부여함.(미리 부여하면 에러발생함.)
            await Categories.AddCategoryAsync(new Category(titleToUpdated));
            
            CategoryViewModels.Add(new CategoryViewModel(Categories.GetCategories().Last()));

            CurrentCategory = CategoryViewModels.Last();

            RegisterCategoryEvent(CurrentCategory);

            var popup = ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            popup.Updated -= CategoryUpdatedAsync;
        }

        private void RenameCategory()
        {

        }

        private async void RemoveCategoryAsync(object sender, EventArgs e)
        {

            if (System.Windows.MessageBox.Show("Remove category?", "Remove category", System.Windows.MessageBoxButton.OKCancel) 
                == System.Windows.MessageBoxResult.OK)
            {
                var categoryViewModel = sender as CategoryViewModel;

                // remove event
                await Categories.RemoveCategoryAsync(categoryViewModel.Category.Id);

                CategoryViewModels.Remove(categoryViewModel);

                // CurrentCategory가 지워진 경우
                if(CurrentCategory == null)
                {
                    CurrentCategory = CategoryViewModels.FirstOrDefault();
                }

                UnRegisterCategoryEvent(categoryViewModel);
            }

        }

        private void RenameCateogryAsync(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RegisterCategoryEvent(CategoryViewModel categoryViewModel)
        {
            categoryViewModel.CategoryRenameRequested += RenameCateogryAsync;
            categoryViewModel.CategoryRemoveRequested += RemoveCategoryAsync;
        }

        private void UnRegisterCategoryEvent(CategoryViewModel categoryViewModel)
        {
            categoryViewModel.CategoryRenameRequested -= RenameCateogryAsync;
            categoryViewModel.CategoryRemoveRequested -= RemoveCategoryAsync;
        }
    }
}