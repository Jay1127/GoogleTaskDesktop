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
        /// UI Dialog 호출 객체
        /// </summary>
        private IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// 초기 화면 Load시 호출(초기 데이터 로딩)
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