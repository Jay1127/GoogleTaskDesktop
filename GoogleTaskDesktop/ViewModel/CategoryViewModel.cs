using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

    public class CategoryViewModel : ViewModelBase
    {
        private TaskFilterMode _filterMode;

        /// <summary>
        /// 바인딩된 카테고리
        /// </summary>
        public ICategory Category { get; }

        public ObservableCollection<TaskItem> Tasks { get; }

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

        public event EventHandler CategoryRenameRequested;
        public event EventHandler CategoryRemoveRequested;

        public CategoryViewModel(ICategory category)
        {
            Category = category;

            Tasks = new ObservableCollection<TaskItem>(Category.GetTasks());
            ShowNewTaskCommand = new RelayCommand(ShowNewTaskPopup);
            RenameCategoryCommand = new RelayCommand(() => CategoryRenameRequested ?.Invoke(this, EventArgs.Empty));
            RemoveCategoryCommand = new RelayCommand(() => CategoryRemoveRequested?.Invoke(this, EventArgs.Empty));
        }

        private void ShowNewTaskPopup()
        {
            var popup = ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            popup.Show("New Task", "Enter new task name");
            popup.Updated += CategoryUpdatedAsync; ;
        }

        private async System.Threading.Tasks.Task CategoryUpdatedAsync(string updatedName)
        {
            await Category.AddTaskAsync(new TaskItem(updatedName));
            Tasks.Add(Category.GetTasks().Last());

            var popup = ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            popup.Updated -= CategoryUpdatedAsync;
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