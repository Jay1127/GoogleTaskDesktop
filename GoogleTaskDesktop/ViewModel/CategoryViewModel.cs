using GalaSoft.MvvmLight;
using GoogleTaskDesktop.Core;
using System.Collections.ObjectModel;

namespace GoogleTaskDesktop.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
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

        public CategoryViewModel(ICategory category)
        {
            Category = category;

            Tasks = new ObservableCollection<TaskItem>(Category.GetTasks());
        }
    }
}