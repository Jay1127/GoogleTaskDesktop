using GalaSoft.MvvmLight;
using GoogleTaskDesktop.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public CategoryViewModel(ICategory category)
        {
            Category = category;

            Tasks = new ObservableCollection<TaskItem>(Category.GetTasks());
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