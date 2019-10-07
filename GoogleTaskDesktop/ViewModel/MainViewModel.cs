using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoogleTaskDesktop.Core;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;

namespace GoogleTaskDesktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// UI Dialog 호출 객체
        /// </summary>
        private IDialogCoordinator _dialogCoordinator;
        private CategoryListViewModel _categoryListViewModel;

        /// <summary>
        /// 카테고리 리스트
        /// </summary>
        public ICategoryList Categories { get; }

        /// <summary>
        /// 카테고리 리스트(뷰모델)
        /// </summary>
        public CategoryListViewModel CategoryListViewModel
        {
            get => _categoryListViewModel;
            set => Set(ref _categoryListViewModel, value);
        }

        /// <summary>
        /// 초기 화면 Load시 호출(초기 데이터 로딩)
        /// </summary>
        public RelayCommand LoadCommand { get; }

        /// <summary>
        /// 상단 카테고리 보여주기 버튼 클릭시 호출
        /// </summary>
        public RelayCommand ShowCategoriesCommand { get; }

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            Categories = new CategoryList();

            LoadCommand = new RelayCommand(async () => await LoadAsync());
            ShowCategoriesCommand = new RelayCommand(() => ShowCategory());
        }

        /// <summary>
        /// 초기 데이터 로딩
        /// </summary>
        /// <returns></returns>
        private async Task LoadAsync()
        {
            var controller = await _dialogCoordinator.ShowProgressAsync(this, "Syncing", "Load google task...");
            controller.SetIndeterminate();

            await Categories.LoadAsync();

            CategoryListViewModel = new CategoryListViewModel(Categories);

            await controller.CloseAsync();
        }

        /// <summary>
        /// 카테고리 팝업 보여주기
        /// </summary>
        private void ShowCategory()
        {
            CategoryListViewModel.ShowPopup();
        }

    }
}