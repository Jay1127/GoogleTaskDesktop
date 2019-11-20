using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GoogleTaskDesktop.Core;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GoogleTaskDesktop.ViewModel
{
    public enum TaskFilterMode
    {
        All,
        InProccess,
        Completed
    }

    public class CategoryViewModel : ViewModelBase, IReqeustItemEditable
    {
        private TaskFilterMode _filterMode;
        private TaskItemViewModel _currentTask;

        /// <summary>
        /// 바인딩된 카테고리
        /// </summary>
        public ICategory Category { get; }

        //public ObservableCollection<TaskItem> Tasks { get; }

        public ObservableCollection<TaskItemViewModel> Tasks { get; }

        public TaskItemViewModel CurrentTask
        {
            get => _currentTask;
            set => Set(ref _currentTask, value);
        }

        /// <summary>
        /// 뷰에 표시할 제목
        /// </summary>
        public string Title
        {
            get
            {
                return $"{Category.Title} ({Category.GetTasks().Count})";
            }
        }

        public TaskFilterMode FilterMode
        {
            get => _filterMode;
            set
            {
                Set(ref _filterMode, value);

                ChangeViewMode();
            }
        }

        public RelayCommand ShowNewTaskCommand { get; }

        /// <summary>
        /// 카테고리 이름 수정하기
        /// </summary>
        public RelayCommand RenameCategoryCommand { get; }

        /// <summary>
        /// 카테고리 삭제하기
        /// </summary>
        public RelayCommand RemoveCategoryCommand { get; }

        public event EventHandler UpdatedRequested;
        public event EventHandler RemoveRequested;

        public CategoryViewModel(ICategory category)
        {
            Category = category;

            //Tasks = new ObservableCollection<TaskItem>(Category.GetTasks());
            Tasks = new ObservableCollection<TaskItemViewModel>(
                Category.GetTasks().Select(t => new TaskItemViewModel(t)));

            foreach(var task in Tasks)
            {
                task.UpdatedRequested += OnTaskUpdatedReqeust;
                task.RemoveRequested += OnTaskRemoveRequested;
            }

            ShowNewTaskCommand = new RelayCommand(ShowNewTaskPopup);
            RenameCategoryCommand = new RelayCommand(() => UpdatedRequested?.Invoke(this, EventArgs.Empty));
            RemoveCategoryCommand = new RelayCommand(() => RemoveRequested?.Invoke(this, EventArgs.Empty));
        }

        private void ShowNewTaskPopup()
        {
            var popup = ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            popup.Show("New Task", "Enter new task name");
            popup.Updated += CategoryUpdatedAsync; ;
        }

        private async Task CategoryUpdatedAsync(string updatedName)
        {
            await Category.AddTaskAsync(new TaskItem(updatedName));
            //Tasks.Add(Category.GetTasks().Last());
            Tasks.Add(new TaskItemViewModel(Category.GetTasks().Last()));

            var popup = ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            popup.Updated -= CategoryUpdatedAsync;
        }

        private async void OnTaskRemoveRequested(object sender, EventArgs e)
        {
            var flyout = ServiceLocator.Current.GetInstance<TaskDetailFlyoutViewModel>();
            flyout.Close();

            var taskVM = (sender as TaskItemViewModel);
            var task = taskVM.Task;

            await Category.RemoveTaskAsync(task.Id);

            Tasks.Remove(taskVM);
            taskVM.SubTasks.Clear();
            task.SubItems.Clear();
        }

        private async void OnTaskUpdatedReqeust(object sender, EventArgs e)
        {
            var taskVM = (sender as TaskItemViewModel);
            var task = taskVM.Task;

            // 새로 추가된 서브 아이템
            var tasksToUdpate = taskVM.SubTasks
                                        .Where(t => string.IsNullOrEmpty(t.Task.Id))
                                        .Select(t => t.Task);

            foreach(var item in tasksToUdpate)
            {
                await Category.AddTaskAsync(item);

                task.SubItems.Add(Category.GetTasks().Last());
            }

            // Task 제목 수정
            await UpdateTask(taskVM);

            // SubTask 제목 수정
            foreach(var item in taskVM.SubTasks)
            {
                await UpdateTask(item);
            }
        }

        private async Task UpdateTask(TaskItemViewModel taskItemViewModel)
        {
            if (taskItemViewModel.NeedUpdate)
            {
                await Category.UpdateTaskAsync(taskItemViewModel.Task);

                taskItemViewModel.NeedUpdate = false;
            }
        }

        private void ChangeViewMode()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Tasks);

            if (_filterMode == TaskFilterMode.Completed)
            {
                collectionView.Filter = (item) => (item as TaskItem).IsCompleted;
            }
            else if (_filterMode == TaskFilterMode.InProccess)
            {
                collectionView.Filter = (item) => !(item as TaskItem).IsCompleted;
            }
            else
            {
                collectionView.Filter = null;
            }
        }
    }
}