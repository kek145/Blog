using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.ArticleCategoryRepository;

public class ArticleCategoryRepository : IArticleCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<ArticleCategoryEntity> GetAllArticleCategories()
    {
        return _context.ArticleCategory;
    }

    public async Task AddArticleCategoryAsync(ArticleCategoryEntity entity)
    {
        await _context.ArticleCategory.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ArticleCategoryEntity> FindArticleCategoryByIdAsync(int articleId, int categoryId)
    {
        var articleCategory = await _context.ArticleCategory.Where(find => find.ArticleId == articleId && find.CategoryId == categoryId).FirstOrDefaultAsync();
        return articleCategory!;
    }
}