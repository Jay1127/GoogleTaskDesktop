using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;

namespace GoogleTaskDesktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// UI Dialog ȣ�� ��ü
        /// </summary>
        private IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// �ʱ� ȭ�� Load�� ȣ��(�ʱ� ������ �ε�)
        /// </summary>
        public RelayCommand LoadCommand { get; }

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            LoadCommand = new RelayCommand(async () => await LoadAsync());
        }

        private async System.Threading.Tasks.Task LoadAsync()
        {
            var controller = await _dialogCoordinator.ShowProgressAsync(this, "Syncing", "Load google task...");
            controller.SetIndeterminate();
            await Task.Delay(3000);

            await controller.CloseAsync();
        }
    }
}