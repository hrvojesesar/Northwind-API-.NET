using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int? id);
    Task<Category> AddCategoryAsync(Category category);
    Task<Category> EditCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int? id);
}