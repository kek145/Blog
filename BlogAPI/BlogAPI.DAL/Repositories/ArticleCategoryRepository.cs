using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class ArticleCategoryRepository : IRelationShipRepository<ArticleCategoryEntity>
{
    private readonly ApplicationDbContext _context;

    public ArticleCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRelationShipAsync(ArticleCategoryEntity entity)
    {
        await _context.ArticleCategory.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}