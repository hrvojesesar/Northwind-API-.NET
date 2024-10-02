using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int? id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> EditCategoryAsync(Category category)
    {
        var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == category.CategoryID);

        if (existingCategory == null)
        {
            return null;
        }

        existingCategory.CategoryName = category.CategoryName;
        existingCategory.Description = category.Description;
        existingCategory.Picture = category.Picture;

        _context.Categories.Update(existingCategory);
        await _context.SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteCategoryAsync(int? id)
    {
        var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);

        if (existingCategory == null)
        {
            return false;
        }

        _context.Categories.Remove(existingCategory);
        await _context.SaveChangesAsync();
        return true;
    }
}