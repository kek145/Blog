using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Repositories;

public class ArticleRepository : IBaseRepository<ArticleEntity>
{
    private readonly ApplicationDbContext _context;

    public ArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ArticleEntity entity)
    {
        await _context.Article.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<ArticleEntity> GetAll()
    {
        return _context.Article;
    }

    public async Task<ArticleEntity> UpdateAsync(ArticleEntity entity)
    {
        _context.Article.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(ArticleEntity entity)
    {
        _context.Article.Remove(entity);
        await _context.SaveChangesAsync();
    }
}