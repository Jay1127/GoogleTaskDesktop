using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GoogleTaskDesktop.Core;

namespace GoogleTaskDesktop.ViewModel
{
    public class TaskItemViewModel : ViewModelBase, IReqeustItemEditable
    {
        public event EventHandler UpdatedRequested;
        public event EventHandler RemoveRequested;

        public bool NeedUpdate { get; set; }

        public string Title
        {
            get => Task.Title;
            set
            {
                if (Task.Title != value)
                {
                    Task.Title = value;
                    NeedUpdate = true;
                }
            }
        }

        public string Note
        {
            get => Task.Note;
            set
            {
                if(Task.Note != value)
                {
                    Task.Note = value;
                    NeedUpdate = true;  
                }
            }
        }

        public bool IsCompleted
        {
            get => Task.IsCompleted;
            set => Task.IsCompleted = value;
        }

        public TaskItem Task { get; }

        public ObservableCollection<TaskItemViewModel> SubTasks { get; private set; }

        /// <summary>
        /// 할일 세부사항 보여주기
        /// </summary>
        public RelayCommand ShowTaskDetailCommand { get; }

        public RelayCommand AddSubTaskItemCommand { get; }

        public RelayCommand RemoveCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand CloseCommand { get; }

        public TaskItemViewModel(TaskItem taskItem)
        {
            Task = taskItem;
            SubTasks = new ObservableCollection<TaskItemViewModel>(taskItem.SubItems.Select(t => new TaskItemViewModel(t)));

            //SubTasks = new ObservableCollection<TaskItem>(Task.SubItems);
            AddSubTaskItemCommand = new RelayCommand(AddSubTask);
            ShowTaskDetailCommand = new RelayCommand(ShowTaskDetail);
            CloseCommand = new RelayCommand(CloseTaskDetail);
            UpdateCommand = new RelayCommand(() => UpdatedRequested?.Invoke(this, EventArgs.Empty));
            RemoveCommand = new RelayCommand(() => RemoveRequested?.Invoke(this, EventArgs.Empty));
        }

        private void AddSubTask()
        {
            SubTasks.Add(new TaskItemViewModel(new TaskItem(string.Empty)
            {
                ParentTask = Task.Id,
                CategoryId = Task.CategoryId
            }));
        }

        private void CloseTaskDetail()
        {
            var flyoutViewModel = SimpleIoc.Default.GetInstance<TaskDetailFlyoutViewModel>();
            flyoutViewModel.Close();
        }

        private void ShowTaskDetail()
        {
            var flyoutViewModel = SimpleIoc.Default.GetInstance<TaskDetailFlyoutViewModel>();
            flyoutViewModel.ShowDetail(this);
        }
    }
}