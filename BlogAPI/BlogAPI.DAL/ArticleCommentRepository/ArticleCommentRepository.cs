using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.ArticleCommentRepository;

public class ArticleCommentRepository : IArticleCommentRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddArticleCommentAsync(ArticleCommentEntity entity)
    {
        await _context.ArticleComment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}