using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GoogleTaskDesktop.Core;

namespace GoogleTaskDesktop.ViewModel
{
    public class TaskItemViewModel : ViewModelBase
    {
        public string Title
        {
            get => Task.Title;
            set
            {
                // no... update...
                Task.Title = value;
            }
        }

        public TaskItem Task { get; }

        public ObservableCollection<TaskItem> SubTasks { get; private set; }

        /// <summary>
        /// 할일 세부사항 보여주기
        /// </summary>
        public RelayCommand ShowTaskDetailCommand { get; }

        public RelayCommand AddSubTaskItemCommand { get; set; }

        public TaskItemViewModel(TaskItem taskItem)
        {
            Task = taskItem;
            //SubTasks = new ObservableCollection<TaskItem>(Task.SubItems);
            //AddSubTaskItemCommand = new RelayCommand(AddSubTask);
            ShowTaskDetailCommand = new RelayCommand(ShowTaskDetail);
        }

        private void ShowTaskDetail()
        {
            var flyoutViewModel = SimpleIoc.Default.GetInstance<TaskDetailFlyoutViewModel>();
            flyoutViewModel.ShowDetail(this);
        }
    }
}