using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.ArticleRepository;

public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<ArticleEntity> GetAllArticles()
    {
        return _context.Article;
    }

    public async Task CreateArticleAsync(ArticleEntity entity)
    {
        await _context.Article.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArticleAsync(ArticleEntity entity)
    {
        _context.Article.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(ArticleEntity entity)
    {
        _context.Article.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ArticleEntity> FindArticleByIdAsync(int articleId)
    {
        var article = await _context.Article.FindAsync(articleId);
        return article!;
    }

    public async Task<ArticleEntity> FindArticleByTitleAsync(string title)
    {
        var article = await _context.Article.Where(find => find.Title == title).FirstOrDefaultAsync();
        return article!;
    }
}