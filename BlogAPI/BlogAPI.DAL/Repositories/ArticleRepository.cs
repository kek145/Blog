using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.Repositories;

public class ArticleRepository : IBaseRepository<ArticleEntity>, IArticleRepository
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

    public async Task<List<ArticleEntity>> GetAllById(int entityId)
    {
        var articles = await GetAll()
            .Where(find => find.UserArticle.Any(commentsArticle => commentsArticle.ArticleId == entityId))
            .ToListAsync();

        return articles;
    }

    public async Task<List<ArticleEntity>> GetAllByUserId(int userId)
    {
        var articles = await GetAll()
            .Where(find => find.UserArticle.Any(userArticles => userArticles.UserId == userId))
            .ToListAsync();
        return articles;
    }

    public async Task<List<ArticleEntity>> DeleteAllAsync(IEnumerable<ArticleEntity> entity)
    {
        _context.Article.RemoveRange(entity);
        await _context.SaveChangesAsync();

        return await _context.Article.ToListAsync();
    }

    public async Task DeleteAsync(ArticleEntity entity)
    {
        _context.Article.Remove(entity);
        await _context.SaveChangesAsync();
    }
}