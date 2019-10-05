using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleTaskDesktop.Core
{
    public interface ICategoryList
    {
        Task LoadAsync();
        Task AddCategoryAsync(Category category);
        List<Category> GetCategories();
        Category FindCategory(string categoryId);
        Task RemoveCategoryAsync(string categoryId);
        Task UpdateCategoryAsync(Category category);
    }
}