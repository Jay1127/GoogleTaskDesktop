using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleTaskDesktop.Core
{
    public interface ICategoryList
    {
        Task LoadAsync();
        Task AddCategoryAsync(ICategory category);
        List<ICategory> GetCategories();
        ICategory FindCategory(string categoryId);
        Task RemoveCategoryAsync(string categoryId);
        Task UpdateCategoryAsync(ICategory category);
    }
}