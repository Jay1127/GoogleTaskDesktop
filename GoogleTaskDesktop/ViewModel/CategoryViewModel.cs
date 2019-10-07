using GalaSoft.MvvmLight;
using GoogleTaskDesktop.Core;

namespace GoogleTaskDesktop.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        public ICategory Category { get; }

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
        }
    }
}