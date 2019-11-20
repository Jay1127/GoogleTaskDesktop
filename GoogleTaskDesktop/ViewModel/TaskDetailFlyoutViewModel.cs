using GalaSoft.MvvmLight;

namespace GoogleTaskDesktop.ViewModel
{
    public class TaskDetailFlyoutViewModel : ViewModelBase
    {
        private bool _isOpen;
        private TaskItemViewModel _task;

        public bool IsOpen
        {
            get => _isOpen;
            set => Set(ref _isOpen, value);
        }

        public TaskItemViewModel Task
        {
            get => _task;
            set => Set(ref _task, value);
        }

        public TaskDetailFlyoutViewModel()
        {

        }

        public void ShowDetail(TaskItemViewModel task)
        {
            IsOpen = true;
            Task = task;
        }

        public void Close()
        {
            IsOpen = false;
            Task = null;
        }
    }
}