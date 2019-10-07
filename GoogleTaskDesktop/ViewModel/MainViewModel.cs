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
        /// UI Dialog ȣ�� ��ü
        /// </summary>
        private IDialogCoordinator _dialogCoordinator;
        private CategoryListViewModel _categoryListViewModel;

        /// <summary>
        /// ī�װ� ����Ʈ
        /// </summary>
        public ICategoryList Categories { get; }

        /// <summary>
        /// ī�װ� ����Ʈ(���)
        /// </summary>
        public CategoryListViewModel CategoryListViewModel
        {
            get => _categoryListViewModel;
            set => Set(ref _categoryListViewModel, value);
        }

        /// <summary>
        /// �ʱ� ȭ�� Load�� ȣ��(�ʱ� ������ �ε�)
        /// </summary>
        public RelayCommand LoadCommand { get; }

        /// <summary>
        /// ��� ī�װ� �����ֱ� ��ư Ŭ���� ȣ��
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
        /// �ʱ� ������ �ε�
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
        /// ī�װ� �˾� �����ֱ�
        /// </summary>
        private void ShowCategory()
        {
            CategoryListViewModel.ShowPopup();
        }

    }
}