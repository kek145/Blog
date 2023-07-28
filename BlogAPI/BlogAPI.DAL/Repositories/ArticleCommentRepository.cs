using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class ArticleCommentRepository : IRelationShipRepository<ArticleCommentEntity>
{
    private readonly ApplicationDbContext _context;

    public ArticleCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRelationShipAsync(ArticleCommentEntity entity)
    {
        await _context.ArticleComment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}