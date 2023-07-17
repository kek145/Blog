using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.CategoryRepository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<CategoryEntity> GetAllCategories()
    {
        return _context.Category;
    }

    public async Task CreateCategoryAsync(CategoryEntity entity)
    {
        await _context.Category.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(CategoryEntity entity)
    {
        _context.Category.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(CategoryEntity entity)
    {
        _context.Category.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<CategoryEntity> FindCategoryByIdAsync(int categoryId)
    {
        var category = await _context.Category.FindAsync(categoryId);
        return category!;
    }

    public async Task<CategoryEntity> FindCategoryByNameAsync(string categoryName)
    {
        var category = await _context.Category.Where(find => find.CategoryName == categoryName).FirstOrDefaultAsync();
        return category!;
    }
}