using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Repositories;

public class CategoryRepository : IBaseRepository<CategoryEntity>
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(CategoryEntity entity)
    {
        await _context.Category.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<CategoryEntity> GetAll()
    {
        return _context.Category;
    }

    public async Task<CategoryEntity> UpdateAsync(CategoryEntity entity)
    {
        _context.Category.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(CategoryEntity entity)
    {
        _context.Category.Remove(entity);
        await _context.SaveChangesAsync();
    }
}