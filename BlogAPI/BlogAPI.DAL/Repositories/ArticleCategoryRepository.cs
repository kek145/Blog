using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class ArticleCategoryRepository : IBaseRepository<ArticleCategoryEntity>
{
    private readonly ApplicationDbContext _context;

    public ArticleCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ArticleCategoryEntity entity)
    {
        await _context.ArticleCategory.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<ArticleCategoryEntity> GetAll()
    {
        return _context.ArticleCategory;
    }

    public async Task<ArticleCategoryEntity> UpdateAsync(ArticleCategoryEntity entity)
    {
        _context.ArticleCategory.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<List<ArticleCategoryEntity>> GetAllById(int entityId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<ArticleCategoryEntity>> DeleteAllAsync(IEnumerable<ArticleCategoryEntity> entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(ArticleCategoryEntity entity)
    {
        _context.ArticleCategory.Remove(entity);
        await _context.SaveChangesAsync();
    }
}