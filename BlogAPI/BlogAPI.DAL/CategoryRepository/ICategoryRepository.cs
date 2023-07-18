using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.CategoryRepository;

public interface ICategoryRepository
{
    IQueryable<CategoryEntity> GetAllCategories();
    Task AddCategoryAsync(CategoryEntity entity);
    Task UpdateCategoryAsync(CategoryEntity entity);
    Task DeleteCategoryAsync(CategoryEntity entity);
    Task<CategoryEntity> FindCategoryByIdAsync(int categoryId);
    Task<CategoryEntity> FindCategoryByNameAsync(string categoryName);
}